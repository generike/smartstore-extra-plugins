using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SmartStore.TwitterAuth.Core
{
    // http://www.burritostand.com/log-in-to-twitter-with-oauth-and-c-sharp-and-get-twitter-user-email
    internal class TwitterUrlHelper
    {
        private readonly string _endpointUrl = string.Empty;

        public TwitterUrlHelper(string endpointUrl)
        {
            _endpointUrl = endpointUrl;
        }

        private string UrlEncode(string url)
        {
            var toBeEncoded = new Dictionary<string, string>() { { "%", "%25" }, { "!", "%21" }, { "#", "%23" }, { " ", "%20" },
                { "$", "%24" }, { "&", "%26" }, { "'", "%27" }, { "(", "%28" }, { ")", "%29" }, { "*", "%2A" }, { "+", "%2B" }, { ",", "%2C" },
                { "/", "%2F" }, { ":", "%3A" }, { ";", "%3B" }, { "=", "%3D" }, { "?", "%3F" }, { "@", "%40" }, { "[", "%5B" }, { "]", "%5D" } };
            var replaceRegex = new Regex(@"[%!# $&'()*+,/:;=?@\[\]]");
            MatchEvaluator matchEval = match => toBeEncoded[match.Value];
            var encoded = replaceRegex.Replace(url, matchEval);
            return encoded;
        }

        private string CreateHMacHash(string key, string message)
        {
            var hasher = new HMACSHA1(Encoding.UTF8.GetBytes(key));
            var data = hasher.ComputeHash(Encoding.UTF8.GetBytes(message));
            return Convert.ToBase64String(data);
        }

        public string CreateSignature(string consumerSecret, List<KeyValuePair<string, string>> parameters, string tokenSecret = null)
        {
            var signatureInner = string.Empty;
            var signature = "GET&";
            signature += UrlEncode(_endpointUrl) + "&";

            foreach (var item in parameters)
            {
                if (item.Key == "oauth_callback")
                {
                    // The callback url needs to be encoded twice, so do it first now.
                    signatureInner += item.Key + "=" + UrlEncode(item.Value) + "&";
                }
                else
                {
                    signatureInner += item.Key + "=" + item.Value + "&";
                }
            }

            signatureInner = signatureInner.Substring(0, signatureInner.Length - 1);
            signatureInner = UrlEncode(signatureInner);
            signature += signatureInner;

            // Encode the whole thing and include the token secret as part of the key if it's passed in.
            return UrlEncode(CreateHMacHash(consumerSecret + "&" + (tokenSecret.HasValue() ? tokenSecret : ""), signature));
        }

        public string CreateCallingUrls(List<KeyValuePair<string, string>> parameters)
        {
            var url = _endpointUrl + "?";

            foreach (var item in parameters)
            {
                if (item.Key == "oauth_callback")
                {
                    url += item.Key + "=" + UrlEncode(item.Value) + "&";
                }
                else
                {
                    url += item.Key + "=" + item.Value + "&";
                }
            }

            url = url.Substring(0, url.Length - 1);
            return url;
        }
    }
}