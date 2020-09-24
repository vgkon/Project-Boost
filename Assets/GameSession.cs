using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    private bool paused = false;
    [SerializeField] Canvas escMenu;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (paused)
            {
                Time.timeScale = 1;
                escMenu.gameObject.SetActive(false);
                paused = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Time.timeScale = 0;
                escMenu.gameObject.SetActive(true);
                paused = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public bool GetPaused()
    {
        return paused;
    }

    public void LoadFirstScene()
    {
        paused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);

    }

    public void Exit()
    {
        Application.Quit();
    }
}
