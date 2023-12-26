using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Done_Rotator : MonoBehaviour
{
    public float rotationSpeed = 30f; // Adjust this value to control the rotation speed

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial X rotation value to 6
        transform.rotation = Quaternion.Euler(1f, 0f, 0f);
    }

    void Update()
    {
        // Rotate the ship around the Y-axis continuously
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
