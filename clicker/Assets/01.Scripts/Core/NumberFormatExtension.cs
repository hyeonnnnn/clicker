
public static class NumberFormatExtension
{
    private static string[] _suffixes =
{
        "", "K", "M", "B", "T",
        "aa", "ab", "ac", "ad", "ae", "af", "ag", "ah", "ai", "aj",
        "ak", "al", "am", "an", "ao", "ap", "aq", "ar", "as", "at",
        "au", "av", "aw", "ax", "ay", "az",
        "ba", "bb", "bc", "bd", "be", "bf", "bg", "bh", "bi", "bj",
        "bk", "bl", "bm", "bn", "bo", "bp", "bq", "br", "bs", "bt",
        "bu", "bv", "bw", "bx", "by", "bz"
    };

    public static string ToFormattedString(this double value)
    {
        if (value < 1000) return ((int)value).ToString();

        int suffixIndex = 0;

        double displayValue = value;
        while (displayValue >= 1000 && suffixIndex < _suffixes.Length - 1)
        {
            displayValue /= 1000;
            suffixIndex++;
        }

        return $"{(int)displayValue}{_suffixes[suffixIndex]}";
    }

    public static string ToFormattedString(this int value)
    {
        return ToFormattedString((double)value);
    }
}
