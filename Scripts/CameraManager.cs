using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] Camera cam;
    [SerializeField] GameObject[] camPositions;

    [SerializeField] float camSpeed;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        if (!gameManager.gamePaused)
        {            
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, camPositions[gameManager.wagonCount].transform.position, camSpeed * Time.deltaTime); //update camera in gameplay   
            cam.GetComponent<Animator>().enabled = true;
        }

        if (gameManager.win)
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, camPositions[6].transform.position, camSpeed * Time.deltaTime);

            while (cam.transform.rotation.x > 0)
            { 
                cam.transform.Rotate(new Vector3(-1, 0, 0) * Time.deltaTime);
            }
        }
    }
}
