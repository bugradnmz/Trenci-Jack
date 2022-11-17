using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    PlayerController playerController;    
    GameManager gameManager;
    [SerializeField] TextMeshProUGUI directionText;
    [SerializeField] TextMeshProUGUI fuelText;
    [SerializeField] TextMeshProUGUI scoreTextHUD;
    [SerializeField] TextMeshProUGUI scoreTextMenu;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject loseMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject hud;
    [SerializeField] GameObject winVFX;
    public Slider fuelBar;
    public Image fill;
    [SerializeField] Image railwaySwitch;
    [SerializeField] Sprite switchLeft;
    [SerializeField] Sprite switchStraight;
    [SerializeField] Sprite switchRight;

    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        if (gameManager.gamePaused) //HUD ON-OFF
            hud.SetActive(false);
        else
            hud.SetActive(true);

        if (gameManager.win && !gameManager.lose) //WIN-LOSE
        {
            StartCoroutine(Win());
        }
        else if (gameManager.lose && !gameManager.win)
        {
            StartCoroutine(Lose());            
        }

        //HUD CODES
        switch (playerController.turnDirection)  //Junction Switch(Turn) indicator
        {
            case 1:
                railwaySwitch.sprite = switchRight;
                directionText.text = "RIGHT";
                break;
            case -1:
                railwaySwitch.sprite = switchLeft;
                directionText.text = "LEFT";
                break;
            case 0:
                railwaySwitch.sprite = switchStraight;
                directionText.text = "STRAIGHT";
                break;
        }
        FuelHUD();
        scoreTextHUD.text = "SCORE: " + gameManager.score.ToString(); //Score HUD
        scoreTextMenu.text = "SCORE: " + gameManager.score.ToString(); //Score MENU
    }

    public void Play()
    {
        playButton.SetActive(false);
        StartCoroutine(DelayStart());
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void FuelHUD()
    {
        fuelBar.value = gameManager.fuel;
        fuelText.text = gameManager.fuel.ToString(); //Number
    }

    private IEnumerator Win()
    {
        yield return new WaitForSeconds(2f);
        winMenu.SetActive(true);
        winVFX.SetActive(true);
    }

    private IEnumerator Lose()
    {
        yield return new WaitForSeconds(1f);
        loseMenu.SetActive(true);
    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(0.1f);
        gameManager.gamePaused = false;
    }
}
