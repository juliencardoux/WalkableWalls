using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Camera cameraToUse;
	public Vector3 cameraDistance;

	public bool followPosition;

	void Update () {
		CameraPosition();
	}
	
	private void CameraPosition() {
		cameraToUse.transform.rotation = Quaternion.Slerp(cameraToUse.transform.rotation, Quaternion.LookRotation(transform.position - cameraToUse.transform.position), Time.deltaTime * 10);
		if(followPosition) cameraToUse.transform.position = Vector3.Slerp(cameraToUse.transform.position, transform.position + cameraDistance, Time.deltaTime * .5f);
	}


}
