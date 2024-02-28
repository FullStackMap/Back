namespace Map.API.Extension;

public static class EnumExtensions
{
    public static string ToStringValue(this Enum value)
    {
        return Convert.ToInt32(value).ToString("000");
    }
}
