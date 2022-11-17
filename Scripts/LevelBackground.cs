using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBackground : MonoBehaviour
{
    GameManager gameManager;
    PlayerController playerController;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!gameManager.gamePaused)
            transform.position += Vector3.forward * playerController.speed * Time.deltaTime;
    }
}
