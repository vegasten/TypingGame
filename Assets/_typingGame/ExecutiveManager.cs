using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DifficultyEnum;

public class ExecutiveManager : MonoBehaviour
{
    public static ExecutiveManager Instance;

    [SerializeField] private List<GameDifficultyScriptableObject> _difficultyData = null;
    public List<GameDifficultyScriptableObject> AllDifficultiesData => _difficultyData;
    
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

    public GameDifficultyScriptableObject GetCurrentDifficultyData()
    {
        return _difficultyData.Where(data => data.Difficulty == _difficulty).FirstOrDefault();
    }

    public void LoadStartMenuScene()
    {
        SceneManager.LoadScene(0); // Load start menu scene
    }
}
