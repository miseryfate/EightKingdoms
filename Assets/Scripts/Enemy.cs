using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public List<Transform> wayPoints = new List<Transform>();
    private int currentWaypoint= 0;
    private Transform target;
    public float reachDistance;
    public float moveSpeed;
    public float turnSpeed = 10f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (target != null) {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            Vector3 direction = target.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);

            if (Vector3.Distance(transform.position, target.transform.position) <= reachDistance)
            {
                SetNewWaypoint();
            }
        }
	}

    public void SetupWaypoints(List<Transform> _wayPoints) {
        wayPoints.Clear();
        for (int i = 0; i < _wayPoints.Count; i++) {
            wayPoints.Add(_wayPoints[i]);
        }
        currentWaypoint = 0;
        SetNewWaypoint();
    }

    void SetNewWaypoint() {
        if (currentWaypoint < wayPoints.Count -1) {
            currentWaypoint++;
            target = wayPoints[currentWaypoint];
        }
        else {
            Destroy(gameObject);
        }
    }
}
