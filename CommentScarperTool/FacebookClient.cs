using System;
using System.Collections.Generic;

namespace CommentScarperTool
{
    internal class FacebookClient
    {
        public FacebookClient(string accessToken)
        {
        }

        public string Version { get; internal set; }

        internal dynamic Get(string v, Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }
    }
}