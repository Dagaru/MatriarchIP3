using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBall : MonoBehaviour
{
	[SerializeField] float tiltSensitivity;

	[SerializeField] Vector3 currentRot;
	private float timerToStart;
	private Rigidbody body;
	
	// Use this for initialization
	private void Start()
	{
		body = GetComponent<Rigidbody>();
		tiltSensitivity = 40f;
		timerToStart = 0f;
	}

	// Update is called once per frame
	private void Update()
	{


        if (body.IsSleeping())
        {
			body.WakeUp();
		}
		
       

        

		currentRot = GetComponent<Transform>().eulerAngles;
		timerToStart += Time.deltaTime;

        //		normalizeRotation ();
       
        if (timerToStart >= 2)
        {
			if ((Input.GetAxis("Mouse X") > 0.2) && (currentRot.z >= 349 || currentRot.z <= 11))
			{
				//			transform.Rotate (0, 0, 1);	
				transform.Rotate(new Vector3(0, 0, -1) * tiltSensitivity * Time.deltaTime);
			}

			if (Input.GetAxis("Mouse X") < -0.2 && (currentRot.z <= 10 || currentRot.z >= 180))
			{
				transform.Rotate(new Vector3(0, 0, 1) * tiltSensitivity * Time.deltaTime);
			}

			if (Input.GetAxis("Mouse Y") > 0.2 && (currentRot.x <= 10 || currentRot.x >= 180))
			{
				transform.Rotate(new Vector3(1, 0, 0) * tiltSensitivity * Time.deltaTime);
			}

			if (Input.GetAxis("Mouse Y") < -0.2 && (currentRot.x >= 349 || currentRot.x <= 11))
			{
				transform.Rotate(new Vector3(-1, 0, 0) * tiltSensitivity * Time.deltaTime);
			}
		}
	}

}

