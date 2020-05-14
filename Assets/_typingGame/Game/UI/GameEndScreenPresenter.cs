using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndScreenPresenter : MonoBehaviour
{
    public event Action OnRestartButtonClicked;
    public event Action OnReturnToStartMenuButtonClicked;

    [SerializeField] private TMP_Text _scoreText = null;
    [SerializeField] private Button _restartButton = null;
    [SerializeField] private Button _returnToStartMenuButton = null;
    [SerializeField] private GameObject _fadeScreen = null;   

    private void Start()
    {
        _restartButton.onClick.AddListener(() => OnRestartButtonClicked?.Invoke());
        _returnToStartMenuButton.onClick.AddListener(() => OnReturnToStartMenuButtonClicked?.Invoke());

        _fadeScreen.SetActive(false);
    }

    public void SetGameEndScreenUI(int score)
    {
        _fadeScreen.SetActive(true);
        _scoreText.text = $"Poengsum: {score}";
    }
}
