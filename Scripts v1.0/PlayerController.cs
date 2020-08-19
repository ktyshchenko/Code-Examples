using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;

    [SerializeField]
    private float movementSpeed = 10.0f;
    private float rotationSpeed = 0.15f;

    private float xRange = 4.0f; // left or right road bounds
    private float zTopRange = 20.0f; // front of the road bound
    private float zBottomRange = 10.0f; // back of the road bound

    private Vector3 direction;

    private Animator anim;
    private bool isMoving = false;
    private bool isOver = false;

    public GameObject weaponPrefab;
    private Vector3 offset; // to put the weapon at the level of the player's arm and not the ground
    private float attackWaitTime = 0.25f; 

    public GameObject postProcessing;

    private IEnumerator coroutine;
    private float stunTime = 1.5f;

    private AudioSource playerAudio;
    public AudioClip playerHitSound;
    public AudioClip powerUpSound;
    private float audioVolume = 1.0f;

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        MovePlayer();
        KeepInBounds();
        ThrowWeapon();
    }

    private void MovePlayer()
    {
        if (GameManager.isGameOver == false)
        {
            // The player can move in forward/backward and sideways at the same time
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            direction = new Vector3(horizontalInput, 0.0f, verticalInput);
            
            transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);

            // Animate and rotate the player only if moving
            if (direction != Vector3.zero)
            {
                isMoving = true;
                anim.SetBool("isMoving", isMoving);

                RotatePlayer(direction);
            }
            else
            {
                isMoving = false;
                anim.SetBool("isMoving", isMoving);
            }
        }
        else
        {
            // Final animation when the game is over
            isOver = true;
            anim.SetBool("isOver", isOver);
        }
    }

    private void KeepInBounds()
    {
        // Keep the player within the left or right bounds of the road
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }

        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }

        // Keep the player within the front or back bounds of the road
        if (transform.position.z < -zBottomRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zBottomRange);
        }

        if (transform.position.z > zTopRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zTopRange);
        }
    }

    private void ThrowWeapon()
    {
        offset = Vector3.up;

        if (GameManager.isGameOver == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Activate attacking animation
                anim.SetLayerWeight(1, 1.0f); // set the attacking layer's (1) weight to override moving (base) layer
                anim.SetTrigger("Attack_trig");

                // Launch a weapon by the player in the direction the player is facing after delay based on attack anim
                StartCoroutine(WeaponAppear());

                IEnumerator WeaponAppear()
                {
                    yield return new WaitForSeconds(attackWaitTime);
                    Instantiate(weaponPrefab, transform.position + offset, transform.rotation);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Stun the player upon collision with the enemy for X sec
        if (other.gameObject.CompareTag("Enemy"))
        {
            playerAudio.PlayOneShot(playerHitSound, audioVolume);
            coroutine = FreezeCoroutine(stunTime);
            StartCoroutine(coroutine);
        }

        // Collect the power-up
        if (other.gameObject.CompareTag("PowerUp"))
        {
            playerAudio.PlayOneShot(powerUpSound, audioVolume);
            Destroy(other.gameObject);

            if (other.gameObject.name.Contains("Health"))
            {
                // Restore a missing life
                GameManager.livesLeft++;
            }
            else if (other.gameObject.name.Contains("Stun"))
            {
                // Stun the troll
                coroutine = StunEnemyCoroutine(Mathf.Exp(stunTime));
                StartCoroutine(coroutine);
            }
        }
    }

    private IEnumerator FreezeCoroutine(float waitTime)
    {
        // Deactivate controls
        GetComponent<PlayerController>().enabled = false;
        postProcessing.GetComponent<PostProcessVolume>().enabled = true;

        // Deactivate animation
        isMoving = false;
        anim.SetBool("isMoving", isMoving);

        yield return new WaitForSecondsRealtime(waitTime);

        // Reactivate controls
        GetComponent<PlayerController>().enabled = true;
        postProcessing.GetComponent<PostProcessVolume>().enabled = false;
    }

    private IEnumerator StunEnemyCoroutine(float waitTime)
    {
        // Deactivate the trolls
        TrollController.isActive = false;

        yield return new WaitForSecondsRealtime(waitTime);

        // Reactivate the trolls
        TrollController.isActive = true;
    }

    private void RotatePlayer(Vector3 rotDirection)
    {
        // Rotate the player to face the walking direction
        // Modified from https://answers.unity.com/questions/803365/make-the-player-face-his-movement-direction.html
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(rotDirection),
            rotationSpeed);
    }
}
