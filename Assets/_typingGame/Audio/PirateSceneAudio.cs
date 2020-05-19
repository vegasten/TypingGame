using UnityEngine;

public class PirateSceneAudio : MonoBehaviour
{
    [SerializeField] AudioSource _cannonShotSound;
    [SerializeField] AudioSource _explosionSound;
    [SerializeField] AudioSource _letterHitSound;
    [SerializeField] AudioSource _letterMissSound;
    [SerializeField] AudioSource _wordFailedSound;
    [SerializeField] AudioSource _buttonClickedSound;

    public void playCannonShotSound()
    {
        _cannonShotSound.Play();
    }

    public void playExplosionSound()
    {
        _explosionSound.Play();
    }

    public void playLetterHitSound()
    {
        _letterHitSound.Play();
    }

    public void playLetterMissSound()
    {
        _letterMissSound.Play();
    }

    public void playWordFailedSound()
    {
        _wordFailedSound.Play();
    }

    public void playButtonClickedSound()
    {
        _buttonClickedSound.Play();
    }
}
