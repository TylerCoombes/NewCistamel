using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject collectiblesMenu;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape) && Time.timeScale > 0)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            Debug.Log("EscapePressed");
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void Options()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Back()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void Collectibles()
    {
        pauseMenu.SetActive(false);
        collectiblesMenu.SetActive(true);
    }
    public void Back2()
    {
        pauseMenu.SetActive(true);
        collectiblesMenu.SetActive(false);
    }
}
