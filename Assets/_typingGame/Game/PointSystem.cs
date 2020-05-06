using TMPro;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    [SerializeField] TMP_Text _pointText;

    public int Points { get; private set; }

    private void Start()
    {
        _pointText.text = "0";
    }

    public void IncrementScore(int points)
    {
        Points += points;
        updateUI();
    }

    public void ResetScore()
    {
        Points = 0;
    }
    
    private void updateUI()
    {
        _pointText.text = Points.ToString();
    }
}
