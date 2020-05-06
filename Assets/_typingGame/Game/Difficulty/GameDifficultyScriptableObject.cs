using UnityEngine;
using static DifficultyEnum;

[CreateAssetMenu(fileName = "Difficulty", menuName = "Difficulty", order = 1)]
public class GameDifficultyScriptableObject : ScriptableObject
{
    public Difficulty Difficulty;
    public float DropSpeed = 50f;
    public int MinWordLength = 2;
    public int MaxWordLength = 5;
    public int MaxWordsAtTheSameTime = 5;
}
