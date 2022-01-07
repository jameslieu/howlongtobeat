using RandomUserAgent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HowLongToBeat
{
    public class HLTBWebScraper
    {
        public async Task<List<Game>> Search(string query)
        {
            string html = await GetGameHTMLResultsAsync(query);

            var hp = new HLTBHtmlParser();
            var result = await hp.GetGameDetailsAsync(html);

            return result;
        }

        private static async Task<string> GetGameHTMLResultsAsync(string query)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
            var userAgent = RandomUa.RandomUserAgent;
            client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

            var values = new Dictionary<string, string>
        {
            {"queryString", query},
            {"t", "games"},
            {"sorthead", "popular"},
            {"sortd", "Normal Order"},
            {"plat", ""},
            {"length_type", " main"},
            {"length_min", ""},
            {"length_max", ""},
            {"detail", " 0"},
        };

            string url = "https://howlongtobeat.com/search_results?page=1";
            var data = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(url, data);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
    }
}