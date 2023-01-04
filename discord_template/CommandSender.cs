using System.Text;

namespace discord_template
{
    internal class CommandSender
    {
        public string[]? CommandList { get; private set; }
        public Ids? Ids { get; private set; }

        public void setJsonCommands(string directory_path)
        {
            if (directory_path == null) { throw new Exception($"指定されたパス {directory_path} は存在しません。"); }
            //ファイル一覧を取得
            if (!Directory.Exists(directory_path)) { throw new Exception($"指定されたパス {directory_path} は存在しません。"); }
            CommandList = Directory.GetFiles(directory_path, "*.json");
            if (CommandList.Length <= 0) { throw new Exception("指定されたパス内にjsonファイルが存在しませんでした。"); }
        }

        public void setIDs(Ids id)
        {
            if (id == null) { throw new ArgumentNullException("id"); }
            Ids = id;
        }

        public void requestSender()
        {
            if (CommandList == null) { throw new NullReferenceException(nameof(CommandList)); }

            HttpRequestMessage[] requests = getHeader();

            foreach (HttpRequestMessage request in requests)
            {
                foreach (string json_command in CommandList)
                {
                    //コンテンツを設定
                    if (Tools.IsNullOrEmpty(json_command)) { throw new Exception("json_commandが不正です。\njson_commandがnullもしくは空白です。"); }
                    HttpRequestMessage sendRequest = requestContentBuilder(request,json_command);

                    //送信する
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = client.Send(sendRequest);
                }
            }
        }
        private HttpRequestMessage[] getHeader()
        {
            if (Ids == null) { throw new NullReferenceException("ids"); }
            if (Ids.m_GuildIds == null) { throw new NullReferenceException("ids.guild_ids"); }
            HttpRequestMessage[] request = new HttpRequestMessage[Ids.m_GuildIds.Length];
            int idcount = 0;
            foreach (string guild_id in Ids.m_GuildIds) {
                if (guild_id.IsNullOrEmpty()) { throw new Exception("guild_idが不正です。\nguild_idがnull、もしくは空白です。"); }
                if (Ids.m_ApplicationId.IsNullOrEmpty()) { throw new Exception("application_idが不正です。\napplication_idがnull、もしくは空白です。"); }
                if (Ids.m_Token.IsNullOrEmpty()) { throw new Exception("Tokenが不正です。\nTokenがnull、もしくは空白です。"); }

                string url = "https://discord.com/api/v8/applications/" + Ids.m_ApplicationId + "/guilds/" + guild_id + "/commands";
                UriBuilder builder = new UriBuilder(new Uri(url));
                request[idcount] = new HttpRequestMessage(HttpMethod.Post, builder.Uri);
                request[idcount].Headers.Add("Authorization", "Bot " + Ids.m_Token);

                idcount++;
            }

            return request;
        }
        private HttpRequestMessage requestContentBuilder(HttpRequestMessage requestMessage, string json_command)
        {
            //渡されたjson形式のコマンド情報をコンテンツに設定する
            if (json_command.IsNullOrEmpty()) { throw new Exception("json_commandが不正です。\njson_commandがnullもしくは空白です。\n"); }
            requestMessage.Content = new StringContent(json_command, Encoding.UTF8, "application/json");

            return requestMessage;
        }
    }
}
