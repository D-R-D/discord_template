using System.Configuration;

namespace discord_template
{
    public class IDs
    {
        public string? token { get; private set; }
        public string[]? guild_ids { get; private set; }
        public string? application_id { get; private set; }
        public string[]? admin_ids { get; private set; }

        public void setIDValues(AppSettingsReader reader)
        {
            //config内のtokenを取得する
            string? token_string = reader.GetValue("token", typeof(string)).ToString();
            if (Tools.checkStringISNullorEmpty(token_string)) { throw new Exception("tokenが不正です。\ntokenがnullもしくは空白です。"); }
            token = token_string;

            //config内の","で区切られたguild_idを取得する
            string? guild_id_string = reader.GetValue("guild_id", typeof(string)).ToString();
            if (Tools.checkStringISNullorEmpty(guild_id_string)) { throw new Exception("guild_idが不正です。\nguild_idがnullもしくは空白です。"); }
            guild_ids = guild_id_string!.Split(',');

            //config内のapplication_idを取得する
            string? application_id_string = reader.GetValue("application_id", typeof(string)).ToString();
            if (Tools.checkStringISNullorEmpty(application_id_string)) { throw new Exception("application_idが不正です。\napplication_idがnullもしくは空白です。"); }
            application_id = application_id_string;

            string? admin_id_string = reader.GetValue("admin_id", typeof(string)).ToString();
            if (Tools.checkStringISNullorEmpty(application_id_string)) { throw new Exception("application_idが不正です。\napplication_idがnullもしくは空白です。"); }
            admin_ids = application_id_string!.Split(',');
        }
    }
}
