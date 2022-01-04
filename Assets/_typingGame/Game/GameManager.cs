using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputReader _inputReader = null;

    [Header("Game")]
    [SerializeField] private float _destroyHeight = 200f;
    [SerializeField] private HealthSystem _healthSystem = null;
    [SerializeField] private PointSystem _pointSystem = null;
    [SerializeField] private WordSpawner _wordSpawner = null;

    [Header("UI")]
    [SerializeField] private Countdown _countdown = null;
    [SerializeField] private GameEndScreenPresenter _gameEndScreenPresenter = null;

    [Header("Animation")]
    [SerializeField] private SpritesAnimation _spriteAnimation = null;

    [Header("Audio")]
    [SerializeField] private PirateSceneAudio _audio;

    private LetterTyper _letterTyper = null;

    private Word _activeWord;
    private HashSet<Word> _existingWords;

    private RulesDataModel _rules;
    private int _completedWords = 0;

    private bool _shouldGenerateWords = true;

    private void Awake()
    {
        _letterTyper = new LetterTyper();
        _existingWords = new HashSet<Word>();
    }

    private void Start()
    {
        initializeDifficultyRules();

        _inputReader.OnLetterTyped += typeLetter;
        _letterTyper.OnActivatingNewWordFailed += onLetterTypingFailed;
        _healthSystem.OnDeath += onGameLost;

        _gameEndScreenPresenter.OnRestartButtonClicked += restartGame;
        _gameEndScreenPresenter.OnReturnToStartMenuButtonClicked += goBackToStartMenu;

        _countdown.OnCountdownCompleted += startWordSpawner;
        _countdown.StartCountdownSequence();
    }

    private void OnDestroy()
    {
        _inputReader.OnLetterTyped -= typeLetter;
        _letterTyper.OnActivatingNewWordFailed -= onLetterTypingFailed;
        _healthSystem.OnDeath -= onGameLost;

        _gameEndScreenPresenter.OnRestartButtonClicked -= restartGame;
        _gameEndScreenPresenter.OnReturnToStartMenuButtonClicked -= goBackToStartMenu;

        _countdown.OnCountdownCompleted -= startWordSpawner;
    }

    private void initializeDifficultyRules()
    {
        var difficultyData = ExecutiveManager.Instance.GetCurrentDifficultyData();

        _rules = new RulesDataModel
        {
            FallingSpeed = difficultyData.FallingSpeed,
            DestroyHeight = _destroyHeight,
            MaxNumberOfAliveWords = difficultyData.MaxWordsAtTheSameTime,
            TimeBetweenSpawningWord = difficultyData.TimeBetweenSpawningWord,
            DamageOnFailedWord = 1,
            MinWordLength = difficultyData.MinWordLength,
            MaxWordLength = difficultyData.MaxWordLength
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
            if (_existingWords.Count < _rules.MaxNumberOfAliveWords)
            {
                var word = _wordSpawner.SpawnRandomWord(_existingWords, _rules);
                word.OnLetterSuccesfullyTyped += onLetterSuccessfullyTyped;
                word.OnLetterTypingFailed += onLetterTypingFailed;
                word.OnWordFinishedTyped += onWordCompleted;
                word.OnWordNotCompleted += onWordNotCompleted;

                _existingWords.Add(word);
            }

            yield return new WaitForSeconds(_rules.TimeBetweenSpawningWord);
        }
    }

    private void typeLetter(char letter)
    {
        var newWord = _letterTyper.TryToTypeLetter(letter, _activeWord, _existingWords); // TODO This is shit
        if (newWord)
        {
            _activeWord = newWord;
            newWord.TryToTypeNextLetter(letter);
        }
    }

    private void onLetterSuccessfullyTyped()
    {
        _pointSystem.IncrementScore(1);
        _audio.playLetterHitSound();
    }

    private void onLetterTypingFailed()
    {
        _audio.playLetterMissSound();
    }

    private void onWordCompleted()
    {
        _pointSystem.IncrementScore(_activeWord.GetWord.Length);
        _completedWords++;

        if (_completedWords % 10 == 0)
        {
            increaseDifficulty();
        }


        _activeWord.OnWordFinishedTyped -= onWordCompleted;
        _activeWord.OnWordNotCompleted -= onWordNotCompleted;

        _existingWords.Remove(_activeWord);

        var targetTransformForAnimation = _activeWord.transform;
        _activeWord = null;

        _spriteAnimation.AnimateWordCompleted(targetTransformForAnimation);
        StartCoroutine(destroyWordAfterDelay(targetTransformForAnimation.gameObject));
    }
    
    private IEnumerator destroyWordAfterDelay(GameObject word)
    {
        yield return new WaitForSeconds(0.4f); // TODO Magic number same as in SpritesAnimation
        Destroy(word);
    }

    private void onWordNotCompleted(Word word)
    {
        _healthSystem.TakeDamage(_rules.DamageOnFailedWord);

        word.OnLetterSuccesfullyTyped -= onLetterSuccessfullyTyped;
        word.OnLetterTypingFailed -= onLetterTypingFailed;
        word.OnWordFinishedTyped -= onWordCompleted;
        word.OnWordNotCompleted -= onWordNotCompleted;

        if (word == _activeWord)
        {
            _activeWord = null;
        }

        _existingWords.Remove(word);
        _spriteAnimation.AnimateWordFailed(word.transform.position);
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
        _pointSystem.SetScorePanelActive(false);
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

    private void increaseDifficulty() // TODO This is a massive hack 
    {
        Debug.Log($"{_completedWords} of completed words! Increasing difficulty!");


        int maxWordLength = 7;
        int maxWordsAlive = 15;
        float minimunSpawningDelay = 0.2f;

        if (_rules.MaxWordLength < maxWordLength && _completedWords % 20 == 0)
        {
            _rules.MaxWordLength++;
            return;
        }
        else if (_rules.MaxNumberOfAliveWords < maxWordsAlive && (_completedWords + 10) % 20 == 0)
        {
            _rules.MaxNumberOfAliveWords++;
        }

        if (_rules.TimeBetweenSpawningWord > minimunSpawningDelay)
        {
            _rules.TimeBetweenSpawningWord -= 0.1f;
        }

        _rules.FallingSpeed += 2;
    }
}
