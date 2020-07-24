using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;  // bool to see if game is paused
    public GameObject pauseMenuUI;
    public GameObject HUD;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("PS4_OPTIONS"))
        {
            if(gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    
    public void Resume ()
    {
        pauseMenuUI.SetActive(false);
        HUD.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        gamePaused = false;
    }

    void Pause ()
    {
        pauseMenuUI.SetActive(true);
        HUD.SetActive(false);
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log(h);
        EventSystem es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        es.SetSelectedGameObject(null);
        es.SetSelectedGameObject(es.firstSelectedGameObject);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void LoadMenu()
    {
        Debug.Log("loading menu");
    }
    public void QuitGame()
    {
        Debug.Log("quitting game...");
        //QuitGame();
    }
}
