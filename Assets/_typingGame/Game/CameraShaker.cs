using System.Collections;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(shake(duration, magnitude));
    }

    private IEnumerator shake(float duration, float magnitude)
    {
        var originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }
}
