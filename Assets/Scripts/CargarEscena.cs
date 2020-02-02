using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CargarEscena : MonoBehaviour
{
    private void Start() 
    {
        if(SceneManager.GetActiveScene().name == "Inicio")
        {
            StartCoroutine(CargarOpciones());
            FindObjectOfType<AudioManager>().Play("Inicio");
        }    

        if(SceneManager.GetActiveScene().name == "Opciones")
        {
            FindObjectOfType<AudioManager>().Play("Menu");
        }    
    }

    private void Update() 
    {
        if(SceneManager.GetActiveScene().name == "Creditos")
        {
            if(Input.touchCount > 0)
            {
                SceneManager.LoadScene("Opciones");
            }
        }
    }

    IEnumerator CargarOpciones()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Opciones");
    }

    public void CargarMenuOpciones()
    {
        FindObjectOfType<AudioManager>().Stop("Punchis");
        FindObjectOfType<AudioManager>().Play("Menu");
        SceneManager.LoadScene("Opciones");
    }

    public void CerrarJuego()
    {
        Application.Quit();
    }

    public void CargarJuego()
    {
        SceneManager.LoadScene("Main");
        FindObjectOfType<AudioManager>().Play("Jugar");
    }

    public void CargarCreditos()
    {
        SceneManager.LoadScene("Creditos");
        FindObjectOfType<AudioManager>().Play("Instrucciones");
    }

    public void CargarInstrucciones()
    {
        SceneManager.LoadScene("Instrucciones");
        FindObjectOfType<AudioManager>().Play("Instrucciones");
    }

}
