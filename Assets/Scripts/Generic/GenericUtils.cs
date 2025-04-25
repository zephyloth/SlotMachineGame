public static class GenericUtils
{
    public static string ColorizeText(object prefix, string color)
    {
        return $"<color={color}>{prefix.ToString()}</color>";
    }
}
