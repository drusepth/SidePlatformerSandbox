using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pausePanel;

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pausePanel.activeSelf)
            {
                PauseGame();
            }
            else
            {
                UnpauseGame();
            }
        }
    }

    public void QuitGame()
    {
        // TODO: save game state
        
        Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        pausePanel.SetActive(true);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        pausePanel.SetActive(false);
    }
}
