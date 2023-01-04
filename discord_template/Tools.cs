namespace discord_template
{
    public class Tools
    {
        public static bool checkStringISNullorEmpty(string? string_item)
        {
            if (string_item == null) { return true; }
            if (string_item == "") { return true; }
            return false;
        }
    }
}
