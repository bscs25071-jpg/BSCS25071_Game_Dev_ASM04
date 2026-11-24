using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject pickupParticlesPrefab;

    void Update() => transform.Rotate(0f, 120f * Time.deltaTime, 0f, Space.World);

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayPickup();

        if (pickupParticlesPrefab != null)
            Instantiate(pickupParticlesPrefab, transform.position, Quaternion.identity);

        if (GameManager.Instance != null)
            GameManager.Instance.AddScore(1);

        Destroy(gameObject);
    }
}