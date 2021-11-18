using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebVella.Pulsar.Utils
{
    public static class NavigatorExt
    {
        public static void ApplyChangeToUrlQuery(NavigationManager navigator, Dictionary<string, string> replaceDict, bool forceLoad = false)
        {
            var currentUrl = navigator.Uri;
            var uri = new System.Uri(currentUrl);
            var queryDictionary = System.Web.HttpUtility.ParseQueryString(uri.Query);

            var newQueryDictionary = new Dictionary<string, string>();
            foreach (string key in queryDictionary.Keys)
            {
                if (!replaceDict.Keys.Contains(key))
                {
                    var queryValue = queryDictionary[key];
                    if (!string.IsNullOrWhiteSpace(queryValue))
                        newQueryDictionary[key] = queryValue;
                }
            }

            foreach (string key in replaceDict.Keys)
            {
                var queryValue = replaceDict[key];
                if (!string.IsNullOrWhiteSpace(queryValue))
                    newQueryDictionary[key] = ProcessQueryValueForUrl(queryValue);
            }

            var queryStringList = new List<string>();
            foreach (var key in newQueryDictionary.Keys)
            {
                queryStringList.Add($"{key}={newQueryDictionary[key]}");
            }
            var urlQueryString = "";
            if (queryStringList.Count > 0)
                urlQueryString = "?" + string.Join("&", queryStringList);

            navigator.NavigateTo(uri.LocalPath + urlQueryString, forceLoad);

        }

        public static string GetQueryValue(NavigationManager navigator, string paramName)
        {
            var result = "";

            var urlAbsolute = navigator.ToAbsoluteUri(navigator.Uri);

            if (QueryHelpers.ParseQuery(urlAbsolute.Query).TryGetValue(paramName, out var paramValue))
            {
                result = UrlUndoReplaceSpecialCharacters(HttpUtility.UrlDecode(paramValue));
            }

            return result;
        }

        public static string ProcessQueryValueForUrl(string queryValue)
        {
            if (String.IsNullOrWhiteSpace(queryValue))
                return null;

            return HttpUtility.UrlEncode(UrlReplaceSpecialCharacters(queryValue));

        }

        public static string UrlReplaceSpecialCharacters(string inputValue)
        {
            return inputValue.Replace("/", "~");
        }
        public static string UrlUndoReplaceSpecialCharacters(string inputValue)
        {
            return inputValue.Replace("~", "/");
        }
    }
}
