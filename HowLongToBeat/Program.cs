using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using RandomUserAgent;
using System.Net.Http.Headers;
using HtmlAgilityPack;
using AngleSharp;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async () => {
	var ws = new WebScraper(); 
	var result = await ws.Test(); 
	return result;
});

app.Run();


public class WebScraper
{
	public async Task<string> Test()
	{

		HttpClient client = new HttpClient();
		client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
		var userAgent = RandomUa.RandomUserAgent;
		client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);


		var values = new Dictionary<string, string>
		{
			{"queryString", "Pathfinder"},
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

		var htmlDoc = new HtmlDocument();
		htmlDoc.LoadHtml(responseString);

		var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='search_list_details']");
		foreach (var node in htmlNodes)
		{
			var txt = node.InnerText;
			if (txt.Length > 0)
			{
				Console.WriteLine(node.InnerText);
			}
		}

		return await Task.FromResult("All good");
	}
}

public class GameDetail
{
    public string Title { get; set; }
    public string Main { get; set; }
    public string MainAndExtras { get; set; }
    public string Completionist { get; set; }

    public GameDetail(string title, string main, string mainandextras, string completionist)
    {
		Title = title;
		Main = main;
		MainAndExtras = mainandextras;
		Completionist = completionist;
    }
}

public class HtmlParser
{
	public async Task<List<GameDetail>> GetGameDetailsAsync(string html)
    {
		var config = Configuration.Default.WithJs();
		var context = BrowsingContext.New(config);
		var document = await context.OpenAsync(req => req.Content(html));

		var gameTitlesNodes = document.All.Where(x => x.LocalName == "a" && x.ClassList.Contains("text_white"));
		var gameTitles = new string[gameTitlesNodes.Count()];

		var i = -1;
		foreach (var x in gameTitlesNodes)
        {
			i++;
			var contentWithoutTabsOrNewLine = Regex.Replace(x.TextContent, @"\t|\n", " ").Trim();
			var title = Regex.Replace(contentWithoutTabsOrNewLine, @"\s+", " ");
			gameTitles[i] = title;
        }

		var gameDetailsNodes = document.All.Where(x => x.LocalName == "div" && x.ClassList.Contains("search_list_details_block"));
		var gameDetails = new string[gameDetailsNodes.Count()];

		int j = -1;
		foreach (var x in gameDetailsNodes)
		{
			j++;
			var contentWithouTabs = Regex.Replace(x.TextContent, @"\t", " ").Trim();
			var content = Regex.Replace(contentWithouTabs, @"\n", ",");
			gameDetails[j] = content.Trim();
		}

		var result = new List<GameDetail>();
		for (var k = 0; k < gameTitles.Length; k++)		
        {
			var details = GetGameData(gameDetails[k]);
			var gd = new GameDetail(
				title: gameTitles[k],
				main: details["main"],
				mainandextras: details["mainAndExtras"],
				completionist: details["completionist"]
			);
			result.Add(gd);
        }

		return await Task.FromResult(result);
    }

	public Dictionary<string, string> GetGameData(string text)
    {
		var result = new Dictionary<string, string>();
        var collection = text.Split(",");

		result.Add("main", collection[1].Trim());
		result.Add("mainAndExtras", collection[3].Trim());
		result.Add("completionist", collection[5].Trim());

		return result;
    }
}

