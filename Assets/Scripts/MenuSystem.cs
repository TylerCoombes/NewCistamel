using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public int playIndex; // Index for scene to load with play button 
    public void LoadLevel()
    {
        SceneManager.LoadScene(playIndex); // Loads desired scene from index
    }

    public void QuitGame()
    {
        Application.Quit(); // Quits the game 
    }
}
