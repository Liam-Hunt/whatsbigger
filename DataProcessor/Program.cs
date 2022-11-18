// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using WhatsBigger.Core;

Console.WriteLine("Hello, World!");


var cities = new List<City>();

Console.WriteLine(Directory.GetCurrentDirectory());


var first = true;
foreach (var line in File.ReadLines(Path.Combine(Directory.GetCurrentDirectory(), "worldcities.csv")))
{
    if (first != true)
    {
        var parts = line.Split(@""",""");

        // Some cities don't have a population
        if (int.TryParse(parts[9], out var population)) {
            cities.Add(new City
            {
                Name = parts[0].Replace(@"""", ""),
                Population = population,
                Country = parts[4],
                Capital = parts[8]
            });
        }
    }

    first = false;
}

Console.WriteLine($"{cities.Count()} cities loaded");

File.WriteAllText("uk.json", JsonSerializer.Serialize(
    cities.Where(x => x.Country == "United Kingdom")
           .OrderByDescending(x => x.Population)
           .Take(50)));

File.WriteAllText("usa.json", JsonSerializer.Serialize(
    cities.Where(x => x.Country == "United States")
           .OrderByDescending(x => x.Population)
           .Take(50)));


File.WriteAllText("global.json", JsonSerializer.Serialize(
    cities.GroupBy(x => x.Country)
           .SelectMany(x => x.OrderByDescending(y => y.Population).Take(2))));