using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// This script can be used to move objects on a patrol path.
// In the hierarchy, this object must have at least two children
// tagged with "Patrol Point." This script will then move the
// patrolObject on the path defined by the position of those points
// at the speed defined by speed.
// NOTE: the patrolObject does not need to be inside an object with
// the patrol script. An object with the patrol script only defines
// a path, and then any object can be told to follow that path.
public class Patrol : MonoBehaviour
{
	public float speed;
	private int currentPoint;
	private float distToPoint;
	public Transform patrolObject;
	private List<Transform> points;

    void Awake()
    {
		points = new List<Transform>();
		foreach (Transform t in gameObject.GetComponentInChildren<Transform>())
		{
			if (t.gameObject.tag != "Patrol Point")
			{
				return;
			}
			points.Add(t);
		}

		if (points.Count < 2) {
			Debug.LogError("Patrol object must have at least 2 child game objects tagged as \"Patrol Point\"");
			Destroy(this);
		}

		distToPoint = Vector3.Distance(patrolObject.position, points[0].position);
    }

    // Update is called once per frame
    void Update()
    {
		patrolObject.position = Vector3.MoveTowards(patrolObject.position, points[currentPoint].position, speed * Time.deltaTime);
		distToPoint -= speed * Time.deltaTime;

		if (distToPoint < 0)
		{
			currentPoint++;
			if (currentPoint == points.Count)
			{
				currentPoint = 0;
			}

			distToPoint = Vector3.Distance(patrolObject.position, points[currentPoint].position);
		}
    }
}
