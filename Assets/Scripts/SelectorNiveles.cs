using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectorNiveles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SiguientePagina()
    {
        SceneManager.LoadScene("SelectorNiveles2");
    }

    public void NivelTutorial()
    {
        SceneManager.LoadScene("Nivel1");
    }

    public void Salir()
    {
        Scene.Manager.LoadScene("MenuPrincipal")
    }
}
