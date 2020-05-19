using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

public class SpritesAnimation : MonoBehaviour
{
    [SerializeField] private Transform _backgroundWaves = null;
    [SerializeField] private Transform _middleGroundWaves = null;
    [SerializeField] private Transform _foreGroundWaves = null;
    [SerializeField] private Transform _boat = null;
    [SerializeField] private Transform _cannon = null;
    [SerializeField] private Transform _cannonSpawnPoint = null;
    [SerializeField] private GameObject _cannonBallPrefab = null;

    [Header("Particle effects")]
    [SerializeField] private GameObject _explosionParticleEffectPrefab = null;
    [SerializeField] private GameObject _bigExplosionParticleEffectPrefab = null;
    [SerializeField] private GameObject _smokeParticleEffectPrefab = null;

    [Header("Camera Shake")]
    [SerializeField] private CameraShaker _cameraShaker;

    [Header("Audio")]
    [SerializeField] private PirateSceneAudio _audio;
     
    private void Start()
    {
        animateSurroundings();
    }

    private void animateSurroundings()
    {
        _foreGroundWaves.DOMove(_foreGroundWaves.transform.position + new Vector3(1, 0.2f, 0), 1.4f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        _middleGroundWaves.DOMove(_middleGroundWaves.transform.position + new Vector3(1, 0.2f, 0), 1.6f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        _backgroundWaves.DOMove(_foreGroundWaves.transform.position + new Vector3(1, 0.2f, 0), 1.8f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        _boat.DOMove(_boat.transform.position + new Vector3(3f, 0.2f, 0), 3).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    public async Task AnimateWordCompleted(Transform target)
    {
        var targetWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(target.position.x, target.position.y, 10));
        await shootCannonAtPosition(targetWorldPosition);
    }

    public void AnimateWordFailed(Vector3 canvasTarget)
    {
        var targetWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(canvasTarget.x, canvasTarget.y, 10));

        _audio.playWordFailedSound();
        _cameraShaker.Shake(0.3f, 0.3f);
        spawnParticleSystemAtPosition(targetWorldPosition, _bigExplosionParticleEffectPrefab);
    }

    private async Task shootCannonAtPosition(Vector3 targetPosition) 
    {

        float animationTime = 0.4f; // TODO Calculate this number from the distance and vary by set speed
               
        _cannon.right = targetPosition - _cannon.position;
        spawnParticleSystemAtPosition(_cannonSpawnPoint.position, _smokeParticleEffectPrefab);
        _audio.playCannonShotSound();

        var cannonball = Instantiate(_cannonBallPrefab, _cannonSpawnPoint.position, Quaternion.identity);
        cannonball.transform.DOMove(targetPosition, animationTime).SetEase(Ease.Linear);

        await Task.Delay((int)(animationTime * 1000));

        _cameraShaker.Shake(0.1f, 0.1f);
        _audio.playExplosionSound();

        spawnParticleSystemAtPosition(cannonball.transform.position, _explosionParticleEffectPrefab);
        Destroy(cannonball);
    }

    private void spawnParticleSystemAtPosition(Vector3 position, GameObject particleEffectPrefab)
    {
        var particleSystemGameObject = Instantiate(particleEffectPrefab, position, Quaternion.identity);
        StartCoroutine(destroyObjectWhenParticleSystemIsDone(particleSystemGameObject));
    }

    private IEnumerator destroyObjectWhenParticleSystemIsDone(GameObject gameobjectWithParticleSystem)
    {
        var particleSystem = gameobjectWithParticleSystem.GetComponent<ParticleSystem>();        
        Assert.IsNotNull(particleSystem, "Trying to use a gameobject that does not have a particle system");

        yield return new WaitForSeconds(particleSystem.main.duration);
        Destroy(gameobjectWithParticleSystem);
    }
}
