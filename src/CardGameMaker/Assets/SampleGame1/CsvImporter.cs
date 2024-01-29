using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CsvImporter
{
    public static CsvCardType[] LoadCsvCardTypes(string assetPath)
    {
        var cardTypes = new List<CsvCardType>();

        var lines = File.ReadAllLines(assetPath);

        for (var i = 1; i < lines.Length; i++)
        {
            var columns = lines[i].Split('|');

            var back = columns[1];
            var name = columns[2];
            var finalBack = !string.Equals(back, "Self") ? back : name;
            var cardType = new CsvCardType
            {
                Layout = columns[0],
                Back = finalBack,
                Name = name,
                NumCopies = int.Parse(columns[3]),
                Type = columns[4],
                TypePrefix = columns[5],
                Description = columns[6],
                Art = LoadSpriteFromResources(columns[5] + name.Replace(" ", "_")),
                Icon = LoadSpriteFromResources("i_" + columns[4])
            };

            cardTypes.Add(cardType);
        }

        return cardTypes.ToArray();
    }

    private static Sprite LoadSpriteFromResources(string spriteName)
    {
        return Resources.Load<Sprite>(spriteName.ToLowerInvariant());
    } 
}

public class CsvCardType : ICardType
{
    public string Layout { get; set; }
    public string Back { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public string TypePrefix { get; set; }
    public int NumCopies { get; set; }

    public Sprite Art { get; set; }
    public Sprite Icon { get; set; }
}
