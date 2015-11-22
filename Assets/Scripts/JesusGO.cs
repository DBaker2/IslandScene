using UnityEngine;
using System.Collections;

public class JesusGO : MonoBehaviour {
	public float speed = 6.0F;
	public const float GRAV = 9.8F;
	public float gravity = GRAV;

	public static Quaternion camRotation;

	private Vector3 moveDirection = Vector3.zero;
	private Vector3 charContCent = Vector3.zero;

	private Animator myAnimator;
	private float timer;
	private float timer2;
	private bool downAndOut;
//	private Camera.main mainCam;


	// Use this for initialization
	void Start () {
		myAnimator = GetComponent<Animator>();	

		Debug.Log("Hello", gameObject);
		camRotation = Camera.main.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {


//		myAnimator.SetFloat("hSpeed", Input.GetAxis("Horizontal"));
		CharacterController controller = GetComponent<CharacterController>();
		Debug.Log (controller.collisionFlags);


		if (myAnimator.GetBool ("startFall")) {
			if ((controller.collisionFlags & CollisionFlags.Below)!=0){
				myAnimator.SetBool ("startFall", false);
				downAndOut = true;
				Invoke ("doNothing", 1.05f);
			}
		}



		// when liftoff happens, stop moving, that's why the if logic
		if (myAnimator.GetBool ("liftoff") && timer2 > 1.03f) {
			moveDirection = new Vector3 (0, 3, 0);
		} else if (myAnimator.GetBool ("liftoff") && timer2 <= 1.03f) {
			moveDirection = new Vector3 (0, 0, 0);
		} else if (downAndOut) {
			moveDirection = new Vector3 (0, 0, 0);
		} else if (!myAnimator.GetBool("liftoff"))  {
			moveDirection = new Vector3 (0, 0, Input.GetAxis ("Vertical"));
			moveDirection = Camera.main.transform.forward;
		}
		charContCent = 4 * moveDirection;
		charContCent.y += 1.05f;
		controller.center = charContCent;
		moveDirection.y -= gravity;
		controller.Move (moveDirection * Time.deltaTime * speed);
		myAnimator.SetFloat ("vSpeed", speed);


//		transform.localRotation = Camera.main.transform.rotation;
		camRotation = Camera.main.transform.rotation;
//		controller.rotation = Camera.main.transform.rotation;


		if(Input.GetButtonDown("Jump")){
			myAnimator.SetBool("jumping",true);
			Invoke("doNothing", 0.1f);
			
		}

		
		// angle
//		transform.Rotate (Vector3.left * myAnimator.GetFloat ("hSpeed"));
		
		
		if (Input.GetKey ("q")) {
			transform.Rotate (Vector3.down * Time.deltaTime * 100.0f);
		}
		
		if (Input.GetKey ("e")) {
			transform.Rotate (Vector3.up * Time.deltaTime * 100.0f);
		}
		if (Input.GetKeyDown ("t")) {
			myAnimator.SetBool("tPose", !(myAnimator.GetBool("tPose")));
			if (myAnimator.GetBool("tPose"))
				gravity = 0;
			else gravity = GRAV;
		}

		// timer is negative after mouse button is released, and is reset to zero when mouse button is clicked
		if (timer > -1f) {
			timer += Time.deltaTime;
		}
		if (timer2 > -1f) {
			timer2 += Time.deltaTime;
		}

		///////////////////////////////////////////////////////////////////////////
///////////////////////// MOUSE CLICKING STUFF ///////////////////////////
		/// ///////////////////////////////////////////////////////////////////////////

		// different things happen depending on how long you hold down the timer
		if (Input.GetMouseButtonDown (0)) {
		    timer = 0f;      // resets the timer
		}

		// release mouse button
		if (Input.GetMouseButtonUp (0)) {
			if (timer < 1f && (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("strut_walking") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("running"))) {
				gravity = 0;
				myAnimator.SetBool("liftoff", true);
				timer2 = 0;

				Invoke("doNothing", 2.6f);   // set liftoff back to false after half a second


			} else if (timer < 1f && (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("flying") || myAnimator.GetCurrentAnimatorStateInfo(0).IsName("floating")) ) {
				gravity = GRAV;
				myAnimator.SetBool("startFall", true);
			}

			else if (timer > 1f) { 
				if (speed > 10) speed = 3;

				else speed = 12;
			}
			timer = -1f;
		}


		if (Input.GetKeyDown ("2")) {
			myAnimator.SetBool("pose2", !(myAnimator.GetBool("pose2")));
		}
	}
	

	void doNothing() {
		myAnimator.SetBool("jumping", false);
		myAnimator.SetBool("liftoff", false);
		myAnimator.SetBool("startFall", false);
		downAndOut = false;

	}

	public static Quaternion getRotation() {
		return camRotation;
	}

	public static Vector3 getForward() {
		return Camera.main.transform.forward;
	}

}
