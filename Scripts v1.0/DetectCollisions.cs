using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisions : MonoBehaviour
{
    public AudioClip enemyDestroyedSound;
    private AudioSource collisionAudio;
    private float audioVolume = 1.0f;

    private Renderer enemyRenderer;

    private void Start()
    {
        collisionAudio = GetComponent<AudioSource>();
        enemyRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            collisionAudio.PlayOneShot(enemyDestroyedSound, audioVolume);
            enemyRenderer.enabled = false;
            GameManager.score++;

            Destroy(other.gameObject);
            Destroy(gameObject, enemyDestroyedSound.length);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            GameManager.score++;
        }
    }
}
    