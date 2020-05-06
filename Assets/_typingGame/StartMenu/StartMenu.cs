using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        var currentDifficulty = ExecutiveManager.Instance.GetCurrentDifficultyData();
        var allDifficulties = ExecutiveManager.Instance.AllDifficultiesData;

        int newIndex = (allDifficulties.IndexOf(currentDifficulty) + 1) % allDifficulties.Count;

        ExecutiveManager.Instance.SetDifficulty(allDifficulties[newIndex].Difficulty);

        updateUI();
    }

    private void updateUI()
    {
        _difficultyText.text = $"Vanskelighetsgrad: {ExecutiveManager.Instance.GetCurrentDifficultyData().Difficulty.ToString()}";
    }
}
