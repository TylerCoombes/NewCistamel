using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPopUps : MonoBehaviour
{
    public GameObject popUpPanel;
    public AudioSource Audio;

    // Start is called before the first frame update
    void Start()
    {
        Audio = GameObject.FindGameObjectWithTag("AudioSFX").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        Time.timeScale = 0;
        popUpPanel.SetActive(true);
        Audio.volume = 0;
    }

    public void ClosePopUp()
    {
        Time.timeScale = 1;
        popUpPanel.SetActive(false);
        Destroy(this.gameObject);
        Audio.volume = 1;
    }
}
