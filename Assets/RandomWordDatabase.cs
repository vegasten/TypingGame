using UnityEngine;

public class RandomWordDatabase
{
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

    public string GetRandomWord()
    {
        int numberOfWords = _words.Length;
        int randomIndex = Random.Range(0, numberOfWords);

        return _words[randomIndex];
    }
}