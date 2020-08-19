using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private float zTopBound = 30.0f; // after the portal location
    private float zBottomBound = 10.0f; // at the exit block
    private float xBound = 5.0f; // left or right road bounds

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        DestroyObject();
    }

    private void DestroyObject()
    {
        // Destroy the object if it goes past the road borders
        if (transform.position.z > zTopBound ||
            transform.position.z < -zBottomBound ||
            transform.position.x > xBound ||
            transform.position.x < -xBound)
        {
            Destroy(gameObject);
        }
    }
}
