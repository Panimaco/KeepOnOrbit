using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonActions : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Error: El nombre de la escena no puede estar vacío.");
            return;
        }

        Scene scene = SceneManager.GetSceneByName(sceneName);

        //Si la escena ya está cargada, la descargamos y la volvemos a cargar

        if (scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
        SceneManager.LoadScene(sceneName);

        if(Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            Debug.Log("Juego Pausado");
        }
        else
        {
            Time.timeScale = 1;
            Debug.Log("Juego Reanudado");
        }
    }
}