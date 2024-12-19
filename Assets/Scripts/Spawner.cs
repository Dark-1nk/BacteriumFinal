using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform spawnPoint; // Location to spawn enemies
    public GameObject[] phaseOneEnemyPrefabs; // Array of enemy prefabs
    public GameObject[] phaseTwoEnemyPrefabs;
    public GameObject[] phaseThreeEnemyPrefabs;
    public float minSpawnInterval = 2f; // Minimum time between spawns
    public float maxSpawnInterval = 5f; // Maximum time between spawns
    public TowerHealth tower;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private System.Collections.IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Wait for a random interval before spawning the next enemy
            float spawnDelay = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnDelay);

            if (tower != null)
            {
                if (tower.health <= tower.maxHealth)
                {
                    maxSpawnInterval = 15;
                    // Randomly pick an enemy type from the array
                    int randomIndex = Random.Range(0, phaseOneEnemyPrefabs.Length);
                    Instantiate(phaseOneEnemyPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);

                }
                if (tower.health <= tower.maxHealth * 2/3)
                {
                    minSpawnInterval = 1;
                    // Randomly pick an enemy type from the array
                    int randomIndex = Random.Range(0, phaseTwoEnemyPrefabs.Length);
                    Instantiate(phaseTwoEnemyPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
                }
                if (tower.health <= tower.maxHealth * 1/3)
                {
                    maxSpawnInterval = 10;
                    // Randomly pick an enemy type from the array
                    int randomIndex = Random.Range(0, phaseThreeEnemyPrefabs.Length);
                    Instantiate(phaseThreeEnemyPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
                }
            }
        }
    }
}
