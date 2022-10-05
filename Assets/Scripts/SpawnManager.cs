using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _maxXpos;
    // Gain access to the Enemy Container game object
    [SerializeField] private GameObject _enemyContainer;
    // Gain access to the powerup game object
    [SerializeField] private GameObject[] _powerupPrefabs;
    // Check if the enemies has stopped spawning
    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    // Spawn enemy game objects every 5 seconds
    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        // While loop is true
        while (_stopSpawning == false)
        {
            // Random position on the X-axis
            float randomX = Random.Range(-_maxXpos, _maxXpos);
            // Spawn position of the Spawn Manageer
            Vector3 spawnPos = new Vector3(randomX, transform.position.y, 0);
            // Create a reference to the Instantiated enemy prefabs
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);
            // Make the Enemy Container the parent to all of the enemies spawned
            newEnemy.transform.parent = _enemyContainer.transform;
            // Wait for 5 seconds
            yield return new WaitForSeconds(5f); 
        }
    }

    // Spawn powerup every random amount of seconds
    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            // Random position on the X-axis
            float randomX = Random.Range(-_maxXpos, _maxXpos);
            // Random position to spawn
            Vector3 spawnPos = new Vector3(randomX, 8f, 0);
            // Spawn random powerup ID's
            int randomPowerup = Random.Range(0, 3);
            // Create a reference to the Instantiated powerup prefab
            GameObject spawnPowerup = Instantiate(_powerupPrefabs[randomPowerup], spawnPos, Quaternion.identity);
            // Spawn at random times between 3 to 7 seconds
            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }

    }

    // Method to stop spawning when player dies
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
