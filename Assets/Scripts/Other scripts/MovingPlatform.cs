using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private WaypointPath _waypointPath;
    [SerializeField] private float _speed;
    [SerializeField] private float waitTime;
    private float currentWaitTime;

    public Animator animator;
    public Animator animator2;

    private int _targetWaypointIndex;

    private Transform _previousWaypoint;
    private Transform _targetWaypoint;

    private float _timeToWaypoint;
    private float _elapsedTime;

    private float playerIsUnderTimer;
    private bool playerIsUnder;

    void Start()
    {
        TargetNextWaypoint();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerIsUnder = true;
            playerIsUnderTimer = 1f;
            animators(false);
        }
    }

    private void OnCollisionStay(Collision collision)
	{
        if (collision.gameObject.tag == "Player")
		{
            playerIsUnder = true;
            playerIsUnderTimer = 1f;
            animators(false);
        }
    }

    void FixedUpdate()
    {
        if (playerIsUnder) return;

        _elapsedTime += Time.deltaTime;

        float elapsedPercentage = _elapsedTime / _timeToWaypoint;
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
        transform.position = Vector3.Lerp(_previousWaypoint.position, _targetWaypoint.position, elapsedPercentage);
        transform.rotation = Quaternion.Lerp(_previousWaypoint.rotation, _targetWaypoint.rotation, elapsedPercentage);

        if (elapsedPercentage >= 1)
        {
            animators(false);

            if (currentWaitTime > 0)
			{
                currentWaitTime -= Time.deltaTime;
			} 
            else
			{
                TargetNextWaypoint();
            }
        }
      
    }

	private void Update()
	{
        if (playerIsUnder == false) return;

        if (playerIsUnderTimer > 0)
		{
            playerIsUnderTimer -= Time.deltaTime;
        } 
        else
		{
            playerIsUnder = false;
            animators(true);
		}

    }

	private void TargetNextWaypoint()
    {
        _previousWaypoint = _waypointPath.GetWaypoint(_targetWaypointIndex);
        _targetWaypointIndex = _waypointPath.GetNextWaypointIndex(_targetWaypointIndex);
        _targetWaypoint = _waypointPath.GetWaypoint(_targetWaypointIndex);

        currentWaitTime = waitTime;
        animators(true);
        _elapsedTime = 0;

        float distanceToWaypoint = Vector3.Distance(_previousWaypoint.position, _targetWaypoint.position);
        _timeToWaypoint = distanceToWaypoint / _speed;
    }
    
    private void animators(bool enabled)
	{
        if (animator != null) animator.enabled = enabled;
        if (animator2 != null) animator2.enabled = enabled;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
