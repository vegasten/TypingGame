using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    private const int MAX_NUMBER_OF_WORDS_ALIVE = 5;

    [SerializeField] InputReader _inputReader = null;
    [SerializeField] RectTransform _wordContainer = null;
    [SerializeField] GameObject _wordDisplayPrefab = null;

    private WordGenerator _wordGenerator;

    private Word _activeWord;
    private HashSet<Word> _words = new HashSet<Word>();

    private string _writtenWord;
    private int _currentIndex;
    private bool _shouldGenerateWords = true;

    private void Awake()
    {
        _wordGenerator = new WordGenerator();
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
            if (_words.Count < MAX_NUMBER_OF_WORDS_ALIVE)
            {

                bool acceptedWord = false;
                string generatedWord = "";

                while (!acceptedWord)
                {
                    generatedWord = _wordGenerator.GetRandomWord();

                    if (_words.Where(w => w.GetWord == generatedWord).FirstOrDefault()) // check if the generated word is already alive
                    {
                        continue;
                    }

                    if (_words.Where(w => w.GetWord[0] == generatedWord[0]).FirstOrDefault()) // check if there is an alive word with the same first letter
                    {
                        continue;
                    }
                    yield return null;
                    acceptedWord = true;
                }

                spawnWord(generatedWord);
                acceptedWord = false;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void spawnWord(string word)
    {
        var wordController = Instantiate(_wordDisplayPrefab, _wordContainer).GetComponent<Word>();
        _words.Add(wordController);
        Vector2 randomPosition = getRandomPositionOnWordContainer();
        wordController.InitalizeWord(word, randomPosition);
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
            Word wordWithCorrectFirstLetter = aliveWordWithFirstLetter(letter);
            if (wordWithCorrectFirstLetter == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(wordWithCorrectFirstLetter.GetWord))
                return;

            _activeWord = wordWithCorrectFirstLetter;
            _writtenWord = letter.ToString();
            _currentIndex = 1;

            _activeWord.TypeNextLetter();
        }

        else
        {
            if (_activeWord.GetWord[_currentIndex] == letter)
            {
                _writtenWord += letter;
                _currentIndex++;

                _activeWord.TypeNextLetter();
                Debug.Log($"At index: {_currentIndex} of the word: {_activeWord.GetWord}");
            }
        }

        if (isWordComplete())
        {
            onWordCompleted();
        }
    }

    private Word aliveWordWithFirstLetter(char letter)
    {
        var firstWordWithCorrectFirstLetter = _words.Where(word => word.GetWord[0] == letter).FirstOrDefault();
        return firstWordWithCorrectFirstLetter;
    }

    private bool isWordComplete()
    {
        return _activeWord.GetWord.Equals(_writtenWord);
    }

    private void onWordCompleted()
    {
        Debug.Log("DONE WITH WORD");
        _words.Remove(_activeWord);
        Destroy(_activeWord.gameObject);
        _activeWord = null;
    }
}
