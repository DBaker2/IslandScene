using UnityEngine;
using System.Collections;

public class JesusAv : MonoBehaviour {

	private Quaternion myRotation;
//	public Quaternion JesusGO.camRotation;

	// Use this for initialization
	void Start () {
//		cR = JesusGO.camRotation;
	}
	
	// Update is called once per frame
	void Update () {
//		myRotation = cR;
		transform.rotation = JesusGO.getRotation();
		transform.localPosition = 4* JesusGO.getForward();
	}
}
