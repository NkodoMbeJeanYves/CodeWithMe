using System.Collections;
using System.Linq.Expressions;
using System.Reflection;
using CodeWithMe.Core.Models.Envelope;
using CodeWithMe.Core.Query;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMe.Core.DataExtensions;

public static class LinQIQueryableExtensions
{
    /// <summary>
    /// Ancien pattern de pagination conservé pour rétro-compatibilité avec les controllers
    /// School/Period/Subject avant leur refactor au pattern canonique.
    /// </summary>
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query, int pageNumber, int pageSize)
    {
        var count = await query.CountAsync();
        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<T>(data, count, pageNumber, pageSize);
    }

    public class PagedResult<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PagedResult(IEnumerable<T> data, int count, int pageNumber, int pageSize)
        {
            Data = data;
            TotalItems = count;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
    }

    /// <summary>
    /// Applique pagination + tri + filtres + recherche selon le contrat EDU Platform v1 (section 5).
    /// Le `searchPredicate` est un callback qui définit les champs cherchés par `q` pour la ressource.
    /// Retourne les items + le <see cref="PageMeta"/>, prêt à passer à <c>ApiResults.Page</c>.
    /// </summary>
    public static async Task<(IReadOnlyList<T> Items, PageMeta Meta)> ToContractPageAsync<T>(
        this IQueryable<T> source,
        QueryOptions opts,
        Func<string, Expression<Func<T, bool>>>? searchPredicate = null,
        CancellationToken ct = default) where T : class
    {
        if (!string.IsNullOrEmpty(opts.Q) && searchPredicate is not null)
            source = source.Where(searchPredicate(opts.Q));

        foreach (var filter in opts.Filters)
        {
            var predicate = BuildFilterPredicate<T>(filter);
            if (predicate is not null)
                source = source.Where(predicate);
        }

        if (opts.Sort.Count > 0)
            source = ApplySort(source, opts.Sort);

        var total = await source.LongCountAsync(ct);
        var items = await source
            .Skip((opts.Page - 1) * opts.Size)
            .Take(opts.Size)
            .ToListAsync(ct);

        var totalPages = opts.Size == 0 ? 0 : (int)Math.Ceiling((double)total / opts.Size);
        return (items, new PageMeta(opts.Page, opts.Size, total, totalPages, opts.RawSort));
    }

    // ---- Tri dynamique ----

    private static IQueryable<T> ApplySort<T>(IQueryable<T> source, IReadOnlyList<SortSpec> sorts)
    {
        IOrderedQueryable<T>? ordered = null;
        foreach (var spec in sorts)
        {
            var prop = FindProperty<T>(spec.Field);
            if (prop is null) continue;

            var param = Expression.Parameter(typeof(T), "x");
            Expression body = Expression.Property(param, prop);
            // Cast vers object pour générer un Func<T, object> uniforme.
            var lambda = Expression.Lambda(Expression.Convert(body, typeof(object)), param);

            var methodName = ordered is null
                ? (spec.Descending ? "OrderByDescending" : "OrderBy")
                : (spec.Descending ? "ThenByDescending" : "ThenBy");

            var sourceExpr = ordered is null ? source.Expression : ordered.Expression;
            var call = Expression.Call(
                typeof(Queryable), methodName, new[] { typeof(T), typeof(object) },
                sourceExpr, Expression.Quote(lambda));

            ordered = (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
        }
        return ordered ?? source;
    }

    // ---- Filtre dynamique ----

    private static Expression<Func<T, bool>>? BuildFilterPredicate<T>(FilterSpec spec)
    {
        var prop = FindProperty<T>(spec.Field);
        if (prop is null) return null;

        var param = Expression.Parameter(typeof(T), "x");
        Expression member = Expression.Property(param, prop);
        var memberType = prop.PropertyType;

        try
        {
            Expression body = spec.Op switch
            {
                "eq" => Expression.Equal(member, ConstFor(spec.Value, memberType)),
                "neq" => Expression.NotEqual(member, ConstFor(spec.Value, memberType)),
                "gt" => Expression.GreaterThan(member, ConstFor(spec.Value, memberType)),
                "gte" => Expression.GreaterThanOrEqual(member, ConstFor(spec.Value, memberType)),
                "lt" => Expression.LessThan(member, ConstFor(spec.Value, memberType)),
                "lte" => Expression.LessThanOrEqual(member, ConstFor(spec.Value, memberType)),
                "like" => BuildLike(member, spec.Value),
                "in" => BuildIn(member, spec.Value, memberType, negate: false),
                "nin" => BuildIn(member, spec.Value, memberType, negate: true),
                _ => null!
            };

            if (body is null) return null;
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
        catch
        {
            // Valeur impossible à convertir → on ignore silencieusement plutôt que de 500.
            return null;
        }
    }

    private static Expression ConstFor(string raw, Type targetType)
    {
        var underlying = Nullable.GetUnderlyingType(targetType) ?? targetType;
        object value;
        if (underlying.IsEnum)
            value = Enum.Parse(underlying, raw, ignoreCase: true);
        else if (underlying == typeof(Guid))
            value = Guid.Parse(raw);
        else if (underlying == typeof(DateTime))
            value = DateTime.Parse(raw, System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.AssumeUniversal | System.Globalization.DateTimeStyles.AdjustToUniversal);
        else if (underlying == typeof(bool))
            value = bool.Parse(raw);
        else
            value = Convert.ChangeType(raw, underlying, System.Globalization.CultureInfo.InvariantCulture);

        return Expression.Constant(value, targetType);
    }

    private static Expression BuildLike(Expression member, string value)
    {
        // EF Core traduira en SQL LIKE via EF.Functions.Like si possible.
        // Ici on utilise String.Contains qui est traduit en LIKE %x% par EF.
        if (member.Type != typeof(string))
            throw new InvalidOperationException("like opérator only applies to string columns.");
        var method = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
        return Expression.Call(member, method, Expression.Constant(value));
    }

    private static Expression BuildIn(Expression member, string csv, Type memberType, bool negate)
    {
        var raws = csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var underlying = Nullable.GetUnderlyingType(memberType) ?? memberType;
        var listType = typeof(List<>).MakeGenericType(underlying);
        var list = (IList)Activator.CreateInstance(listType)!;
        foreach (var r in raws)
        {
            var constExpr = (ConstantExpression)ConstFor(r, underlying);
            list.Add(constExpr.Value!);
        }
        var containsMethod = typeof(Enumerable).GetMethods()
            .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
            .MakeGenericMethod(underlying);
        Expression body = Expression.Call(null, containsMethod, Expression.Constant(list), member);
        return negate ? Expression.Not(body) : body;
    }

    // ---- Recherche propriété insensible à la casse + tolérance camelCase/PascalCase ----

    private static readonly System.Collections.Concurrent.ConcurrentDictionary<(Type, string), PropertyInfo?> _propCache = new();

    private static PropertyInfo? FindProperty<T>(string name)
    {
        return _propCache.GetOrAdd((typeof(T), name), key =>
        {
            var (t, n) = key;
            return t.GetProperty(n, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        });
    }
}
