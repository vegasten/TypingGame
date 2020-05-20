using TMPro;
using UnityEngine;

public class WordDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text _text = null;

    private string _word;
    private int _lettersColored;

    private string _neutralColor = "\"red\"";
    private string _coloredColor = "\"yellow\"";

    private void Awake()
    {
        //_neutralColor = "#" + ColorUtility.ToHtmlStringRGBA(Color.red);
        //_coloredColor = "#" + ColorUtility.ToHtmlStringRGBA(Color.yellow);
    }

    public void DisplayWord(string word, Vector2 randomPosition)
    {
        _text.text = $"<color={_neutralColor}>{word}</color>";
        _word = word;

        transform.localPosition = randomPosition;
    }

    public void SetWord(string word)
    {
        _word = word;
    }

    public void ColorNextLetter()
    {
        _lettersColored++;
        string coloredLetters = _word.Substring(0, _lettersColored);
        string neutralLetters = _word.Substring(_lettersColored);
        string wordWithColorTag = $"<color={_coloredColor}>{coloredLetters}</color><color={_neutralColor}>{neutralLetters}</color>";

        _text.text = wordWithColorTag;

    }
}
