using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelManager : MonoBehaviour {

	public static string Authcred;
	public static string Token;

	public string Project;
	public string Cell;
	public string server = "https://flux.io";
	public float MaxSize;
	string url;
	public GameObject prefab;

	public List<Material> materials;

	List<string> idList = new List<string> ();
	List<string> bimDataList = new List<string>();

	int materialIdx = -1;

	void Start() {
		Application.ExternalCall ("Loaded", "Loaded!"); 
	}

	// Load data in the shape of an array with two elements: geometry, and bim information.
	public void CreateGeometryAndBimInfo(string json) {
		GameObject gameObj = getGameObjectFromId (json.Substring (0, 32));
		JSONObject geometryAndBim = new JSONObject(json.Substring(32));
		AddGeometryToObject (gameObj, geometryAndBim.list[0]);
		BimData b = gameObj.GetComponent<BimData> ();
		b.SetBimData(geometryAndBim.list[1]);
	}

	// Load pure geometric data
	public void CreateGeometry(string json) {
		GameObject gameObj = getGameObjectFromId (json.Substring (0, 32));
		JSONObject geometry = new JSONObject(json.Substring(32));
		AddGeometryToObject (gameObj, geometry);
	}

	// Delete an element with a specific name.
	// Checks the tag so it won't destroy anything other than Flux geometry.
	public void DeleteGeometry(string id) {
		GameObject tempGameObject = getGameObjectFromId (id);
		if (tempGameObject != null && tempGameObject.tag == "loadedgeometry") Destroy (tempGameObject);
	}

	// The ID of an object is always appended to the start. 
	// This is because the in-browser SendMessage method for communicating
	// with Unity only takes very simple objects.
	GameObject getGameObjectFromId(string id) {
		GameObject gameObj = GameObject.Find (id);
		if (gameObj == null) {
			gameObj = Instantiate (prefab);
			gameObj.name = id;
			materialIdx++;
			gameObj.GetComponent<MeshRenderer> ().material = materials [materialIdx%materials.Count];
		}
		return gameObj;
	}

	// Something about this method is wonky and doesn't work as an updating function. 
	void AddGeometryToObject(GameObject newObj, JSONObject json) {
		GeomParser g = newObj.GetComponent<GeomParser> ();
		g.MakeGeometry (json);
	}

	// Sets the scale of all geometry loaded from this point forward.
	public void SetSize(float newSize) {
		MaxSize = newSize;
	}

	public void ResetCoords() {
		GeomParser.SmallestCoordsUnset = true;
	}
}

