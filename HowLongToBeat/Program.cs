using RandomUserAgent;
using AngleSharp;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async () => {
	var pathfinder = new Game(title: "Pathfinder: Kingmaker", imgURL: "https://example.com", main: "74½ Hours", mainandextras: "126 Hours", completionist: "188 Hours");
	var wor = new Game(title: "Pathfinder: Wrath of the Righteous", imgURL: "https://example.com", main: "57½ Hours", mainandextras: "125 Hours", completionist: "200 Hours");

	return new List<Game> { pathfinder, wor };
});

app.MapGet("/search/{query}", async (string query) => {
	var ws = new HLTBWebScraper();
	var result = await ws.Search(query);
	return result;
});

app.Run();


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

public class Game
{
    public string Title { get; }
    public string ImgURL { get; }
    public string Main { get; }
    public string MainAndExtras { get; }
    public string Completionist { get; }

    public Game(string title, string main, string mainandextras, string completionist, string imgURL)
    {
		Title = title;
        ImgURL = imgURL;
		Main = main;
		MainAndExtras = mainandextras;
		Completionist = completionist;
    }
}

public class HLTBHtmlParser
{
	public async Task<List<Game>> GetGameDetailsAsync(string html)
    {
        var config = Configuration.Default;
        var context = BrowsingContext.New(config);
        var document = await context.OpenAsync(req => req.Content(html));

        var gameTitles = GetGameTitles(document);
        var gameImgs = GetGameImgs(document);
        var gameDetails = GetGameDetails(document);

        var result = new List<Game>();
        for (var k = 0; k < gameTitles.Length; k++)
        {
            var details = MapToDictionary(gameDetails[k]);
            var gd = new Game(
                title: gameTitles[k],
                imgURL: gameImgs[k],
                main: details["main"],
                mainandextras: details["mainAndExtras"],
                completionist: details["completionist"]
            );
            result.Add(gd);
        }

        return await Task.FromResult(result);
    }

    private string[] GetGameImgs(AngleSharp.Dom.IDocument document)
    {
        var gameImgNodes = document.All.Where(x => x.LocalName == "img");
        var gameImgs = new string[gameImgNodes.Count()];
        var i = -1;
        foreach (var x in gameImgNodes)
        {
            i++;
            var imgNode = x.Attributes.First(x => x.LocalName == "src");
            var imgURI = $"https://howlongtobeat.com{imgNode.Value}";
            gameImgs[i] = imgURI;
        }

        return gameImgs;
    }

    private string[] GetGameTitles(AngleSharp.Dom.IDocument document)
    {
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

        return gameTitles;
    }

    private string[] GetGameDetails(AngleSharp.Dom.IDocument document)
    {
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

        return gameDetails;
    }

    private Dictionary<string, string> MapToDictionary(string text)
    {
		var result = new Dictionary<string, string>();
        var collection = text.Split(",");

		result.Add("main", collection[1].Trim());
		result.Add("mainAndExtras", collection[3].Trim());
		result.Add("completionist", collection[5].Trim());

		return result;
    }
}

