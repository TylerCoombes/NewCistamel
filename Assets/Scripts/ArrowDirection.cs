using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDirection : MonoBehaviour
{
    public bool rotateArrow;
    public Transform Player;
    public void Start()
    {
        rotateArrow = false;
    }

    public void Update()
    {/*
        if(rotateArrow)
        {
            RotateAround();
        }*/

        //Aim player at mouse
        //which direction is up
        Vector2 upAxis = Vector2.up;
        Vector2 mouseScreenPosition = Input.mousePosition;
        //set mouses z to your targets
        mouseScreenPosition.x = Player.position.x;
        Vector2 mouseWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        transform.LookAt(mouseWorldSpace, upAxis);
        //zero out all rotations except the axis I want
        transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);
    }

    public void RotateAround()
    {
        //Aim player at mouse
        //which direction is up
        Vector3 upAxis = new Vector3(0, 0, -1);
        Vector3 mouseScreenPosition = Input.mousePosition;
        //set mouses z to your targets
        mouseScreenPosition.z = transform.position.z;
        Vector3 mouseWorldSpace = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        transform.LookAt(mouseWorldSpace, upAxis);
        //zero out all rotations except the axis I want
        transform.eulerAngles = new Vector3(0, 0, -transform.eulerAngles.z);
    }
}
