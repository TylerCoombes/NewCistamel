using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public PlayerController playerController;
    public MousePosition2D mousePosition2D;

    public Transform Gun;
    public Transform Grapple;

    public float speed = 10f;
    public float gunAngle;

    public bool shotGunOut;
    public bool grappleGunOut;

    public LayerMask mouseAimMask;
    public Transform targetTransform;

    public void Start()
    {
        shotGunOut = false;
        grappleGunOut = false;
        Gun.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseAimMask))
        {
            targetTransform.position = hit.point;
        }

        if (Input.GetMouseButton(1))
        {
            Debug.Log("right mouse down");
            shotGunOut = true;
            Gun.gameObject.SetActive(true);
            RotateGun();
        }
        else
        {
            Gun.gameObject.SetActive(false);
            shotGunOut = false;
        }
    }

    public void RotateGun()
    {
        if(shotGunOut)
        {
            if (playerController.lookingRight)
            {
                gunAngle += Input.GetAxis("Mouse Y") * speed * Time.deltaTime;
                gunAngle = Mathf.Clamp(gunAngle, 0, 180);
                Gun.eulerAngles = new Vector3(0, 0, gunAngle);
            }

            if (!playerController.lookingRight)
            {
                gunAngle += Input.GetAxis("Mouse Y") * speed * -Time.deltaTime;
                gunAngle = Mathf.Clamp(gunAngle, -180, 0);
                Gun.eulerAngles = new Vector3(0, 0, gunAngle);
            }
        }
    }
}
