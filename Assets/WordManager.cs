using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    private const int MAX_NUMBER_OF_EXISTING_WORDS = 5;

    [Header("Input")]
    [SerializeField] InputReader _inputReader = null;

    [Header("Game")]
    [SerializeField] RectTransform _wordContainer = null;
    [SerializeField] GameObject _wordDisplayPrefab = null;

    private WordGenerator _wordGenerator;

    private Word _activeWord;
    private HashSet<Word> _existingWords;

    private bool _shouldGenerateWords = true;

    private void Awake()
    {
        _existingWords = new HashSet<Word>();
        _wordGenerator = new WordGenerator(this);
    }

    private void Start()
    {
        _inputReader.OnLetterTyped += typeLetter;
        startWordSpawner();
    }

    private void OnDestroy()
    {
        _inputReader.OnLetterTyped -= typeLetter;
    }

    private void startWordSpawner()
    {
        StartCoroutine(wordSpawnCoroutine());
    }

    private IEnumerator wordSpawnCoroutine()
    {
        while (_shouldGenerateWords)
        {
            if (_existingWords.Count < MAX_NUMBER_OF_EXISTING_WORDS)
            {
                spawnRandomWord();
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void spawnRandomWord()
    {
        string wordString = _wordGenerator.GenerateRandomWordWithUniqueFirstLetter(_existingWords);

        var word = Instantiate(_wordDisplayPrefab, _wordContainer).GetComponent<Word>();
        _existingWords.Add(word);
        Vector2 randomPosition = getRandomPositionOnWordContainer();
        word.InitalizeWord(wordString, randomPosition);

        word.OnWordFinishedTyped += onWordCompleted;
    }

    private Vector2 getRandomPositionOnWordContainer()
    {
        var width = _wordContainer.rect.width;
        var height = _wordContainer.rect.height;

        var randomX = Random.Range(0, width) + 100; // TODO Fix this magic number
        var randomY = Random.Range(0, height) + 100;

        return new Vector2(randomX, randomY);
    }

    private void typeLetter(char letter)
    {
        if (string.IsNullOrEmpty(_activeWord?.GetWord))
        {
            Word wordWithCorrectFirstLetter = existingWordWithFirstLetter(letter);
            if (wordWithCorrectFirstLetter == null)
            {
                return;
            }

            _activeWord = wordWithCorrectFirstLetter;
            _activeWord.TryToTypeNextLetter(letter);
        }

        else
        {
            _activeWord.TryToTypeNextLetter(letter);
        }
    }

    private Word existingWordWithFirstLetter(char letter)
    {
        var firstWordWithCorrectFirstLetter = _existingWords.Where(word => word.GetWord[0] == letter).FirstOrDefault();
        return firstWordWithCorrectFirstLetter;
    }

    private void onWordCompleted()
    {
        _activeWord.OnWordFinishedTyped -= onWordCompleted;

        Debug.Log("DONE WITH WORD");
        _existingWords.Remove(_activeWord);
        Destroy(_activeWord.gameObject);
        _activeWord = null;
    }
}
