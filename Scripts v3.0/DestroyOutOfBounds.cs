using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private const float zTopBound = 30.0f; // after the portal location
    private const float zBottomBound = 10.0f; // at the exit block
    private const float xBound = 5.0f; // left or right road bounds

    // Update is called once per frame
    private void Update()
    {
        DestroyObject();
    }

    private void DestroyObject()
    {
        // Destroy the object if it goes past the road borders
        if (transform.localPosition.z > zTopBound ||
            transform.localPosition.z < -zBottomBound ||
            transform.localPosition.x > xBound ||
            transform.localPosition.x < -xBound)
        {
            gameObject.SetActive(false);
        }
    }
}
