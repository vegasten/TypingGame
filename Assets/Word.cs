using System;
using UnityEngine;

public class Word : MonoBehaviour
{
    public event Action OnWordFinishedTyped;

    [SerializeField] private WordDisplay _display = null;

    private string _word;
    public string GetWord => _word;

    private int _currentTypedIndex;


    public void InitalizeWord(string word, Vector2 randomPosition)
    {
        _word = word;
        _display.SetWord(word);
        _display.DisplayWord(word, randomPosition);
    }

    public void TryToTypeNextLetter(char letter)
    {
        if (letter.Equals(_word[_currentTypedIndex]))
        {
            _display.ColorNextLetter();
            _currentTypedIndex++;

            if (_currentTypedIndex == _word.Length)
            {
                OnWordFinishedTyped?.Invoke();
            }
        }
    }
}
