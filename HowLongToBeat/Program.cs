using HowLongToBeat;

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

