using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public event Action OnCountdownCompleted;

    [SerializeField] private TMP_Text _countdownText;

    public void StartCountdownSequence(int startNumber = 3, float waitBetweenEachNumber = 1.0f)
    {
        _countdownText.text = startNumber.ToString();
        _countdownText.gameObject.SetActive(true);
        StartCoroutine(countdownCoroutine(startNumber, waitBetweenEachNumber));
    }

    private IEnumerator countdownCoroutine(int number, float waitBetweenEach)
    {
        for (int i = number; i > 0; i--)
        {
            _countdownText.text = i.ToString();
            yield return new WaitForSeconds(waitBetweenEach);
        }

        _countdownText.gameObject.SetActive(false);
        OnCountdownCompleted?.Invoke();
    }
}
