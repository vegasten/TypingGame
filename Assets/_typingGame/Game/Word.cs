using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Word : MonoBehaviour
{
    public event Action OnWordFinishedTyped;
    public event Action<Word> OnWordNotCompleted;

    [SerializeField] private WordDisplay _display = null;
    private Rigidbody2D _rigidBody;

    private string _word;
    public string GetWord => _word;

    private int _currentTypedIndex;

    private float _fallingSpeed;
    private float _yDestroyHeight;

    private bool _canFail = true;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _rigidBody.velocity = new Vector2(0, -_fallingSpeed);

        if (_canFail && isInDestroyArea())
        {
            destroyWord();
        }
    }

    public void InitalizeWord(string word, Vector2 randomPosition, RulesDataModel rulesData)
    {
        _word = word;
        _display.SetWord(word);
        _display.DisplayWord(word, randomPosition);

        _fallingSpeed = rulesData.FallingSpeed;
        _yDestroyHeight = rulesData.DestroyHeight;
    }

    public void TryToTypeNextLetter(char letter)
    {
        if (letter.Equals(_word[_currentTypedIndex]))
        {
            _display.ColorNextLetter();
            _currentTypedIndex++;

            if (_currentTypedIndex == _word.Length)
            {
                _canFail = false;
                OnWordFinishedTyped?.Invoke();
            }
        }
    }

    private bool isInDestroyArea()
    {
        return transform.position.y < _yDestroyHeight;
    }

    private void destroyWord()
    {
        OnWordNotCompleted?.Invoke(this);
    }
}
