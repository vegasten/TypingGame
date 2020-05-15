using UnityEngine;

public class RandomWeight : MonoBehaviour
{
    public static RandomWeight Instance;

    public AnimationCurve _animationCurve;

    private void Awake()
    {
        Instance = this;
    }

    public int GetRandomNumber(int minimum, int maximum)
    {
        int diff = maximum - minimum;
        int randomToAdd = (int)(diff * _animationCurve.Evaluate(Random.value));

        return minimum + randomToAdd;         
    }
}
