namespace CodeWithMe.Core.DataExtensions;

public static class StringExtensions
{
    /// <summary>
    /// Convertit une chaîne au format HH:mm en TimeSpan.
    /// </summary>
    /// <param name="str">Chaîne au format HH:mm</param>
    /// <returns>TimeSpan correspondant</returns>
    /// <exception cref="ArgumentException">Si la chaîne n'est pas valide</exception>
    public static TimeSpan AsTimeSpan(this string str)
    {
        if (TimeSpan.TryParseExact(str, "hh\\:mm", null, out var value))
        {
            return value;
        }

        throw new ArgumentException($"Unable to parse '{str}' as TimeSpan (expected HH:mm format).", nameof(str));
    }
}


