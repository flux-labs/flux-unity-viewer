using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Linq;

public class PlayerController : MonoBehaviour {

	public float speed = 500.0F;
	public float rotationSpeed = 300.0F;

	public GameObject frisbee;
	public bool throttle = true;

	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;

	Rigidbody rigid;

	public Transform cameraTransform;

	public bool StereoscopicVision = true;
	public bool AllowDesktopControls;

	float inputX;
	float inputY;

	float lastButtonClickMobile = 0.0f;
	bool buttonAlreadyClicked = false;

	GameObject monoCam;
	GameObject stereoCam;
	GameObject mapCam;
	Image crosshairLeft;
	Image crosshairRight;
	Image crosshairSingle;

	public Shader toonShader;

	public BimInfoCanvas bimfo;

	public static bool FrisbeesOff = true;

	bool vrControls;

	void Start() {
		monoCam = GameObject.Find ("Monoscopic main camera");
		stereoCam = GameObject.Find ("Stereoscopic main camera");
		mapCam = GameObject.Find ("map camera");
//		crosshairLeft = GameObject.Find ("crosshairs left").GetComponent<Image>();
//		crosshairRight = GameObject.Find ("crosshairs right").GetComponent<Image>();
//		crosshairSingle = GameObject.Find ("crosshairs center").GetComponent<Image>();

//		ToggleMapCam ();

		calibrateCamera ();
		rigid = gameObject.GetComponent<Rigidbody> ();
	}

	void calibrateCamera() {
		cameraTransform = StereoscopicVision ? stereoCam.transform : monoCam.transform;
		monoCam.SetActive (!StereoscopicVision);
//		crosshairSingle.enabled = (!StereoscopicVision);
		stereoCam.SetActive (StereoscopicVision);
//		crosshairLeft.enabled = (StereoscopicVision);
//		crosshairRight.enabled = (StereoscopicVision);
	}

	public void SwitchCameraMode() {
		StereoscopicVision = !StereoscopicVision;
		calibrateCamera ();
	}

	public void SwitchAllowVRControls() {
		vrControls = !vrControls;

//		#if !UNITY_EDITOR && UNITY_WEBGL
//		WebGLInput.captureAllKeyboardInput = vrControls;
//		#endif

//		if (vrControls) {
//			Cursor.visible = false;
//			Cursor.lockState = UnityEngine.CursorLockMode.Locked;
//		} else {
//			Cursor.visible = true;
//			Cursor.lockState = UnityEngine.CursorLockMode.None;
//		}
	}

	void Update() {
		ControlMovement ();
		RaycastBimData ();
	}

	void ControlMovement() {
		// Mouselook
		float rotationX = transform.localEulerAngles.y + inputX* sensitivityX;

		rotationY += inputY * sensitivityY;
		rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);

		cameraTransform.localEulerAngles = new Vector3(-rotationY, 0, 0);
		transform.localEulerAngles = new Vector3 (0, rotationX, 0);

		if (Input.touchCount > 0) {
			if (buttonAlreadyClicked) {
				if (Time.time - .15f > lastButtonClickMobile) {
					// If the button has been held down for more than .3 seconds, 
					// start moving forward.
					Vector3 newVec = Vector3.zero;
					newVec += cameraTransform.forward * speed;
					rigid.velocity = new Vector3 (newVec.x, rigid.velocity.y, newVec.z);
				}
			} else {
				// This is a new click. Register it and the time of the click.
				buttonAlreadyClicked = true;
				lastButtonClickMobile = Time.time;
			}
		} else {
			if (buttonAlreadyClicked && (Time.time - .15f < lastButtonClickMobile)) {
				// If the screen was touched and released before .3 seconds, jump and shoot a frisbee.
				Jump ();
				ShootFrisbee ();
			}

			buttonAlreadyClicked = false;

			Vector3 newVector = Vector3.zero;
			newVector += Input.GetAxis ("Vertical") * cameraTransform.forward * speed;
			newVector += Input.GetAxis ("Horizontal") * Vector3.Cross (cameraTransform.forward, Vector3.down) * speed;
			rigid.velocity = new Vector3 (newVector.x, rigid.velocity.y, newVector.z);

			if (Input.GetMouseButton (0)) {
				MoveMouseY (Input.GetAxis ("Mouse Y"));
				MoveMouseX (Input.GetAxis ("Mouse X"));
			} else {
				MoveMouseY (0);
				MoveMouseX (0);
			}

			if (Input.GetButtonDown ("jump")) {
				Jump ();
			}
			if ((Input.GetButton ("Fire1") && !throttle) ||
				(Input.GetButtonDown ("Fire1"))) {
				ShootFrisbee ();
			}
		}
	}

	void RaycastBimData() {
		Camera cam = StereoscopicVision ? 
			cameraTransform.GetComponentInChildren<Camera> () :
			cameraTransform.GetComponent<Camera> ();

		// If using a stereoscopic camera, the cursor should be at one quarter of screen width, 
		// otherwise it should be one half.
		int screenSizeDivisor = StereoscopicVision ? 4 : 2;
		Ray ray = cam.ScreenPointToRay(new Vector2(Screen.width/screenSizeDivisor, Screen.height/2));

		// Attempt to find a BimData class in the object and display its information.
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			BimData b = hit.collider.gameObject.GetComponent<BimData> ();
			if (b != null) bimfo.SetText (b.GetBimData());
				
		} else bimfo.SetText ("");
	}

	void ShootFrisbee() {
		if (FrisbeesOff) return;
		GameObject frisbeeObj = (GameObject)Instantiate (frisbee, cameraTransform.position + cameraTransform.forward, Quaternion.identity);

		frisbeeObj.GetComponent<Rigidbody> ().velocity = cameraTransform.forward * 20;

		frisbeeObj.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, -10000, 0);
	}

	public void Jump() {
		gameObject.transform.Translate (new Vector3 (0, 1, 0));
		rigid.velocity = new Vector3 (0, 5, 0);
	}

	// The in browser controls are way too fast.
	public void MoveMouseX(float movement) {
		inputX = movement/10;
	}

	public void MoveMouseY(float movement) {
		inputY = movement/10;
	}

	public void ToggleFrisbees() {
		FrisbeesOff = !FrisbeesOff;
	}

	public void ToggleMapCam() {
//		mapCam.GetComponent<Camera> ().enabled = false;
		Camera mc = mapCam.GetComponent<Camera> ();
		mc.enabled = true;
//		mc.SetReplacementShader (toonShader, null);
		mc.RenderWithShader (toonShader, null);
	}

	public void GetPosition() {
		Application.ExternalCall ("ReceivePosition", transform.position.ToString () + transform.localEulerAngles.ToString()); 
	}

	public void SetPosition(string pos) {
		// Verify format before continuing. 
		print("SetPosition");
		if (!Regex.IsMatch(pos, @"\((-?\d+\.\d+),(-?\d+\.\d+),(-?\d+\.\d+)\)\((-?\d+\.\d+),(-?\d+\.\d+),(-?\d+\.\d+)\)")) return;
		print ("Got through");
		float[] matches = Regex.Matches(pos, @"-?\d+\.\d+")
			.Cast<Match>()
			.Select(m => float.Parse(m.Value))
			.ToArray();
		transform.position = new Vector3 (matches [0], matches [1], matches [2]);
		transform.localEulerAngles = new Vector3 (matches [3], matches [4], matches [5]);
	}
}
