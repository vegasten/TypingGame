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
    [SerializeField] private PirateSceneAudio _audio = null;

    private void Start()
    {
        _restartButton.onClick.AddListener(onRestartButtonClicked);
        _returnToStartMenuButton.onClick.AddListener(onReturnToStartMenuButtonClicked);

        _fadeScreen.SetActive(false);
    }

    public void SetGameEndScreenUI(int score)
    {
        _fadeScreen.SetActive(true);
        _scoreText.text = $"Poengsum: {score}";
    }

    private void onRestartButtonClicked()
    {
        _audio.playButtonClickedSound();
        OnRestartButtonClicked?.Invoke();
    }

    private void onReturnToStartMenuButtonClicked ()
    {
        _audio.playButtonClickedSound();
        OnReturnToStartMenuButtonClicked?.Invoke();
    }
}
