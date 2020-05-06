using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty", menuName = "Difficulty", order = 1)]
public class GameDifficultyScriptableObject : ScriptableObject
{
    public float DropSpeed = 50f;
    public int MinWordLength = 2;
    public int MaxWordLength = 5;
}
