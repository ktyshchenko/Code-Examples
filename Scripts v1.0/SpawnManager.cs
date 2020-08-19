using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject portalPrefab;

    public GameObject[] powerUpPrefabs;
    private int powerUpHealth = 0;
    private float waitTime = 15.0f;

    public static int waveNumber;
    public int enemyCount;

    private float xRange = 4.0f; // left or right road bounds
    private float zTopRange = 20.0f; // front of the road bound
    private float zBottomRange = 10.0f; // back of the road bound
    private float spawnPosX;
    private float spawnPosZ;
    private Vector3 portalLoc;

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
            waveNumber++;
            SpawnEnemyWave(waveNumber);

            // Spawn power-up to restore health - only if required
            if (GameManager.livesLeft < GameManager.livesFull)
            {
                SpawnPowerUp(powerUpHealth);
            }
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

        portalLoc = portalPrefab.transform.position;
        spawnPosZ = Random.Range(zBottomRange, portalLoc.z);

        Vector3 spawnPos = new Vector3(spawnPosX, 0f, spawnPosZ);

        // Spawn the enemies at a random loc at the portal
        InstantiateObject(enemyPrefabs[enemyIndex], spawnPos);
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
            InstantiateObject(powerUpPrefabs[index], spawnPos);
        }
    }

    private void InstantiateObject(GameObject obj, Vector3 pos)
    {
        Instantiate(obj,
            pos,
            obj.transform.rotation);
    }

    private IEnumerator PowerUpCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);

            if (GameManager.isGameActive == true)
            {
                // Randomize the power-ups excluding health power-up
                int powerUpIndex = Random.Range(1, powerUpPrefabs.Length);
                SpawnPowerUp(powerUpIndex);
            }
        }
    }
}
