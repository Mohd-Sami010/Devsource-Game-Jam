using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioSource dieAudioSource;
    [SerializeField] private AudioSource winAudioSource;
    [SerializeField] private AudioSource abilityPickupAudioSource;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GameManager.Instance.OnGameOver += (_, gameOverType) => {
        if (gameOverType == GameManager.GameOverType.Died) PlaySoundWithRandomPitch(dieAudioSource);
        else if (gameOverType == GameManager.GameOverType.Won) PlaySoundWithRandomPitch(winAudioSource);
        };
    }
    public void PlayAbilityPickupSound()
    {
        PlaySoundWithRandomPitch(abilityPickupAudioSource);
    }

    private void PlaySoundWithRandomPitch(AudioSource audioSource, float minPitch = 0.8f, float maxPitch = 1.2f)
    {
        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.Play();
    }
}
