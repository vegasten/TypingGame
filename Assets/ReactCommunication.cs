using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReactCommunication : MonoBehaviour
{
    [SerializeField]
    private Button _testReactCommunicationButton;

    [SerializeField]
    private TMP_Text _testText;

    private void Start()
    {
        _testReactCommunicationButton.onClick.AddListener(TestMethod);
    }

    [DllImport("__Internal")]
    private static extern void TestEvent(string message);

    public void TestMethod()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        TestEvent("This is a message from Unity");
#endif
    }

    public void ReactTest(int testNumber)
    {
        _testText.text = testNumber.ToString();
    }
}
