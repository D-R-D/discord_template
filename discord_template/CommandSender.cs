using System.Text;

namespace discord_template
{
    internal class CommandSender
    {
        public string[]? command_list { get; private set; }
        public IDs? ids { get; private set; }

        public void setJsonCommands(string directory_path)
        {
            if(directory_path == null) { throw new ArgumentNullException("directory_path"); }
            //ファイル一覧を取得
            if (!Directory.Exists(directory_path)) { throw new Exception("指定されたパス " + directory_path + " は存在しません。"); }
            command_list = Directory.GetFiles(directory_path, "*.json");
            if (command_list.Length <= 0) { throw new Exception("指定されたパス内にjsonファイルが存在しませんでした。"); }
        }

        public void setIDs(IDs id)
        {
            if (id == null) { throw new ArgumentNullException("id"); }
            ids = id;
        }

        public void requestSender()
        {
            if(command_list == null) { throw new NullReferenceException(nameof(command_list)); }

            HttpRequestMessage[] requests = getHeader();

            foreach (HttpRequestMessage request in requests)
            {
                foreach (string json_command in command_list)
                {

                }
            }
        }
        private HttpRequestMessage[] getHeader()
        {
            if (ids == null) { throw new NullReferenceException("ids"); }
            if (ids.guild_ids == null) { throw new NullReferenceException("ids.guild_ids"); }
            HttpRequestMessage[] request = new HttpRequestMessage[ids.guild_ids.Length];
            int idcount = 0;
            foreach (string guild_id in ids.guild_ids)
            {
                if (Tools.checkStringISNullorEmpty(guild_id)) { throw new Exception("guild_idが不正です。\nguild_idがnull、もしくは空白です。"); }
                if (Tools.checkStringISNullorEmpty(ids.application_id)) { throw new Exception("application_idが不正です。\napplication_idがnull、もしくは空白です。"); }
                if (Tools.checkStringISNullorEmpty(ids.token)) { throw new Exception("Tokenが不正です。\nTokenがnull、もしくは空白です。"); }

                string url = "https://discord.com/api/v8/applications/" + ids.application_id + "/guilds/" + guild_id + "/commands";
                UriBuilder builder = new UriBuilder(new Uri(url));
                request[idcount] = new HttpRequestMessage(HttpMethod.Post, builder.Uri);
                request[idcount].Headers.Add("Authorization", "Bot " + ids.token);

                idcount++;
            }

            return request;
        }
        private HttpRequestMessage requestContentBuilder(HttpRequestMessage requestMessage, string json_command)
        {        
            //渡されたjson形式のコマンド情報をコンテンツに設定する
            if (Tools.checkStringISNullorEmpty(json_command)) { throw new Exception("json_commandが不正です。\njson_commandがnullもしくは空白です。\n"); }
            requestMessage.Content = new StringContent(json_command, Encoding.UTF8, "application/json");

            return requestMessage;
        }
    }
}
