using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;

    // Start is called before the first frame update
    private void Start()
    {
        // Set the offset between camera and player
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        // Change camera's position based on where the player is
        transform.position = player.transform.position + offset;
    }
}
