using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorCirculo : MonoBehaviour
{

    Rigidbody2D miRb = null;
    [SerializeField] float velocidadMovimiento = 0f;
    [SerializeField] float tiempoCambioVelocidad = 0f;
    [SerializeField] float velocidadEncogimiento = 0f;
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject implosion;
    bool cambiarVelocidadAhora = true;

    void Start()
    {
        miRb = GetComponent<Rigidbody2D>();    
    }

    void Update()
    {
        if(cambiarVelocidadAhora)
        {
            cambiarVelocidadAhora = false;
            StartCoroutine(CambioDireccion(tiempoCambioVelocidad));
        }
        if(!ControladorJuego.juegoTerminado)
        {
            CondicionesDerrota();
        }
        
    }

    void MovimientoAleatorio()
    {
        Vector2 randomVector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        randomVector.Normalize();
        miRb.velocity = randomVector * velocidadMovimiento;

    }

    void ReducirConTiempo()
    {
        transform.localScale -= new Vector3(velocidadEncogimiento, velocidadEncogimiento, 0f) * Time.deltaTime;
    }

    void CondicionesDerrota()
    {
        if(transform.localScale.x > 0f)
        {
            ReducirConTiempo();
        }
        if(transform.localScale.x > 1.0f)
        {
            Instantiate(explosion, transform.position, Quaternion.identity, null);
            Destroy(gameObject);
            FindObjectOfType<AudioManager>().Play("Explosion");
        }
        else if(transform.localScale.x <= 0f)
        {
            Instantiate(implosion, transform.position, Quaternion.identity, null);
            Destroy(gameObject);
            FindObjectOfType<AudioManager>().Play("Implosion");
        }
        
    }

    IEnumerator CambioDireccion(float tiempoCambioVelocidad)
    {
        MovimientoAleatorio();  
        yield return new WaitForSeconds(tiempoCambioVelocidad);
        cambiarVelocidadAhora = true;
    }
}
