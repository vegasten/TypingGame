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

    private LetterTyper _letterTyper = null;

    private Word _activeWord;
    private HashSet<Word> _existingWords;

    private RulesDataModel _rules;

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
    }

    private async void onWordCompleted()
    {
        _pointSystem.IncrementScore(_activeWord.GetWord.Length);

        _activeWord.OnWordFinishedTyped -= onWordCompleted;
        _activeWord.OnWordNotCompleted -= onWordNotCompleted;

        _existingWords.Remove(_activeWord);

        var targetTransformForAnimation = _activeWord.transform;
        _activeWord = null;

        await _spriteAnimation.AnimateWordCompleted(targetTransformForAnimation);
        Destroy(targetTransformForAnimation.gameObject);
    }

    private void onWordNotCompleted(Word word)
    {
        _healthSystem.TakeDamage(_rules.DamageOnFailedWord);

        word.OnLetterSuccesfullyTyped -= onLetterSuccessfullyTyped;
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
}
