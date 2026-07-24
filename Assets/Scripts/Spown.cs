using UnityEngine;

public class Spown : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private int spawnCount = 3;

    // ÉXÉ|Å[ÉìîÕàÕ
    [SerializeField]
    private float spawnRadius = 5f;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;
            TriggerAction(other);
        }
    }

    void TriggerAction(Collider playerCollider)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = transform.position + new Vector3(i * 2f, 0, 0);

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.playerCollider = playerCollider;
            }
        }
    }
}