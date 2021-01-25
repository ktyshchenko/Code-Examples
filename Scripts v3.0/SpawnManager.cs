using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject portalPrefab;

    public GameObject[] powerUpPrefabs;
    private const int powerUpHealth = 0;
    private const int powerUpRestore = 1;
    public static bool isSpawned = false;
    private float waitTime = 10.0f;

    public static int waveNumber;
    public int enemyCount;

    private const float xRange = 4.0f; // left or right road bounds
    private const float zTopRange = 20.0f; // front of the road bound
    private const float zBottomRange = 10.0f; // back of the road bound
    private float spawnPosX;
    private float spawnPosZ;
    private Vector3 portalLoc;

    private int levelTwoScore = 50;

    // Start is called before the first frame update
    private void Start()
    {
        SpawnEnemyWave(waveNumber);

        // Spawn power-ups randomly, excluding health power-up
        StartCoroutine(PowerUpCoroutine());
    }

    // Update is called once per frame
    private void Update()
    {
        enemyCount = FindObjectsOfType<TrollController>().Length;
        if (enemyCount == 0)
        {
            // Spawn enemy waves
            waveNumber++;
            SpawnEnemyWave(waveNumber);

            // Spawn power-up to restore health - only if required
            if (GameManager.livesLeft < GameManager.livesFull)
            {
                SpawnPowerUp(powerUpHealth);
            }
        }

        // Spawn a power-up fully restoring health with every X points
        // Don't spawn if previous one is not collected
        float scoreForRestore = GameManager.score % levelTwoScore;
        if (GameManager.score != 0 && scoreForRestore == 0f && isSpawned == false)
        {
            SpawnPowerUp(powerUpRestore);
            isSpawned = true;
        }
    }

    private void SpawnEnemyWave(int enemiesToSpawn)
    {
        if (GameManager.isGameActive == true &&
            GameManager.isGameOver == false)
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                SpawnEnemy();
            }
        }
    }

    private void SpawnEnemy()
    {
        // Randomize the appearing enemy
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);

        // Randomize the position within the specified boundaries
        spawnPosX = Random.Range(-xRange, xRange);

        portalLoc = portalPrefab.transform.localPosition;
        spawnPosZ = Random.Range(zBottomRange, portalLoc.z);

        Vector3 spawnPos = new Vector3(spawnPosX, 0f, spawnPosZ);

        // Spawn the enemies at a random loc at the portal
        if (GameManager.score >= levelTwoScore)
        {
            ActivateEnemy(enemyIndex, spawnPos); // both enhanced and normal-sized trolls appear randomly
        }
        else
        {
            ActivateEnemy(0, spawnPos); // only normal-sized trolls appear
        }
    }

    private void SpawnPowerUp(int index)
    {
        if (GameManager.isGameOver == false)
        {
            // Randomize the position within the specified boundaries
            spawnPosX = Random.Range(-xRange, xRange);
            spawnPosZ = Random.Range(-zBottomRange, zTopRange);
            Vector3 spawnPos = new Vector3(spawnPosX, 0f, spawnPosZ);

            // Spawn a power-up at a random loc
            Instantiate(powerUpPrefabs[index], spawnPos, Quaternion.identity);
        }
    }

    private void ActivateEnemy(int index, Vector3 pos)
    {
        if (index != 0)
        {
            GameObject enemy = ObjectPooling.SharedInstance.GetPooledEnemyEnhanced();

            if (enemy != null)
            {
                enemy.transform.position = pos;
                enemy.SetActive(true);
            }
        }
        else
        {
            GameObject enemy = ObjectPooling.SharedInstance.GetPooledEnemy();

            if (enemy != null)
            {
                enemy.transform.position = pos;
                enemy.SetActive(true);
            }
        }
    }

    private IEnumerator PowerUpCoroutine()
    {
        while (true)
        {
            var delay = new WaitForSeconds(waitTime);
            yield return delay;

            if (GameManager.isGameActive == true)
            {
                // Randomize the power-ups excluding health power-ups
                int powerUpIndex = Random.Range(powerUpRestore + 1, powerUpPrefabs.Length);
                SpawnPowerUp(powerUpIndex);
            }
        }
    }
}
