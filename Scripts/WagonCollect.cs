using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WagonCollect : MonoBehaviour
{
    Animator animator;
    GameManager gameManager;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && gameManager.wagonCount < gameManager.wagon.Length)
        {
            gameManager.wagonCount++;
            StartCoroutine(Collected());
        }
    }

    IEnumerator Collected()
    {
        //Debug.Log("WagonCollect");
        animator.Play("ScoreCollected", 0, 0);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
