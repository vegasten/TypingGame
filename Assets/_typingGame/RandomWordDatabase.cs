using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class RandomWordDatabase
{
    private Dictionary<int, List<string>> _lengthOfWordDictionary;
    public void InitializeDataBase()
    {
        _lengthOfWordDictionary = new Dictionary<int, List<string>>
        {
            {1, new List<string>()},
            {2, new List<string>()},
            {3, new List<string>()},
            {4, new List<string>()},
            {5, new List<string>()}
        };

        using (StreamReader sr = new StreamReader("Assets/Resources/ordliste.txt"))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                {
                    return; // TODO: Why does not continue work here?
                }

                var lineOfWords = line.ToLower().Split(' ');
                var filteredListOfWords = lineOfWords.Where(x => !x.Contains('æ') && !x.Contains('ø') && !x.Contains('å')).ToArray();

                if (filteredListOfWords.Length == 0)
                {
                    return; // TODO: Why does not continue work here?
                }   

                _lengthOfWordDictionary[filteredListOfWords[0].Length].AddRange(filteredListOfWords);
            }
        }
    }

    private string[] _words =
    {
        "ape", "annen", "anelse",
        "bare", "bæsj", "bly",
        "dust", "dumming", "drenere",
        "ekkel", "evig", "esel",
        "fisk", "fis", "flytende",
        "gud", "gass", "gul",
        "hest", "hoste", "hes",
        "ingen", "is", "ikke",
        "jazz", "jovial", "juggel"
    };

    public string GetRandomWord(int minLength, int maxLength)
    {
        int randomLength = Random.Range(minLength, maxLength + 1);
        var listOfWords = _lengthOfWordDictionary[randomLength];

        return listOfWords[Random.Range(0, listOfWords.Count)];
    }
}