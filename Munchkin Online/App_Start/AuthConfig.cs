using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using Munchkin_Online.Core.Auth;

namespace Munchkin_Online
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            OAuthWebSecurity.RegisterClient(
                   client: new VKAuthClient(
                          "4551784", "eBso2kOrsWVZuYqQ8AGW"),
                   displayName: "ВКонтакте",
                   extraData: null);
        }
    }
}