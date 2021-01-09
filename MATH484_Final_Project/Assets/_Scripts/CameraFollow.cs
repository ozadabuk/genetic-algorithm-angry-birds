using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Vector3 OriginPoint;
	public Transform Target;
	public bool followTarget = false;
	public float distToTarget = 0f;
	public float camZoom = 5f;

	void Update () {
		if (followTarget) {
			WatchTargetPosition (Target.position);
		} else {
			WatchTargetPosition (OriginPoint);
		}
	}

	public void WatchTargetPosition(Vector3 targetPosition){
		distToTarget = Vector3.Distance (transform.position, targetPosition);
		if (distToTarget > 0.2f) {
			Vector3 dir = targetPosition - transform.position;
			dir = new Vector3 (dir.x, dir.y, -5f);
			transform.position += dir * 10f * Time.deltaTime;
		}
	}

	public void FollowTarget(Transform target){
		Target = target;
		followTarget = true;
	}

	public void FollowOrigin(){
		followTarget = false;
	}
}
