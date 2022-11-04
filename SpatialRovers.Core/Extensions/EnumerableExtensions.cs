namespace SpatialRovers.Core.Extensions;

public static class EnumberableExtensions
{
    public static TEnum Next<TEnum>(this TEnum source)
        where TEnum : Enum
    {
        TEnum[] values = (TEnum[])Enum.GetValues(source.GetType());
        int nextIndex = Array.IndexOf(values, source) + 1;
        return nextIndex == values.Length
            ? values[0]
            : values[nextIndex];
    }
    public static TEnum Previous<TEnum>(this TEnum source)
        where TEnum : Enum
    {
        TEnum[] values = (TEnum[])Enum.GetValues(source.GetType());
        int nextIndex = Array.IndexOf(values, source) - 1;
        return nextIndex == -1
            ? values[^1]
            : values[nextIndex];
    }
}