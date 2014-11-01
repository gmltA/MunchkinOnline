using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Munchkin_Online.Core
{
    public static class MotoImporter
    {
        static Uri NodePath;

        static MotoImporter()
        {
            UriBuilder ub = new UriBuilder();
            ub.Scheme = "http";
            ub.Host = "localhost";
            ub.Port = 8888;
            NodePath = ub.Uri;
        }

        public static string GetMoto()
        {
            try
            {
                return new StreamReader(((HttpWebRequest)WebRequest.Create(NodePath)).GetResponse().GetResponseStream()).ReadToEnd();
            }
            catch
            {
                return "Убивай гусей,<br/>подставляй друзей!";
            }
        }
    }
}