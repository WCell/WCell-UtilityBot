using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using ServiceStack.Text;

namespace WCellUtilityBot
{
    public class FisheyeWebHookData
    {
        public Repository Repository { get; set; }
        public Changeset Changeset { get; set; }
    }
    public class Repository
    {
        public string Name { get; set; }
    }
    public class Changeset
    {
        public string Csid { get; set; }
        public string Displayid { get; set; }
        public string Author { get; set; }
        public string Comment { get; set; }
        public string Date { get; set; }
        public string[] Branches { get; set; }
        public string[] Tags { get; set; }
        public string[] Parents { get; set; }
    }

    static class CommitListener
    {
        public static void StartListener()
        {
            var listener = new HttpListener();
            listener.Start();
            listener.Prefixes.Add(Properties.Settings.Default.CommitListenerAddress);
            while (true)
            {
                var context = listener.GetContext();
                var obj = JsonSerializer.DeserializeFromStream<FisheyeWebHookData>(context.Request.InputStream);
                context.Response.StatusCode = (int) HttpStatusCode.OK;
                context.Response.Close();
                IrcConnection.Irc.CommandHandler.Msg(Properties.Settings.Default.CommitNotificationChannel, string.Format("Commit-> Project: {0} Author: {1} Branch: {2} Commit Note: {3}", obj.Repository.Name, StripEmailFromAuthor(obj.Changeset.Author), obj.Changeset.Branches[0], obj.Changeset.Comment));
            }
        }

        private static string StripEmailFromAuthor(string author)
        {
            try
            {
                if (author.Contains("@"))
                {
                    var start = author.IndexOf('<');
                    var end = author.Length;
                    return author.Remove(start, end);
                }

            }
            catch (Exception)
            {
                return author;
            }
            return author;
        }
    }
}
