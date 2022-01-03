using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using RandomUserAgent;
using System.Net.Http.Headers;
using HtmlAgilityPack;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async () => {
	var ws = new WebScraper(); 
	var result = await ws.Test(); 
	return result;
});

app.Run();

public class Row
{
	public string Title { get; set; }
}

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

/* 
 * 

https://dotnetfiddle.net/ozk9kE
 
 // @nuget: HtmlAgilityPack

using System;
using System.Xml;
using System.Linq;
using HtmlAgilityPack;
					
public class Program
{
	public static void Main()
	{
		var html = 
		@"<div class=""global_padding shadow_box back_blue center"">
	<h3> We Found 8 Games for ""Pathfinder"" </h3>
</div>

<ul>
	<div class=""clear""></div>
	<li class=""back_darkish""
		style=""background-image:linear-gradient(rgb(31, 31, 31), rgba(31, 31, 31, 0.9)), url('/games/60050_Pathfinder_Kingmaker.jpg')"">
		<div class=""search_list_image"">
			<a aria-label=""Pathfinder Kingmaker"" title=""Pathfinder Kingmaker"" href=""game?id=60050"">
				<img alt=""Box Art"" src=""/games/60050_Pathfinder_Kingmaker.jpg"" />
			</a>
		</div>
		<div class=""search_list_details"">
			<h3 class=""shadow_text"">
				<a class=""text_white"" title=""Pathfinder Kingmaker"" href=""game?id=60050"">Pathfinder: Kingmaker</a>
			</h3>
			<div class=""search_list_details_block"">
				<div>
					<div class=""search_list_tidbit text_white shadow_text"">Main Story</div>
					<div class=""search_list_tidbit center time_70"">74&#189; Hours </div>
					<div class=""search_list_tidbit text_white shadow_text"">Main + Extra</div>
					<div class=""search_list_tidbit center time_100"">126 Hours </div>
					<div class=""search_list_tidbit text_white shadow_text"">Completionist</div>
					<div class=""search_list_tidbit center time_100"">188 Hours </div>
				</div>
			</div>
		</div>
	</li>
	<li class=""back_darkish""
		style=""background-image:linear-gradient(rgb(31, 31, 31), rgba(31, 31, 31, 0.9)), url('/games/83856_Pathfinder_Wrath_of_the_Righteous.jpg')"">
		<div class=""search_list_image"">
			<a aria-label=""Pathfinder Wrath of the Righteous"" title=""Pathfinder Wrath of the Righteous""
				href=""game?id=83856"">
				<img alt=""Box Art"" src=""/games/83856_Pathfinder_Wrath_of_the_Righteous.jpg"" />
			</a>
		</div>
		<div class=""search_list_details"">
			<h3 class=""shadow_text"">
				<a class=""text_white"" title=""Pathfinder Wrath of the Righteous"" href=""game?id=83856"">Pathfinder: Wrath
					of the Righteous</a>
			</h3>
			<div class=""search_list_details_block"">
				<div>
					<div class=""search_list_tidbit text_white shadow_text"">Main Story</div>
					<div class=""search_list_tidbit center time_50"">57&#189; Hours </div>
					<div class=""search_list_tidbit text_white shadow_text"">Main + Extra</div>
					<div class=""search_list_tidbit center time_100"">125 Hours </div>
					<div class=""search_list_tidbit text_white shadow_text"">Completionist</div>
					<div class=""search_list_tidbit center time_40"">200 Hours </div>
				</div>
			</div>
		</div>
	</li>
	<div class=""clear""></div>
	<li class=""back_darkish""
		style=""background-image:linear-gradient(rgb(31, 31, 31), rgba(31, 31, 31, 0.9)), url('/games/42106_Pathfinder_Adventures.jpg')"">
		<div class=""search_list_image"">
			<a aria-label=""Pathfinder Adventures"" title=""Pathfinder Adventures"" href=""game?id=42106"">
				<img alt=""Box Art"" src=""/games/42106_Pathfinder_Adventures.jpg"" />
			</a>
		</div>
		<div class=""search_list_details"">
			<h3 class=""shadow_text"">
				<a class=""text_white"" title=""Pathfinder Adventures"" href=""game?id=42106"">Pathfinder Adventures</a>
			</h3>
			<div class=""search_list_details_block"">
				<div>
					<div class=""search_list_tidbit text_white shadow_text"">Main Story</div>
					<div class=""search_list_tidbit center time_40"">30 Hours </div>
					<div class=""search_list_tidbit text_white shadow_text"">Main + Extra</div>
					<div class=""search_list_tidbit center time_00"">--</div>
					<div class=""search_list_tidbit text_white shadow_text"">Completionist</div>
					<div class=""search_list_tidbit center time_40"">37 Hours </div>
				</div>
			</div>
		</div>
	</li>
	<li class=""back_darkish""
		style=""background-image:linear-gradient(rgb(70, 70, 70), rgba(70, 70, 70, 0.9)), url('/games/66107_Pathfinder_Kingmaker_-_Varnholds_Lot.jpg')"">
		<div class=""search_list_image"">
			<a aria-label=""Pathfinder Kingmaker  Varnholds Lot"" title=""Pathfinder Kingmaker  Varnholds Lot""
				href=""game?id=66107"">
				<img alt=""Box Art"" src=""/games/66107_Pathfinder_Kingmaker_-_Varnholds_Lot.jpg"" />
			</a>
		</div>
		<div class=""search_list_details"">
			<h3 class=""shadow_text"">
				<a class=""text_white"" title=""Pathfinder Kingmaker  Varnholds Lot"" href=""game?id=66107"">Pathfinder:
					Kingmaker - Varnhold's Lot</a>
			</h3>
			<div class=""search_list_details_block"">
				<div>
					<div class=""search_list_tidbit text_white shadow_text"">Main Story</div>
					<div class=""search_list_tidbit center time_40"">5 Hours </div>
					<div class=""search_list_tidbit text_white shadow_text"">Main + Extra</div>
					<div class=""search_list_tidbit center time_40"">10&#189; Hours </div>
					<div class=""search_list_tidbit text_white shadow_text"">Completionist</div>
					<div class=""search_list_tidbit center time_40"">24 Hours </div>
				</div>
			</div>
		</div>
	</li>
	<div class=""clear""></div>
	<li class=""back_darkish""
		style=""background-image:linear-gradient(rgb(31, 31, 31), rgba(31, 31, 31, 0.9)), url('/games/54250_Azure_Saga_Pathfinder.jpg')"">
		<div class=""search_list_image"">
			<a aria-label=""Azure Saga Pathfinder"" title=""Azure Saga Pathfinder"" href=""game?id=54250"">
				<img alt=""Box Art"" src=""/games/54250_Azure_Saga_Pathfinder.jpg"" />
			</a>
		</div>
		<div class=""search_list_details"">
			<h3 class=""shadow_text"">
				<a class=""text_white"" title=""Azure Saga Pathfinder"" href=""game?id=54250"">Azure Saga: Pathfinder</a>
			</h3>
			<div class=""search_list_details_block"">
				<div>
					<div class=""search_list_tidbit text_white shadow_text"">Main Story</div>
					<div class=""search_list_tidbit center time_40"">22 Hours </div>
					<div class=""search_list_tidbit text_white shadow_text"">Main + Extra</div>
					<div class=""search_list_tidbit center time_40"">26 Hours </div>
					<div class=""search_list_tidbit text_white shadow_text"">Completionist</div>
					<div class=""search_list_tidbit center time_00"">--</div>
				</div>
			</div>
		</div>
	</li>
	<li class=""back_darkish""
		style=""background-image:linear-gradient(rgb(70, 70, 70), rgba(70, 70, 70, 0.9)), url('/games/54864_Pathfinder_Adventures_-_Rise_of_the_Goblins.jpg')"">
		<div class=""search_list_image"">
			<a aria-label=""Pathfinder Adventures  Rise of the Goblins""
				title=""Pathfinder Adventures  Rise of the Goblins"" href=""game?id=54864"">
				<img alt=""Box Art"" src=""/games/54864_Pathfinder_Adventures_-_Rise_of_the_Goblins.jpg"" />
			</a>
		</div>
		<div class=""search_list_details"">
			<h3 class=""shadow_text"">
				<a class=""text_white"" title=""Pathfinder Adventures  Rise of the Goblins"" href=""game?id=54864"">Pathfinder
					Adventures - Rise of the Goblins</a>
			</h3>
			<div class=""search_list_details_block"">
				<div>
					<div class=""search_list_tidbit text_white shadow_text"">Main Story</div>
					<div class=""search_list_tidbit center time_40"">4 Hours </div>
					<div class=""search_list_tidbit text_white shadow_text"">Main + Extra</div>
					<div class=""search_list_tidbit center time_40"">6 Hours </div>
					<div class=""search_list_tidbit text_white shadow_text"">Completionist</div>
					<div class=""search_list_tidbit center time_00"">--</div>
				</div>
			</div>
		</div>
	</li>
	<div class=""clear""></div>
	<li class=""back_darkish""
		style=""background-image:linear-gradient(rgb(31, 31, 31), rgba(31, 31, 31, 0.9)), url('/games/92455_Pathfinders_Memories.jpg')"">
		<div class=""search_list_image"">
			<a aria-label=""Pathfinders Memories"" title=""Pathfinders Memories"" href=""game?id=92455"">
				<img alt=""Box Art"" src=""/games/92455_Pathfinders_Memories.jpg"" />
			</a>
		</div>
		<div class=""search_list_details"">
			<h3 class=""shadow_text"">
				<a class=""text_white"" title=""Pathfinders Memories"" href=""game?id=92455"">Pathfinders: Memories</a>
			</h3>
			<div class=""search_list_details_block"">
				<div>
					<div class=""search_list_tidbit text_white shadow_text"">Main Story</div>
					<div class=""search_list_tidbit center time_00"">--</div>
					<div class=""search_list_tidbit text_white shadow_text"">Main + Extra</div>
					<div class=""search_list_tidbit center time_00"">--</div>
					<div class=""search_list_tidbit text_white shadow_text"">Completionist</div>
					<div class=""search_list_tidbit center time_00"">--</div>
				</div>
			</div>
		</div>
	</li>
	<li class=""back_darkish""
		style=""background-image:linear-gradient(rgb(70, 70, 70), rgba(70, 70, 70, 0.9)), url('/games/82064_Pathfinder_Adventures_-_Rise_of_the_Goblins_Deck_2.jpg')"">
		<div class=""search_list_image"">
			<a aria-label=""Pathfinder Adventures  Rise of the Goblins Deck 2""
				title=""Pathfinder Adventures  Rise of the Goblins Deck 2"" href=""game?id=82064"">
				<img alt=""Box Art"" src=""/games/82064_Pathfinder_Adventures_-_Rise_of_the_Goblins_Deck_2.jpg"" />
			</a>
		</div>
		<div class=""search_list_details"">
			<h3 class=""shadow_text"">
				<a class=""text_white"" title=""Pathfinder Adventures  Rise of the Goblins Deck 2""
					href=""game?id=82064"">Pathfinder Adventures - Rise of the Goblins Deck 2</a>
			</h3>
			<div class=""search_list_details_block"">
				<div>
					<div class=""search_list_tidbit text_white shadow_text"">Main Story</div>
					<div class=""search_list_tidbit center time_00"">--</div>
					<div class=""search_list_tidbit text_white shadow_text"">Main + Extra</div>
					<div class=""search_list_tidbit center time_00"">--</div>
					<div class=""search_list_tidbit text_white shadow_text"">Completionist</div>
					<div class=""search_list_tidbit center time_00"">--</div>
				</div>
			</div>
		</div>
	</li>
	<div class=""clear""></div>
</ul>
";

		var htmlDoc = new HtmlDocument();
		htmlDoc.LoadHtml(html);

		var htmlNodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='search_list_tidbit']");
		if (htmlNodes.Count() > 0)
		{	
			foreach (var node in htmlNodes)
			{
				var txt = node.InnerText;
				if (txt.Length > 0)
				{
					Console.WriteLine(txt);
				}
			}
		}
	}
}
 
 */
