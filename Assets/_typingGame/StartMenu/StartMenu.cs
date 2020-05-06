using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DifficultyEnum;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Button _startGameButton = null;
    [SerializeField] private Button _changeDifficultyButton = null;
    [SerializeField] private TMP_Text _difficultyText = null;

    private void Start()
    {
        _startGameButton.onClick.AddListener(onStartGame);
        _changeDifficultyButton.onClick.AddListener(onChangeDifficulty);

        updateUI();
    }

    private void onStartGame()
    {
        ExecutiveManager.Instance.LoadGameScene();
    }

    private void onChangeDifficulty()
    {
        var currentDifficulty = ExecutiveManager.Instance.GetDifficultyData().Difficulty;

        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                ExecutiveManager.Instance.SetDifficulty(Difficulty.Medium);
                break;
            case Difficulty.Medium:
                ExecutiveManager.Instance.SetDifficulty(Difficulty.Hard);
                break;
            case Difficulty.Hard:
                ExecutiveManager.Instance.SetDifficulty(Difficulty.Easy);
                break;
            default:
                Debug.LogError("Reached a difficulty state that should not be possible");
                break;
        }

        updateUI();
    }

    private void updateUI()
    {
        _difficultyText.text = $"Vanskelighetsgrad: {ExecutiveManager.Instance.GetDifficultyData().Difficulty.ToString()}";
    }
}
