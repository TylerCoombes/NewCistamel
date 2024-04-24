using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun_rotation : MonoBehaviour
{

    public Animator animator;
    public Animator playerAnimator;
    public SpriteRenderer playerRenderer;

    public Transform inplaceL;
    public Transform inplaceR;
    public Transform fallingL;
    public Transform fallingR;
    public Transform jumpingL;
    public Transform jumpingR;
    public Transform runningL;
    public Transform runningR;
    private Vector3 newPosition;

    public Transform targetTransform;
    public float yAimOffset;
    public GameObject gun;
    public Transform launchPos;
    private Camera mainCamera;
    private SpriteRenderer gunSpriteRenderer;
    private bool aiming;
    private bool lookRight;
    public float minAim;
    public float maxAim;
    [SerializeField] private LayerMask mouseAimMask;
    public bool shotGunOut;
    public Grapple_Rotation grappleRotation;

    public GameObject directionalArrow;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;

        gunSpriteRenderer = gun.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        aiming = this.GetComponentInParent<PlayerController>().isAiming;
        lookRight = this.GetComponentInParent<PlayerController>().lookingRight;

        if(aiming)
        {
            gunSpriteRenderer.enabled = true;
        }
        else
        {
            gunSpriteRenderer.enabled = false;
        }

        updateTargetPosition();

        aimGunAtTarget();
    }

    void updateTargetPosition()
	{
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseAimMask))
        {
            targetTransform.position = hit.point;
        }

        if (targetTransform.position.x < Camera.main.transform.position.x) //target is to the left of the player
		{
            transform.parent.GetComponent<PlayerController>().isAimingRight = false;
		} else
		{
            transform.parent.GetComponent<PlayerController>().isAimingRight = true;
        }
    }

    void aimGunAtTarget()
	{
        if (aiming)
        {
            if (!shotGunOut) animator.SetTrigger("is_aiming");

            //this is the Destination - Origin to get the direction for a raycast
            Vector3 direction = targetTransform.position - transform.position;
            //debug the raycast (doesnt showup if gizmos are turned off, it didnt take me 20 minutes to realize that)
            Debug.DrawRay(transform.position, direction, Color.green);

            //add an offset to the y position to point the gun more accurately
            float yOverwrite = targetTransform.position.y + yAimOffset - transform.position.y;
            //scary trigonometry
            float angle = Mathf.Atan2(yOverwrite, direction.x) * Mathf.Rad2Deg;

            shotGunOut = true;

            if (targetTransform.position.x < transform.position.x) //adjust the gun position when flipping the sprite
            {
                gunSpriteRenderer.flipY = true;
                //angle = ModularClamp(angle, 0, 360, 0, 360);
                //gun.transform.localPosition = new Vector3(gun.transform.localPosition.x, 1.765f,  gun.transform.localPosition.z);
                //launchPos.localPosition = launchPos.position = new Vector3(2.77f, -1.57f, 0);
            }
            else
            {
                gunSpriteRenderer.flipY = false;
                //angle = Mathf.Clamp(angle, -80, 45);
                //gun.transform.localPosition = new Vector3(gun.transform.localPosition.x, -1.813f, gun.transform.localPosition.z);
                //launchPos.localPosition = new Vector3(2.77f, 1.57f, 0);
            }

            //apply the rotation
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
           // directionalArrow.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, angle - 270f));

            //apply the position offest
            setPositionOffset();
            transform.position = newPosition;

        }
        else shotGunOut = false;

    }

    static public float ModularClamp(float val, float min, float max, float rangemin = -180f, float rangemax = 180f)
    {
        var modulus = Mathf.Abs(rangemax - rangemin);
        if ((val %= modulus) < 0f) val += modulus;
        return Mathf.Clamp(val + Mathf.Min(rangemin, rangemax), min, max);
    }

    void setPositionOffset ()
	{
        
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("chr_aim_running"))
        {
            if (playerRenderer.flipX == true)
			{
                newPosition = runningL.position;
			}
			else
			{
                newPosition = runningR.position;
            }
        } 
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("chr_aim_stance"))
        {
            if (playerRenderer.flipX == true)
            {
                newPosition = inplaceL.position;
            }
            else
            {
                newPosition = inplaceR.position;
            }
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("chr_aim_in_place"))
        {
            if (playerRenderer.flipX == true)
            {
                newPosition = inplaceL.position;
            }
            else
            {
                newPosition = inplaceR.position;
            }
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("chr_aim_jumping"))
        {
            if (playerRenderer.flipX == true)
            {
                newPosition = jumpingL.position;
            }
            else
            {
                newPosition = inplaceR.position;
            }
        }
        else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("chr_aim_falling"))
        {
            if (playerRenderer.flipX == true)
            {
                newPosition = fallingL.position;
            }
            else
            {
                newPosition = fallingR.position;
            }
        }

    }

}
