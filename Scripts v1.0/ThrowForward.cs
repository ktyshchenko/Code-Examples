using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowForward : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 15.0f;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }
}
