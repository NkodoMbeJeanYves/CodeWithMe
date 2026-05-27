using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace CodeWithMe.Core.Query;

/// <summary>
/// Options de requête extraites de la query-string, alignées au contrat EDU Platform v1
/// (section 5 — pagination, filtrage, tri).
/// </summary>
public sealed record QueryOptions(
    int Page,
    int Size,
    IReadOnlyList<SortSpec> Sort,
    string? Q,
    IReadOnlyList<string> Fields,
    IReadOnlyList<string> Include,
    IReadOnlyList<FilterSpec> Filters)
{
    public string? RawSort =>
        Sort.Count == 0
            ? null
            : string.Join(",", Sort.Select(s => s.Descending ? $"-{s.Field}" : s.Field));

    public static QueryOptions FromRequest(HttpRequest req)
    {
        var page = ParseInt(req.Query["page"], 1, min: 1);
        var size = ParseInt(req.Query["size"], 20, min: 1, max: 100);

        var sort = ParseSort(req.Query["sort"]);
        var q = req.Query["q"].ToString();
        if (string.IsNullOrWhiteSpace(q)) q = null;

        var fields = SplitCsv(req.Query["fields"]);
        var include = SplitCsv(req.Query["include"]);
        var filters = ParseFilters(req.Query);

        return new QueryOptions(page, size, sort, q, fields, include, filters);
    }

    private static int ParseInt(StringValues raw, int @default, int min = int.MinValue, int max = int.MaxValue)
    {
        if (int.TryParse(raw.ToString(), out var v))
            return Math.Clamp(v, min, max);
        return @default;
    }

    private static IReadOnlyList<SortSpec> ParseSort(StringValues raw)
    {
        var s = raw.ToString();
        if (string.IsNullOrWhiteSpace(s)) return Array.Empty<SortSpec>();
        return s.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(seg => seg.StartsWith('-')
                    ? new SortSpec(seg[1..], true)
                    : new SortSpec(seg, false))
                .ToArray();
    }

    private static IReadOnlyList<string> SplitCsv(StringValues raw)
    {
        var s = raw.ToString();
        return string.IsNullOrWhiteSpace(s)
            ? Array.Empty<string>()
            : s.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    private static IReadOnlyList<FilterSpec> ParseFilters(IQueryCollection query)
    {
        var filters = new List<FilterSpec>();
        foreach (var kv in query)
        {
            var key = kv.Key;
            if (!key.StartsWith("filter[", StringComparison.OrdinalIgnoreCase) || !key.EndsWith("]"))
                continue;

            var inner = key.Substring(7, key.Length - 8);
            string field;
            string op;

            var split = inner.IndexOf("][", StringComparison.Ordinal);
            if (split >= 0)
            {
                field = inner[..split];
                op = inner[(split + 2)..];
            }
            else
            {
                field = inner;
                op = "eq";
            }

            filters.Add(new FilterSpec(field, op.ToLowerInvariant(), kv.Value.ToString()));
        }
        return filters;
    }
}

public sealed record SortSpec(string Field, bool Descending);

public sealed record FilterSpec(string Field, string Op, string Value);
