using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public AudioClip destroyerSound;
    public AudioClip enemyDestroyedSound;
    public AudioClip enemyWoundedSound;
    private AudioSource collisionAudio;
    private float audioVolume = 1.0f;

    private Renderer enemyRenderer;
    private GameObject destructionParticles;

    public float enemyHealth;
    public float enemyHealthMax;
    public Slider healthBar;

    private void Start()
    {
        collisionAudio = GetComponent<AudioSource>();
        enemyRenderer = GetComponent<MeshRenderer>();
        destructionParticles = transform.Find("DestructionParticles").gameObject;

        enemyHealth = enemyHealthMax;
        healthBar.value = CalculateHealth();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && GameManager.isDestructorAvailable)
        {
            activateDestroyer();
        }
    }

    private void activateDestroyer()
    {
        StartCoroutine(enemyDeactivate());

        IEnumerator enemyDeactivate()
        {
            collisionAudio.PlayOneShot(destroyerSound, audioVolume);
            GameManager.score++;
            destructionParticles.SetActive(true);

            yield return new WaitForSeconds(destroyerSound.length);

            destructionParticles.SetActive(false);
            gameObject.SetActive(false);

            GameManager.isDestructorAvailable = false; // can only use destructor once (till available again)
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            if (enemyHealth == 1.0f) // health of 1 means the enemy's health will become 0 because the weapon hits
            {
                enemyHealth--;
                healthBar.value = CalculateHealth();

                StartCoroutine(enemyDeactivate()); // NB: version of the one above

                IEnumerator enemyDeactivate()
                {
                    collisionAudio.PlayOneShot(enemyDestroyedSound, audioVolume);
                    enemyRenderer.enabled = false;
                    GameManager.score++;

                    yield return new WaitForSeconds(enemyDestroyedSound.length);

                    other.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                }
            }
            else
            {
                enemyHealth--;
                healthBar.value = CalculateHealth();

                collisionAudio.PlayOneShot(enemyWoundedSound, audioVolume);

                other.gameObject.SetActive(false);
            }
        }

        if (other.gameObject.CompareTag("Player"))
        {
            enemyHealth = 0f;

            gameObject.SetActive(false);

            GameManager.score++;
        }
    }

    public float CalculateHealth()
    {
        return enemyHealth / enemyHealthMax;
    }
}
