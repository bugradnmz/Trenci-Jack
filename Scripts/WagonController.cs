using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonController : MonoBehaviour
{
    GameManager gameManager;
    Animator animator;
    GameObject player;

    float speed;
    float switchSpeed;
    float targetPosX;    

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");        
        switchSpeed = player.GetComponent<PlayerController>().switchSpeed;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!gameManager.gamePaused)
        {
            speed = player.GetComponent<PlayerController>().speed; //needs to be updated for stop whole train at GameManager.cs
            transform.position += Vector3.forward * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Trail")
            StartCoroutine(Switch());
    }

    private IEnumerator Switch()
    {
        if (player.transform.position.x > transform.position.x)
        {
            targetPosX = transform.position.x + 5.1f;
            animator.Play("SwitchRight", 0, 0);
        }
        else if (player.transform.position.x < transform.position.x)
        {
            targetPosX = transform.position.x - 5.1f;
            animator.Play("SwitchLeft", 0, 0);
        }

        while (transform.position.x != targetPosX)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosX, transform.position.y, transform.position.z), switchSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
