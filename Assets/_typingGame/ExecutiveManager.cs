using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DifficultyEnum;

public class ExecutiveManager : MonoBehaviour
{
    [SerializeField] private List<GameDifficultyScriptableObject> _difficultyData = null;

    public static ExecutiveManager Instance;

    private Difficulty _difficulty = Difficulty.Easy;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
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
