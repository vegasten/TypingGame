using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DifficultyEnum;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private Button _startGameButton = null;
    [SerializeField] private Button _changeDifficultyButton = null;
    [SerializeField] private TMP_Text _difficultyText = null;

    [Header("Difficulty text colors")]
    [SerializeField] private Color _easyColor;
    [SerializeField] private Color _mediumColor;
    [SerializeField] private Color _hardColor;

    [Header("Audio")]
    [SerializeField] private AudioSource _buttonClickAudio;

    private void Start()
    {
        _startGameButton.onClick.AddListener(onStartGame);
        _changeDifficultyButton.onClick.AddListener(onChangeDifficulty);

        updateUI();
    }

    private void onStartGame()
    {
        _buttonClickAudio.Play();

        ExecutiveManager.Instance.LoadGameScene();
    }

    private void onChangeDifficulty()
    {
        _buttonClickAudio.Play();

        var currentDifficulty = ExecutiveManager.Instance.GetCurrentDifficultyData();
        var allDifficulties = ExecutiveManager.Instance.AllDifficultiesData;

        int newIndex = (allDifficulties.IndexOf(currentDifficulty) + 1) % allDifficulties.Count;

        ExecutiveManager.Instance.SetDifficulty(allDifficulties[newIndex].Difficulty);

        updateUI();
    }

    private void updateUI()
    {
        var difficulty = ExecutiveManager.Instance.GetCurrentDifficultyData().Difficulty;
        setToNorwegianDifficultyText(difficulty);
    }

    private void setToNorwegianDifficultyText(Difficulty difficulty)
    {        
        switch(difficulty)
        {
            case Difficulty.Easy:
                _difficultyText.text = "Lett";
                _difficultyText.color = _easyColor;
                break;
            case Difficulty.Medium:
                _difficultyText.text = "Middels";
                _difficultyText.color = _mediumColor;
                break;
            case Difficulty.Hard:
                _difficultyText.text = "Vanskelig";
                _difficultyText.color = _hardColor;
                break;
            default:
                _difficultyText.text = "Error???";
                _difficultyText.color = Color.white;
                break;
        }
    }
}
