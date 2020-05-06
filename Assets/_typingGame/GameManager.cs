using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DifficultyEnum;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<GameDifficultyScriptableObject> _difficultyData;

    public static GameManager Instance;

    private Difficulty _difficulty;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(1); // Load game scene
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        _difficulty = difficulty;
    }

    public GameDifficultyScriptableObject GetDifficultyData()
    {
        return _difficultyData[(int)_difficulty];
    }

    public void LoadStartMenuScene()
    {
        SceneManager.LoadScene(0); // Load start menu scene
    }    
}
