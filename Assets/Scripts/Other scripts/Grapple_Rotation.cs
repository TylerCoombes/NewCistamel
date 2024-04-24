using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Grapple_Rotation : MonoBehaviour
{
    public Transform targetTransform;
    public float yAimOffset;
    public float minAim;
    public float maxAim;
    public Renderer grappleSpriteRenderer;
    private Camera mainCamera;
    [SerializeField] private LayerMask grappleMask;
    public bool shotGunOut;

    public gun_rotation gunRotation;
    public RaycastHit hit;
    public Ray ray;

    public TextMeshPro eText;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        //start invisible
        grappleSpriteRenderer.enabled = false;

        shotGunOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        updateTargetPosition();

        aimGunAtTarget();

    }

    void updateTargetPosition()
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, grappleMask))
        {
            targetTransform.position = hit.point;
            Debug.Log("Hit" + hit.transform.gameObject.name);

            eText = hit.transform.gameObject.GetComponentInChildren<TextMeshPro>();
            eText.enabled = true;
        }
        else
        {
            eText.enabled = false;
        }
    }

    void aimGunAtTarget()
    {
        if(!gunRotation.shotGunOut)
        {
            //grappleSpriteRenderer.enabled = true;

            //this is the Destination - Origin to get the direction for a raycast
            Vector3 direction = targetTransform.position - transform.position;
            //debug the raycast (doesnt showup if gizmos are turned off, it didnt take me 20 minutes to realize that)
            Debug.DrawRay(transform.position, direction, Color.green);

            //rotate the gun to face the target
            float yOverwrite = targetTransform.position.y + yAimOffset - transform.position.y;
            float angle = Mathf.Atan2(yOverwrite, direction.x) * Mathf.Rad2Deg;
            Mathf.Clamp(angle, minAim, maxAim);
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
        else
        {
            grappleSpriteRenderer.enabled = false;
        }
    }
}
