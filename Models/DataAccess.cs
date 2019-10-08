using System.Data.SQLite;
using System;
using System.Net;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

namespace WordPredictor
{
    public class SQLiteDb
    {
        string _path;
        public SQLiteDb(string path)
        {
            _path = path;
        }

        public static List<string> GetDictionaryMatches(string snippet)
        {
            List<string> Matches = new List<string>();

            using (SQLiteConnection connect = new SQLiteConnection(@"Data Source=C:\Users\chri0a77\SQLite\Dictionary.db"))
            {
                connect.Open();
                using (SQLiteCommand cmd = connect.CreateCommand())
                {
                    cmd.CommandText = @"SELECT * FROM Words WHERE CHARINDEX('" + snippet + "', Value) = 1";
                    cmd.CommandType = CommandType.Text;
                    SQLiteDataReader r = cmd.ExecuteReader();
                    while (r.Read())
                    {
                        Matches.Add(Convert.ToString(r["Value"]));
                    }
                }
            }
            return Matches;
        }
    }

    public class WordPrediction
    {
        public static List<string> GetWordPredictMatches(string snippet)
        {
            string token = File.ReadAllText("Models/Token.txt").ToString();

            List<string> Matches = new List<string>();

            var uri = "https://services.lingapps.dk/misc/getPredictions" + "?locale=da-DK&text=" + HttpUtility.UrlEncode(snippet);

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";

            httpWebRequest.Headers.Add("Authorization", "Bearer " + token);

            using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {

                while (!reader.EndOfStream)
                {
                    var trimmed = reader.ReadLine()
                        .Trim()
                        .Replace("\"", "")
                        .Replace("[", "")
                        .Replace("]", "")
                        .Replace(",", "");

                    if (trimmed == "")
                        continue;

                    Matches.Add(Regex.Unescape(trimmed));
                }
                return Matches;
            }
        }
    }

}
