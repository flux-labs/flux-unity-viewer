using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BimInfoCanvas : MonoBehaviour {
	public Text leftText;
	public Text rightText;
	public Text centerText;
	public GameObject stereoscopicCanvases;
	public GameObject monoscopicCanvases;

	PlayerController p;

	void Start() {
		p = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
	}

	public void SetText(string text) {
		if (text == "") gameObject.SetActive (false);
		else {
			centerText.text = text;
			leftText.text = text;
			rightText.text = text;
			gameObject.SetActive (true);
			monoscopicCanvases.SetActive (!p.StereoscopicVision);
			stereoscopicCanvases.SetActive (p.StereoscopicVision);
		}
	}
	
}
