using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject Buildings;


    /// <summary>
    /// On this start function I get access to the scene names so I can tell the objects whether they should be enabled or not
    /// this allowed me to only use 1 script instead of 2
    /// 
    /// Below the if statement is code for the resolution settings, I gain access to the resolution settings, I clear the list so
    /// its empty when we begin the level
    /// 
    /// I then loop through each element in the list of options to get access to all the resolution options
    /// </summary>
    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(false);
            optionsMenu.SetActive(false);
        }

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions(); //clear out all the options in the drop down

        List<string> options = new List<string>(); // create a list of string which are our options


        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) //loop through each element in our array
        {
            string option = resolutions[i].width + " x" + resolutions[i].height; //create a nice formated string which displays the resolution
            options.Add(option); //add it to the list

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) //gets the current resolution
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options); //add the list to the drop down
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }


    /// <summary>
    /// This is a function which allows for control of the volume slider, it calls a hidden paramater called "MusicVol"
    /// </summary>
    /// <param name="sliderValue"></param>
    public void SetLevel(float sliderValue)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20); //logarithimic scale
    }

    /// <summary>
    /// This is a function which allows for control of the volume slider, it calls a hidden paramater called "SFXvolume"
    /// </summary>
    /// <param name="sliderValue"></param>
    public void SetSFXLevel(float sliderValue)
    {
        audioMixer.SetFloat("SFXvol", Mathf.Log10(sliderValue) * 20); //logarithimic scale
    }

    /// <summary>
    /// This function is to control the graphics quality of the game by setting the quality level to the index level which is set
    /// in the options menu when you select it
    /// </summary>
    /// <param name="qualityIndex"></param>
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    /// <summary>
    /// This funciton is to toggle on fullscreen, it gains access to the fullscreen setting and assigns a bool to it
    /// either true or false
    /// </summary>
    /// <param name="isFullscreen"></param>
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    /// <summary>
    /// This function is to set the resoluton of the game, is gains access to the resolution setting and assigns an index which is
    /// selected in the options tab of the game. it then sets the resolution based on what index is selected.
    /// </summary>
    /// <param name="resolutionIndex"></param>
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    /// <summary>
    /// This function is attached onto the options button which sets the options panel to tru and the others to false
    /// </summary>
    public void Options()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        Buildings.SetActive(false);
    }


    /// <summary>
    /// This function is to quit out of the project as a while using the "Application.Quit" function which completely closes
    /// the application
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// This funciton is also attached onto a button which allows for us to go back to the previous panel from the options menu
    /// </summary>
    public void Back()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        Buildings.SetActive(true);
    }
}
