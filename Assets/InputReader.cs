using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    public event Action<char> OnLetterTyped;

    private void Update()
    {
        foreach (char c in Input.inputString)
        {
            OnLetterTyped?.Invoke(c);
        }
    }
}
