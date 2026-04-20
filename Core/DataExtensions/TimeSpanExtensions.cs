namespace CodeWithMe.Core.DataExtensions;

public static class TimeSpanExtensions
{
    /// <summary>
    /// Convertit un TimeSpan en chaîne au format HH:mm.
    /// </summary>
    /// <param name="ts">Le TimeSpan à convertir</param>
    /// <returns>Chaîne formatée HH:mm</returns>
    public static string AsString(this TimeSpan ts)
    {
        return ts.ToString(@"hh\:mm");
    }
}
