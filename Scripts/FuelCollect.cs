using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelCollect : MonoBehaviour
{
    Animator animator;
    Animator fuelBarAnimator;
    GameManager gameManager;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        fuelBarAnimator = gameManager.GetComponent<UIManager>().fuelBar.GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            fuelBarAnimator.Play("FuelBar", 0, 0);
            gameManager.FillFuel();
            StartCoroutine(Collected());
        }
    }

    IEnumerator Collected() //CHANGE IT!
    {
        //Debug.Log("FuelCollect");
        animator.Play("ScoreCollected", 0, 0);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
