using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowForward : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 15.0f;

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }
}
