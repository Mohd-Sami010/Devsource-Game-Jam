using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [SerializeField] private float weakShakeDuration = 0.15f;
    [SerializeField] private float weakShakeMagnitude = 0.05f;
    [SerializeField] private float mediumShakeDuration = 0.25f;
    [SerializeField] private float mediumShakeMagnitude = 0.15f;
    [SerializeField] private float strongShakeDuration = 0.4f;
    [SerializeField] private float strongShakeMagnitude = 0.3f;

    private Vector3 originalLocalPosition;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        originalLocalPosition = transform.localPosition;
    }
    private void Start()
    {
        GameManager.Instance.OnGameOver += (_, gameOverType) => {
        if (gameOverType == GameManager.GameOverType.Died) ShakeStrong();
        };
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void ShakeWeak()
    {
        StartShake(weakShakeDuration, weakShakeMagnitude);
    }

    public void ShakeMedium()
    {
        StartShake(mediumShakeDuration, mediumShakeMagnitude);
    }

    public void ShakeStrong()
    {
        StartShake(strongShakeDuration, strongShakeMagnitude);
    }

    private void StartShake(float duration, float magnitude)
    {
        if (Instance != this)
        {
            return;
        }

        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        transform.localPosition = originalLocalPosition;
        shakeCoroutine = StartCoroutine(Shake(duration, magnitude));
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            Vector2 offset = Random.insideUnitCircle * magnitude;
            transform.localPosition = originalLocalPosition + new Vector3(offset.x, offset.y, 0);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localPosition = originalLocalPosition;
        shakeCoroutine = null;
    }
}
