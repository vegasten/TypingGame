using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DifficultyEnum;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _changeDifficultyButton;
    [SerializeField] private TMP_Text _difficultyText;

    private Difficulty _currentDifficulty = Difficulty.Easy;
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }

    private void Start()
    {
        _startGameButton.onClick.AddListener(onStartGame);
        _changeDifficultyButton.onClick.AddListener(onChangeDifficulty);

        updateUI();
    }

    private void onStartGame()
    {
        _gameManager.LoadGameScene();
    }

    private void onChangeDifficulty()
    {
        switch (_currentDifficulty)
        {
            case Difficulty.Easy:
                _currentDifficulty = Difficulty.Medium;
                break;
            case Difficulty.Medium:
                _currentDifficulty = Difficulty.Hard;
                break;
            case Difficulty.Hard:
                _currentDifficulty = Difficulty.Easy;
                break;
            default:
                Debug.LogError("Reached a difficulty state that should not be possible");
                break;
        }

        _gameManager.SetDifficulty(_currentDifficulty);
        updateUI();
    }

    private void updateUI()
    {
        _difficultyText.text = $"Vanskelighetsgrad: {_currentDifficulty.ToString()}";
    }
}
