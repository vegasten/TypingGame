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
            {5, new List<string>()},
            {6, new List<string>()},
            {7, new List<string>()}
        };

        var textFile = Resources.Load<TextAsset>("ordliste");
        var lines = textFile.ToString().Split('\n'); // TODO this produces an error

        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            var lineOfWords = line.Trim().ToLower().Split(' ');
            var filteredListOfWords = lineOfWords.Where(x => !x.Contains('æ') && !x.Contains('ø') && !x.Contains('å')).ToArray();

            if (filteredListOfWords.Length == 0)
            {
                continue;
            }
            try
            {
                _lengthOfWordDictionary[filteredListOfWords[0].Length].AddRange(filteredListOfWords);
            }
            catch
            {
                Debug.LogError($"Error in initializing dictionary with line: {line}");
            }
        }


        //using (StreamReader sr = new StreamReader("Assets/Resources/ordliste.txt"))
        //{
        //    string line;
        //    while ((line = sr.ReadLine()) != null)
        //    {
        //        if (string.IsNullOrEmpty(line))
        //        {
        //            continue;
        //        }

        //        var lineOfWords = line.Trim().ToLower().Split(' ');
        //        var filteredListOfWords = lineOfWords.Where(x => !x.Contains('æ') && !x.Contains('ø') && !x.Contains('å')).ToArray();

        //        if (filteredListOfWords.Length == 0)
        //        {
        //            continue;
        //        }
        //        try
        //        {
        //            _lengthOfWordDictionary[filteredListOfWords[0].Length].AddRange(filteredListOfWords);
        //        }
        //        catch
        //        {
        //            Debug.LogError($"Error in initializing dictionary with line: {line}");
        //        }
        //    }
        //}
    }

    public string GetRandomWord(int minLength, int maxLength)
    {
        int randomLength = RandomWeight.Instance.GetRandomNumber(minLength, maxLength);

        var listOfWords = _lengthOfWordDictionary[randomLength];

        return listOfWords[Random.Range(0, listOfWords.Count)];
    }
}