using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{    
    Touch touch;
    Vector2 startPosition, direction; //first touch position, direction of finger movement

    [SerializeField] GameObject trail;  //used for switch rail of wagons only at junction
    [SerializeField] GameObject engine;
    [SerializeField] GameObject cam;
    [HideInInspector] public Animator camAnimator; //gameManager.cs uses for phase two
    Animator engineAnimator;    
    GameManager gameManager;

    public float speed;  //also used by RotateWheels.cs and WagonController.cs
    public float switchSpeed; //also used by WagonController.cs
    [HideInInspector] public float turnDirection; //used for rail switch triggers UIManager.cs needs to access it.

    void Start()
    {
        engineAnimator = engine.GetComponent<Animator>();
        camAnimator = cam.GetComponent<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        if (!gameManager.gamePaused)
        {            
            transform.position += Vector3.forward * speed * Time.deltaTime; //Choo Choo!

            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        startPosition = touch.position;
                        break;

                    case TouchPhase.Ended:
                        direction = touch.position - startPosition;
                        direction = Vector3.Normalize(direction);     //Turning pixel difference to normalized vector value
                        if (direction.x > 0 && turnDirection < 1)
                            turnDirection += 1;
                        else if (direction.x < 0 && turnDirection > -1)
                            turnDirection -= 1;
                        break;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Right" && turnDirection == 1)
        {
            Instantiate(trail, new Vector3(transform.position.x, transform.position.y, transform.position.z + 2), Quaternion.identity);
            StartCoroutine(SwitchRight());
        }
        else if (other.gameObject.tag == "Left" && turnDirection == -1)
        {
            Instantiate(trail, new Vector3(transform.position.x,transform.position.y,transform.position.z + 2), Quaternion.identity);
            StartCoroutine(SwitchLeft());
        }
        else if(other.gameObject.tag == "ForcedSwitch") //alignment for phase 2
        {
            Instantiate(trail, new Vector3(transform.position.x, transform.position.y, transform.position.z + 2), Quaternion.identity);
            if(transform.position.x < 0)
                StartCoroutine(SwitchRight());
            else
                StartCoroutine(SwitchLeft());
        }
        else if (other.gameObject.tag == "Fork") //random decision at start of phase 2. If player doesn't decide
        {
            Instantiate(trail, new Vector3(transform.position.x, transform.position.y, transform.position.z + 2), Quaternion.identity);
            if (turnDirection == 0)
            {
                bool random = (Random.value > 0.5f);
                if (random)
                    StartCoroutine(SwitchRight());
                else
                    StartCoroutine(SwitchLeft());
            }
            else if (turnDirection == 1)
            {
                StartCoroutine(SwitchRight());

            }
            else if (turnDirection == -1)
            {
                StartCoroutine(SwitchLeft());
            }
        }

        if(other.gameObject.tag == "CargoPhase") //Phase 2 collecting cargo
        {
            gameManager.wagonType = 1;
            gameManager.PhaseTwo();
            //Debug.Log("CargoPhase");
        }
        else if (other.gameObject.tag == "PassengerPhase") //Phase 2 collecting passenger
        {
            gameManager.wagonType = 2;
            gameManager.PhaseTwo();
            //Debug.Log("PassengerPhase");
        }

        if (other.gameObject.tag == "Finish")
        {
            gameManager.Win();
        }
    }

    private IEnumerator SwitchRight()
    {
        turnDirection = 0;
        engineAnimator.Play("SwitchRight", 0, 0);
        camAnimator.Play("CamShake", 0, 0);
        float targetPosX = transform.position.x + 5.1f;
        while (transform.position.x != targetPosX)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosX, transform.position.y, transform.position.z), switchSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        if(gameManager.phaseTwo) //constant camera shake effect at phase two was getting canceled when train switch the rail
            camAnimator.Play("CamShakePhaseTwo", 0, 0);
    }

    private IEnumerator SwitchLeft()
    {
        turnDirection = 0;
        engineAnimator.Play("SwitchLeft", 0, 0);
        camAnimator.Play("CamShake", 0, 0);
        float targetPosX = transform.position.x - 5.1f;
        while (transform.position.x != targetPosX)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosX, transform.position.y, transform.position.z), switchSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        if (gameManager.phaseTwo)
            camAnimator.Play("CamShakePhaseTwo", 0, 0);
    }
}
