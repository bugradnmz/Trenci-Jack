using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWheels : MonoBehaviour
{
    float playerSpeed;

    void Start()
    {
        playerSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().speed;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * playerSpeed * 50 * Time.deltaTime);
    }
}
