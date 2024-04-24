using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virtual_Mouse : MonoBehaviour
{
    public Texture2D cursor;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    void CursorPos()
    {
      /*Make the regular hardware mouse cursor lock position with Cursor.lockState = CursorLockMode.Locked; in the Start method
       
        Create a new game object called "Mouse Cursor". Using an image on a screen - space canvas for this purpose, 
        but in a 2d game you could use a sprite in world - space.
         
        Create a script on it which uses Input.GetAxis("Vertical") and Input.GetAxis("Horizontal") to measure the vertical and horizontal mouse movement since the last update.
        Multiply that value with Time.deltaTime and your desired speed multiplier, and change the position of the mouse cursor object accordingly.*/

    }
}
