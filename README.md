# HowLongToBeat Wrapper

## About & Credits

[How long to beat](https://howlongtobeat.com/) provides information and data about games and how long it will take to finish them. It's a great website which relies heavily from community data.

This package is a simple wrapper api to fetch data from [How long to beat](https://howlongtobeat.com/) (search and detail). Please use this responsibly.

## Usage

### Install
Install the library using nuget package manager (search for HowLongToBeat) or using Package Manager Console

```bash
Install-Package HowLongToBeat
```

Install the library using .NET CLI
```bash
dotnet add package HowLongToBeat
```

#### Sample Code

- Import namespace
```csharp
using HowLongToBeat;
```


- Instantiate the `HLTBWebScraper` object and use the `Search()` method
```csharp
var query = "Halo";
var ws = new HLTBWebScraper();
var result = await ws.Search(query);
```

#### Example Result
```
[
  {
    "title": "Halo Infinite",
    "imgURL": "https://howlongtobeat.com/games/57454_Halo_Infinite.jpg",
    "main": "10½ Hours",
    "mainAndExtras": "17½ Hours",
    "completionist": "25 Hours"
  },
  {
    "title": "Halo: The Master Chief Collection",
    "imgURL": "https://howlongtobeat.com/games/Halo_Collection.jpg",
    "main": "",
    "mainAndExtras": "",
    "completionist": ""
  },
  {
    "title": "Halo 3",
    "imgURL": "https://howlongtobeat.com/games/256px-Halo_3_final_boxshot.JPG",
    "main": "8 Hours",
    "mainAndExtras": "11½ Hours",
    "completionist": "18½ Hours"
  },
  {
    "title": "Halo: Combat Evolved",
    "imgURL": "https://howlongtobeat.com/games/Halobox.jpg",
    "main": "10 Hours",
    "mainAndExtras": "11 Hours",
    "completionist": "13 Hours"
  }
]
```
