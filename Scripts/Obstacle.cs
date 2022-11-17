using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Obstacle : MonoBehaviour
{
    Animator camAnimator;
    GameManager gameManager;
    Animator fuelBarAnimator;
    Image fill;
    Color red = new Color32(183, 19, 0, 255);
    Color green = new Color32(19, 183, 0, 255);

    void Start()
    {
        camAnimator = Camera.main.GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        fuelBarAnimator = gameManager.GetComponent<UIManager>().fuelBar.GetComponent<Animator>();
        fill = gameManager.GetComponent<UIManager>().fill;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(ChangeFillColor());
            gameManager.FuelLoss();
            camAnimator.Play("CamShake", 0, 0);
            StartCoroutine(Explode());
        }
    }

    private IEnumerator ChangeFillColor()
    {
        fuelBarAnimator.Play("FuelBar", 0, 0);
        fill.color = red;
        yield return new WaitForSeconds(0.5f);
        fill.color = green;
    }

    private IEnumerator Explode()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
