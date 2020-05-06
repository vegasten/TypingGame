using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputReader _inputReader = null;
    [SerializeField] private WordSpawner _wordSpawner = null;

    [Header("Game")]
    [SerializeField] private float _yDestroyHeight = 200f;
    [SerializeField] private HealthSystem _healthSystem = null;
    [SerializeField] private PointSystem _pointSystem = null;

    [Header("UI")]
    [SerializeField] private Countdown _countdown = null;
    [SerializeField] private GameEndScreenPresenter _gameEndScreenPresenter = null;

    private Word _activeWord;
    private HashSet<Word> _existingWords;

    private RulesDataModel _rulesData;

    private bool _shouldGenerateWords = true;

    private void Awake()
    {
        _existingWords = new HashSet<Word>();
    }

    private void Start()
    {
        initializeDifficultyRules();

        _inputReader.OnLetterTyped += typeLetter;
        _healthSystem.OnDeath += onGameLost;

        _gameEndScreenPresenter.OnRestartButtonClicked += restartGame;
        _gameEndScreenPresenter.OnReturnToStartMenuButtonClicked += goBackToStartMenu;

        _countdown.OnCountdownCompleted += startWordSpawner;
        _countdown.StartCountdownSequence();
    }

    private void OnDestroy()
    {
        _inputReader.OnLetterTyped -= typeLetter;
        _healthSystem.OnDeath -= onGameLost;

        _gameEndScreenPresenter.OnRestartButtonClicked -= restartGame;
        _gameEndScreenPresenter.OnReturnToStartMenuButtonClicked -= goBackToStartMenu;

        _countdown.OnCountdownCompleted -= startWordSpawner;
    }

    private void initializeDifficultyRules()
    {
        _rulesData = new RulesDataModel
        {
            FallingSpeed = ExecutiveManager.Instance.GetCurrentDifficultyData().DropSpeed,
            DestroyHeight = _yDestroyHeight,
            MaxNumberOfAliveWords = ExecutiveManager.Instance.GetCurrentDifficultyData().MaxWordsAtTheSameTime,
            TimeBetweenSpawn = 1.0f
        };
    }

    private void startWordSpawner()
    {
        StartCoroutine(wordSpawnCoroutine());
    }

    private IEnumerator wordSpawnCoroutine()
    {
        while (_shouldGenerateWords)
        {
            while (_shouldGenerateWords)
            {
                if (_existingWords.Count < _rulesData.MaxNumberOfAliveWords)
                {
                    var word = _wordSpawner.SpawnRandomWord(_existingWords, _rulesData);                    
                    word.OnWordFinishedTyped += onWordCompleted;
                    word.OnWordNotCompleted += onWordNotCompleted;

                    _existingWords.Add(word);
                }

                yield return new WaitForSeconds(_rulesData.TimeBetweenSpawn);
            }
        }
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
        _pointSystem.IncrementScore(1);

        _activeWord.OnWordFinishedTyped -= onWordCompleted;
        _activeWord.OnWordNotCompleted -= onWordNotCompleted;

        _existingWords.Remove(_activeWord);
        Destroy(_activeWord.gameObject);
        _activeWord = null;
    }

    private void onWordNotCompleted(Word word)
    {
        _healthSystem.TakeDamage(1);

        word.OnWordFinishedTyped -= onWordCompleted;
        word.OnWordNotCompleted -= onWordNotCompleted;

        if (word == _activeWord)
        {
            _activeWord = null;
        }

        _existingWords.Remove(word);
        Destroy(word.gameObject);
    }

    private void onGameLost()
    {
        _shouldGenerateWords = false;
        foreach (var word in _existingWords)
        {
            Destroy(word.gameObject);
        }

        _activeWord = null;

        _gameEndScreenPresenter.SetGameEndScreenUI(_pointSystem.Points);
        _gameEndScreenPresenter.gameObject.SetActive(true);
        _pointSystem.ResetScore();
    }

    private void goBackToStartMenu()
    {
        ExecutiveManager.Instance.LoadStartMenuScene();
    }

    private void restartGame()
    {
        ExecutiveManager.Instance.LoadGameScene();
    }
}
