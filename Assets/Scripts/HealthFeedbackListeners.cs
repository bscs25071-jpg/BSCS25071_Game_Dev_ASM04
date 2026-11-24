using System.Collections;
using UnityEngine;

public class HealthFeedbackListeners : MonoBehaviour
{
    public Health health;
    public ParticleSystem hitParticles;
    public Transform cameraToShake;

    void OnEnable()
    {
        if (health != null)
            health.Damaged += OnPlayerDamaged;
    }

    void OnDisable()
    {
        if (health != null)
            health.Damaged -= OnPlayerDamaged;
    }

    void OnPlayerDamaged(int current, int max)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayDamage();

        if (hitParticles != null)
            hitParticles.Play();

        if (cameraToShake != null)
            StartCoroutine(ShakeCamera());
    }

    IEnumerator ShakeCamera()
    {
        Vector3 originalPos = cameraToShake.localPosition;
        float elapsed = 0f;
        while (elapsed < 0.15f)
        {
            elapsed += Time.deltaTime;
            cameraToShake.localPosition = originalPos + Random.insideUnitSphere * 0.15f;
            yield return null;
        }
        cameraToShake.localPosition = originalPos;
    }
}