using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollController : MonoBehaviour
{
    public static bool isActive = true;

    public static float movementSpeed;

    private int animMovingInt = 1;
    private int animCheeringInt = 6;
    //private int animJumpingInt = 7;
    private int animHittingInt = 5;

    private Animator anim;

    private float zBottomBound = 10.0f; // at the exit block
    private float offset = 0.7f;

    private AudioSource trollAudio;
    public AudioClip trollGrowlSound;
    public AudioClip trollAttackSound;
    private float audioVolume = 0.2f;
    private float audioVolumeFull = 1.0f;

    private float startDelay = 0.5f;
    private float growlInt = 5.0f;

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        trollAudio = GetComponent<AudioSource>();

        InvokeRepeating("Growling", startDelay, growlInt);
    }

    // Update is called once per frame
    private void Update()
    {
        MoveTroll();
        DamageTown();
    }

    private void MoveTroll()
    {
        if (GameManager.isGameOver == false && isActive == true)
        {
            // Move the troll forward with its animation
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
            anim.SetInteger("moving", animMovingInt);
        }
        else if (GameManager.isGameOver == false && isActive == false)
        {
            // Stun the troll
            anim.SetInteger("moving", 0);
        }
        else if (GameManager.isGameOver == true)
        {
            anim.SetInteger("moving", animCheeringInt);
        }
    }

    private void DamageTown()
    {
        if (GameManager.isGameOver == false)
        {
            // If the troll gets to the exit boundary, hit it
            if (transform.position.z <= -zBottomBound + offset)
            {
                anim.SetInteger("moving", animHittingInt);
                trollAudio.PlayOneShot(trollAttackSound, audioVolumeFull);
            }

            // If the troll gets past the exit boundary, subtract a life
            if (transform.position.z <= -zBottomBound)
            {
                GameManager.livesLeft--;
            }
        }
    }

    private void Growling()
    {
        trollAudio.PlayOneShot(trollGrowlSound, audioVolume);
    }
}
