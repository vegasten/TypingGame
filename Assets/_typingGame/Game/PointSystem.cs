using TMPro;
using UnityEngine;

public class PointSystem : MonoBehaviour
{
    [SerializeField] GameObject _scorePanel = null;
    [SerializeField] TMP_Text _pointText = null;

    public int Points { get; private set; }

    private void Start()
    {
        _pointText.text = "0";
        _scorePanel.SetActive(true);
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

    public void SetScorePanelActive(bool enable)
    {
        _scorePanel.SetActive(enable);
    }
    
    private void updateUI()
    {
        _pointText.text = Points.ToString();
    }
}
