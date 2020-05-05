using DG.Tweening;
using UnityEngine;


public class SpritesAnimation : MonoBehaviour
{
    [SerializeField] private Transform _backgroundWaves;
    [SerializeField] private Transform _foreGroundWaves;
    [SerializeField] private Transform _boat;

  
    private void Start()
    {
        _foreGroundWaves.DOMove(_foreGroundWaves.transform.position + new Vector3(1, 0.2f, 0), 1).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        _backgroundWaves.DOMove(_foreGroundWaves.transform.position + new Vector3(1, 0.2f, 0), 0.8f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        _boat.DOMove(_foreGroundWaves.transform.position + new Vector3(3f, 0.2f, 0), 3).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);        
    }

}
