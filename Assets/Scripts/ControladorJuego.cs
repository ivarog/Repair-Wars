using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControladorJuego : MonoBehaviour
{
    [SerializeField] GameObject circuloJugador1;
    [SerializeField] GameObject circuloJugador2;
    [SerializeField] GameObject paredSuperior;
    [SerializeField] GameObject paredInferior;
    [SerializeField] GameObject paredDerecha;
    [SerializeField] GameObject paredIzquierda;
    [SerializeField] GameObject bolaJugador1;
    [SerializeField] GameObject bolaJugador2;
    [SerializeField] float tiempoTurno = 0f;
    [SerializeField] float tiempoJuego = 0f;
    [SerializeField] Text relojJuego;
    [SerializeField] GameObject ganador;
    SpriteRenderer spriteCirculoJugador1;
    TrailRenderer trailCirculoJugador1;
    SpriteRenderer spriteCirculoJugador2;
    TrailRenderer trailCirculoJugador2;
    ControladorLanzamiento controlador1;
    ControladorLanzamiento controlador2;

    public static bool juegoTerminado = false;

    bool iniciarMusica = true;

    [SerializeField] Sprite[] arrayFondos = null;
    [SerializeField] Sprite[] arrayPersonajes = null;
    [SerializeField] GameObject fondo;
    [SerializeField] SpriteRenderer spriteJugador1;
    [SerializeField] SpriteRenderer spriteJugador2;

    void Start()
    {
        
        Physics2D.IgnoreCollision(circuloJugador1.GetComponent<Collider2D>(), circuloJugador2.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(bolaJugador1.GetComponent<Collider2D>(), bolaJugador2.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(bolaJugador1.GetComponent<Collider2D>(), paredSuperior.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(bolaJugador1.GetComponent<Collider2D>(), paredInferior.GetComponent<Collider2D>());
        // Physics2D.IgnoreCollision(bolaJugador1.GetComponent<Collider2D>(), paredDerecha.GetComponent<Collider2D>());
        // Physics2D.IgnoreCollision(bolaJugador1.GetComponent<Collider2D>(), paredIzquierda.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(bolaJugador2.GetComponent<Collider2D>(), paredSuperior.GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(bolaJugador2.GetComponent<Collider2D>(), paredInferior.GetComponent<Collider2D>());
        // Physics2D.IgnoreCollision(bolaJugador2.GetComponent<Collider2D>(), paredIzquierda.GetComponent<Collider2D>());
        // Physics2D.IgnoreCollision(bolaJugador2.GetComponent<Collider2D>(), paredDerecha.GetComponent<Collider2D>());
        spriteCirculoJugador1 = circuloJugador1.GetComponent<SpriteRenderer>();
        trailCirculoJugador1 = circuloJugador1.GetComponent<TrailRenderer>();
        spriteCirculoJugador2 = circuloJugador2.GetComponent<SpriteRenderer>();
        trailCirculoJugador2 = circuloJugador2.GetComponent<TrailRenderer>();

        controlador1 = bolaJugador1.GetComponent<ControladorLanzamiento>();
        controlador2 = bolaJugador2.GetComponent<ControladorLanzamiento>();
        
        juegoTerminado = false;
        fondo.GetComponent<SpriteRenderer>().sprite = arrayFondos[Random.Range(0, 3)];

        int randomSprite1 = Random.Range(0, 5);
        int randomSprite2 = Random.Range(0, 5);
        while(randomSprite1 == randomSprite2)
        {
            randomSprite2 = Random.Range(0, 5);
        }

        spriteJugador1.sprite = arrayPersonajes[randomSprite1];
        spriteJugador2.sprite = arrayPersonajes[randomSprite2];
    }

    void Update()
    {

        if(iniciarMusica)
        {
            iniciarMusica = false;
            FindObjectOfType<AudioManager>().Stop("Menu");
            FindObjectOfType<AudioManager>().Play("Punchis");
        }

        if(circuloJugador1 != null && circuloJugador2 != null)
        {
            OrdenDependiendoTamano();
            ModificarCaminoTamano(circuloJugador1, trailCirculoJugador1);
            ModificarCaminoTamano(circuloJugador2, trailCirculoJugador2);
        }
        
        if(!juegoTerminado)
        {
            ReducirTiempoJuego();
            RevisarExistenciaCirculos();
        }
        
    }

    void OrdenDependiendoTamano()
    {
        
        if(circuloJugador1.transform.localScale.x > circuloJugador2.transform.localScale.x)
        {
            spriteCirculoJugador2.sortingOrder = 20;
            trailCirculoJugador2.sortingOrder = 19;
            spriteCirculoJugador1.sortingOrder = 10;
            trailCirculoJugador1.sortingOrder = 9;
        }
        else
        {
            spriteCirculoJugador1.sortingOrder = 20;
            trailCirculoJugador1.sortingOrder = 19;
            spriteCirculoJugador2.sortingOrder = 10;
            trailCirculoJugador2.sortingOrder = 9;
        }
    
    }

    void ModificarCaminoTamano(GameObject circulo, TrailRenderer trailCirculo)
    {
        float tamanoActual = circulo.transform.localScale.x;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0.0f, tamanoActual);
        curve.AddKey(1.2f, 0.0f);
        trailCirculo.widthCurve = curve;
    }

    void ReducirTiempoJuego()
    {
        tiempoJuego -= Time.deltaTime;
        relojJuego.text = ((int)tiempoJuego).ToString();
        if(tiempoJuego <= 0f)
        {
            string ganadorTexto = "" ;
            if(circuloJugador1.transform.localScale.x > circuloJugador2.transform.localScale.x)
            {
                ganadorTexto = "Gana Jugador " + spriteJugador1.sprite.name.ToString();
            }
            else
            {
                ganadorTexto = "Gana Azul " + spriteJugador2.sprite.name.ToString();
            }
            juegoTerminado = true;
            StartCoroutine(MostrarPanelGanador(ganadorTexto));
        }
    }

    void RevisarExistenciaCirculos()
    {
        if(circuloJugador1 == null)
        {
            juegoTerminado = true;
            StartCoroutine(MostrarPanelGanador("Gana Jugador " + spriteJugador2.sprite.name));
        }
        else if(circuloJugador2 == null)
        {
            juegoTerminado = true;
            StartCoroutine(MostrarPanelGanador("Gana Jugador " + spriteJugador1.sprite.name));
        }
    }

    IEnumerator MostrarPanelGanador(string ganadorTexto)
    {
        yield return new WaitForSeconds(5f);
        ganador.SetActive(true);
        ganador.GetComponentInChildren<Text>().text = ganadorTexto;
    } 
}
