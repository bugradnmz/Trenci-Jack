using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoCollect : MonoBehaviour
{
    Animator animator;
    GameManager gameManager;
    [SerializeField] GameObject vfx;

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
        if (other.gameObject.tag == "Player" && gameManager.wagonType == 1)
        {
            gameManager.CollectScore();
            
            if(gameManager.scoreCollected)
            { 
                StartCoroutine(Collected());
                gameManager.scoreCollected = false;
            }
        }
    }

    IEnumerator Collected()
    {
        //Debug.Log("CargoCollect");
        Instantiate(vfx, transform.position, Quaternion.identity);
        animator.Play("ScoreCollected", 0, 0);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
