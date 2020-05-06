using System.Collections.Generic;
using UnityEngine;

public class HealthPresenter : MonoBehaviour
{
    [SerializeField] private Sprite _boatSprite;
    [SerializeField] private Dictionary<int, Sprite> _boatSpriteForHealthLeft;

    public void UpdateHealth(int healthLeft)
    {
        Debug.Log("Update health left presentation");
    }
}
