using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerController playerController;

    [SerializeField] GameObject littleSmoke, bigSmoke; //engine smoke increases at phase two
    public GameObject[] wagon; //Other scripts needs to access it for collecting wagons
    public int wagonCount; //Other scripts needs to access it.
    public int wagonType; //0 = Empty, 1 = Cargo, 2 = Passenger //Other scripts needs to access it.
    
    Coroutine drainFuel;
    bool fuelDrain; // is it draining or not? used for start a coroutine only once in Update() function
    [SerializeField] int fillAmount; //fuel canister
    [SerializeField] int lossAmount; //obstacle
    [SerializeField] float fuelDrainRate; //lower for faster fuel drain
    public int fuel;

    [HideInInspector] public bool scoreCollected; //Destroy or leave the collectable
    [HideInInspector] public int score; //UIManager.cs needs to access as well

    public bool gamePaused; //is game playable or not?
    [HideInInspector] public bool win, lose; //UI
    [HideInInspector] public bool phaseTwo; //which phase are we on? used for animations

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        SpawnWagon(wagonCount);
        ChangeWagonType(wagonType, wagonCount);

        if(!fuelDrain)
            drainFuel = StartCoroutine(DrainFuel()); //Stop it before train stops when game is over.

        if (fuel <= 0) //Lose condition (out of fuel)
            Lose();   
    }

    private void SpawnWagon(int count)
    {
        for (int i = 0; i < count; i++)  //enable 
        {
            wagon[i].transform.GetChild(0).gameObject.SetActive(true);
        }
        for (int i = count; i < wagon.Length; i++) //disable (maybe destroy wagons via obstacles in future development)
        {
            wagon[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void ChangeWagonType(int type, int count) //type: 0 = Empty, 1 = Cargo, 2 = Passenger
    {
        switch (type)
        {
            case 0:
                for (int i = 0; i < count; i++)
                {
                    wagon[i].transform.GetChild(0).gameObject.SetActive(true);
                    wagon[i].transform.GetChild(1).gameObject.SetActive(false);
                    wagon[i].transform.GetChild(2).gameObject.SetActive(false);
                }
                break;
            case 1:
                for (int i = 0; i < count; i++)
                {
                    wagon[i].transform.GetChild(0).gameObject.SetActive(false);
                    wagon[i].transform.GetChild(1).gameObject.SetActive(true);
                    wagon[i].transform.GetChild(2).gameObject.SetActive(false);
                }
                break;
            case 2:
                for (int i = 0; i < count; i++)
                {
                    wagon[i].transform.GetChild(0).gameObject.SetActive(false);
                    wagon[i].transform.GetChild(1).gameObject.SetActive(false);
                    wagon[i].transform.GetChild(2).gameObject.SetActive(true);
                }
                break;
        }
    }

    public void CollectScore()
    {
        if (score < wagonCount * 5)
        {
            int scoreMod = score % 5;
            wagon[score/5].transform.GetChild(wagonType).transform.GetChild(scoreMod).gameObject.SetActive(true); //filling wagons visually with score collectables in right order
            score++;
            scoreCollected = true;
            //Debug.Log("score/5: " + score/5);
            //Debug.Log("scoreMod: " + scoreMod);
        }
    }

    public void FillFuel()
    {
        if (fuel <= 100 - fillAmount)
            fuel += fillAmount;
        else
            fuel = 100;
    }

    public void FuelLoss()
    {
        if (0 < fuel - lossAmount)
            fuel -= lossAmount; 
        else
            fuel = 0;
    }

    public void PhaseTwo()
    {
        phaseTwo = true;
        playerController.speed += playerController.speed / 2;
        fuelDrainRate = fuelDrainRate / 5;
        littleSmoke.SetActive(false);
        bigSmoke.SetActive(true);
        playerController.camAnimator.Play("CamShakePhaseTwo", 0, 0);
    }

    public void Win()
    {
        if (!gamePaused)
        {
            StartCoroutine(StopTrain());
            win = true;
        }
    }

    public void Lose()
    {
        if (!gamePaused)
        {
            StartCoroutine(StopTrain());
            lose = true;
        }
    }

    private IEnumerator DrainFuel()
    {       
        while (fuel > 0 && !gamePaused)
        {
            fuelDrain = true;
            fuel -= 1;
            yield return new WaitForSeconds(fuelDrainRate);
        }
        fuelDrain = false;
    }

    private IEnumerator StopTrain()
    {
        if (drainFuel != null)
        {
            StopCoroutine(drainFuel);
            fuelDrain = false;
        }

        playerController.turnDirection = 0;

        while (playerController.speed > 0)
        {
            playerController.speed -= 1f;
            yield return new WaitForSeconds(0.1f);
        }
        gamePaused = true;
        playerController.camAnimator.enabled = false;
    }
}
