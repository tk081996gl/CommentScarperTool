using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommentScarperTool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var client = new WebClient();

            string oauthUrl = string.Format("https://graph.facebook.com/oauth/access_token?type=client_cred&client_id={0}&client_secret={1}", "appid", "appsecret");

            //string accessToken = client.DownloadString(oauthUrl).Split('=')[1];

           // string pageInfo = client.DownloadString(string.Format("https://graph.facebook.com/wikipedia?access_token={0} ", accessToken));
           // string pagePosts = client.DownloadString(string.Format("https://graph.facebook.com/wikipedia/posts?access_token={0} ", accessToken));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CST());
        }

    }
}
