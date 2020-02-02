using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControladorLanzamiento : MonoBehaviour
{

    //Calculate distance 
    private float validDistance;

    //Shoting direction
    private Vector3 shootDirection;

    //Max distance of shot
    [SerializeField] private float maxDistance = 2.0f;

    //Shooting power variables
    [SerializeField] private float power = 1.0f;
    private float shootPower;

    //Pivot and arrow objects
    [SerializeField] GameObject arrow = null;

    [SerializeField] float reparacion = 0.0f;

    //Mouse variables
    Vector3 tempMousePosition;
    private float distanceMouseBall;
    private Vector3 directionShootingVector;

    //Direction and power of shot
    private Vector3 push;

    Vector3 initalPosition = Vector3.zero;

    Rigidbody2D myRb;
    Collider2D myCd;

    [SerializeField] float limiteX;
    [SerializeField] float limiteY;

    private int fingerID = 0;
    bool pointerDown = true;

    [SerializeField] Text prueba;


    //////////////////////////////////////
    bool objetoTocado = false;
    int miDedoId;
    int indiceAMover = -1;


    private void Awake()
    {
        myRb = GetComponent<Rigidbody2D>();
        myCd = GetComponent<Collider2D>();
        initalPosition = transform.position;
    }

    private void Update()
    {
        ChecarLimites();
        if(!ControladorJuego.juegoTerminado)
        {
            ControlMultiTouch();
        }

    }

    private void HandleMousePosition()
    {
        tempMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(indiceAMover).position.x, Input.GetTouch(indiceAMover).position.y, 10.0f));
        distanceMouseBall = Vector3.Distance(tempMousePosition, transform.position);
    }

    private void HandleArrow()
    {
        directionShootingVector = (tempMousePosition - transform.position).normalized;


        if(distanceMouseBall > maxDistance)
        {
            validDistance = maxDistance;
            //Clamp arrow
            arrow.transform.position = transform.position + directionShootingVector * validDistance * -1;
        }
        else
        {
            validDistance = distanceMouseBall;        
            //Arrow in the oppositive way
            arrow.transform.position = transform.position + directionShootingVector * validDistance * -1;
        }


        RotationsInShot();
    }

    private void PowerAndShotDirection()
    {
        //Calculate power
        shootPower = Mathf.Abs(validDistance) * power;

        //Calculate direction
        shootDirection = Vector3.Normalize(arrow.transform.position - transform.position);
    }

    public void RotationsInShot()
    {
        //Angle between  directionShootingVector and X axis
        float angle = Mathf.Atan2(directionShootingVector.y, directionShootingVector.x) * Mathf.Rad2Deg + 90.0f;
        //Rotation around z axis
        Quaternion dirObject = Quaternion.AngleAxis(angle, Vector3.forward);
        arrow.transform.rotation = dirObject;
    }

     private void TouchDrag()
    {
        // if(esMiturno)
        // {
        HandleMousePosition();
        HandleArrow();
        PowerAndShotDirection();
        // }

    }

    //If t mouseup and was my turn then i can shoot the player
    private void TouchUp()
    {
        // if(esMiturno)
        // {
            arrow.transform.localPosition = Vector3.zero;
            push = shootDirection * shootPower;
            myRb.AddForce(push, ForceMode2D.Impulse);
        // }
    }


    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag == "Circulo")
        {
            other.transform.localScale += new Vector3(reparacion, reparacion, 0f);
            FindObjectOfType<AudioManager>().Play("Absorber");
            transform.position = initalPosition;
            myRb.velocity = Vector3.zero;
            myRb.angularVelocity = 0f;
        }
    }

    void ChecarLimites()
    {
        if(transform.position.x > limiteX || transform.position.x < -limiteX || transform.position.y > limiteY || transform.position.y < -limiteY)
        {
            transform.position = initalPosition;
            myRb.velocity = Vector3.zero;
        }
    }

    void ControlMultiTouch()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) 
        {
            if (Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position)) == myCd) 
            {
                miDedoId = 1;
                objetoTocado = true;
            }
        }
        else if (Input.touchCount == 2 && Input.GetTouch(1).phase == TouchPhase.Began) 
        {
            if (Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position)) == myCd) 
            {
                miDedoId = 2;
                objetoTocado = true;
            }
        }

        if(Input.touchCount > 0)
        {
            if(miDedoId == 2 && Input.touchCount == 2 && Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                indiceAMover = 1;
                TouchDrag();
                // Debug.Log("Entre if 1");
            }
            else if(miDedoId == 1 && Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                indiceAMover = 0;
                TouchDrag();
                // Debug.Log("Entre if 2");
            }
            else if(miDedoId == 1 && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Debug.Log(gameObject.name + " " + "Mi id " + miDedoId);
                indiceAMover = 0;
                TouchDrag();
                // Debug.Log("Entre if 3");
            }
            else if(miDedoId == 2 && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                indiceAMover = 0;
                TouchDrag();
                Debug.Log("Entre if 4");
            }

            // Debug.Log(gameObject.name+ " " + indiceAMover + "Mi id" + miDedoId);
            // Debug.Log(Input.GetTouch(0).phase);
        }

        if(Input.touchCount > 0)
        {

            if(miDedoId == 1 && Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                objetoTocado = false;
                TouchUp();
                miDedoId = 0;
            }
            else if(miDedoId == 1 && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                objetoTocado = false;
                TouchUp();
                miDedoId = 0;
            }
            else if(miDedoId == 2 && Input.touchCount == 2 && Input.GetTouch(1).phase == TouchPhase.Ended)
            {
                objetoTocado = false;
                TouchUp();
                miDedoId = 0;
            }
            else if(miDedoId == 2 && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                objetoTocado = false;
                TouchUp();
                miDedoId = 0;
            }
        }

        if(Input.touchCount == 0)
        {
            miDedoId = 0;
            indiceAMover = 0;
        }
    }
}
