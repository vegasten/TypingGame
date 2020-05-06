using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndScreenPresenter : MonoBehaviour
{
    public event Action OnRestartButtonClicked;
    public event Action OnReturnToStartMenuButtonClicked;

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _returnToStartMenuButton;

    private void Start()
    {
        _restartButton.onClick.AddListener(() => OnRestartButtonClicked?.Invoke());
        _returnToStartMenuButton.onClick.AddListener(() => OnReturnToStartMenuButtonClicked?.Invoke());
    }

    public void SetGameEndScreenUI(int score)
    {
        _scoreText.text = $"Your score: {score}";
    }
}
