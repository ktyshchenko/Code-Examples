using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling SharedInstance;

    public List<GameObject> weaponsPooled;
    public GameObject weaponToPool;
    public int weaponAmountToPool = 20;

    public List<GameObject> enemiesPooled;
    public List<GameObject> enemiesEnhancedPooled;
    public GameObject[] enemiesToPool;
    public int enemyAmountToPool = 100;
    private EnemyHealth enemyHealthScript;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        weaponsPooled = new List<GameObject>();
        for (int i = 0; i < weaponAmountToPool; i++)
        {
            GameObject obj = Instantiate(weaponToPool);
            obj.SetActive(false);
            weaponsPooled.Add(obj);
        }

        enemiesPooled = new List<GameObject>();
        for (int i = 0; i < enemyAmountToPool; i++)
        {
            GameObject obj = Instantiate(enemiesToPool[0]);
            obj.SetActive(false);
            enemiesPooled.Add(obj);
        }

        enemiesEnhancedPooled = new List<GameObject>();
        for (int i = 0; i < enemyAmountToPool; i++)
        {
            GameObject obj = Instantiate(enemiesToPool[1]);
            obj.SetActive(false);
            enemiesEnhancedPooled.Add(obj);
        }
    }

    public GameObject GetPooledWeapon()
    {
        for (int i = 0; i < weaponsPooled.Count; i++)
        {
            if (!weaponsPooled[i].activeInHierarchy)
            {
                return weaponsPooled[i];
            }
        }
        return null;
    }

    public GameObject GetPooledEnemy()
    {
        for (int i = 0; i < enemiesPooled.Count; i++)
        {
            if (!enemiesPooled[i].activeInHierarchy)
            {
                // Restore health for recycled enemies
                enemyHealthScript = enemiesPooled[i].GetComponent<EnemyHealth>();
                enemyHealthScript.enemyHealth = enemyHealthScript.enemyHealthMax;
                enemyHealthScript.healthBar.value = enemyHealthScript.CalculateHealth();

                return enemiesPooled[i];
            }
        }
        return null;
    }

    public GameObject GetPooledEnemyEnhanced()
    {
        for (int i = 0; i < enemiesEnhancedPooled.Count; i++)
        {
            if (!enemiesEnhancedPooled[i].activeInHierarchy)
            {
                // Restore health for recycled enemies
                enemyHealthScript = enemiesEnhancedPooled[i].GetComponent<EnemyHealth>();
                enemyHealthScript.enemyHealth = enemyHealthScript.enemyHealthMax;
                enemyHealthScript.healthBar.value = enemyHealthScript.CalculateHealth();

                return enemiesEnhancedPooled[i];
            }
        }
        return null;
    }
}
