using UnityEngine;

public class Word : MonoBehaviour
{
    [SerializeField] private WordDisplay _display = null;

    private string _word;
    public string GetWord => _word;

    public void InitalizeWord(string word, Vector2 randomPosition)
    {
        _word = word;
        _display.SetWord(word);
        _display.DisplayWord(word, randomPosition);
    }

    public void TypeNextLetter()
    {
        _display.ColorNextLetter();
    }
}
