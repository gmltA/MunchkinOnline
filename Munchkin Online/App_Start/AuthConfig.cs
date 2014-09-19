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
                          "4550448", "a5oDRnliLHMVK1GGDs5I"),
                   displayName: "ВКонтакте",
                   extraData: null);
        }
    }
}