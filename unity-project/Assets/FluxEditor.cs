//using UnityEngine;
//using System.Collections;
//using UnityEditor;
//
//public class FluxEditor : EditorWindow
//{
//	string project = "aroJ3KdLZzyOwGkJO";
//	string cell = "40ad6418f550091a59d9978797b24c6d";
//	bool websockets = true;
//
//	bool groupEnabled;
//	bool myBool = true;
//	float myFloat = 1.23f;
//
//	// Add menu item named "My Window" to the Window menu
//	[MenuItem("Window/Flux")]
//	public static void ShowWindow()
//	{
//		//Show existing window instance. If one doesn't exist, make one.
//		EditorWindow.GetWindow(typeof(FluxEditor));
//	}
//		
//	void UpdateFluxInfo() {
//		EchoTest ws = GameObject.FindGameObjectWithTag ("websocket").GetComponent<EchoTest> ();
//		ws.TurnedOn = websockets;
//		ws.Project = project;
//
//		scriptologist flux = GameObject.FindGameObjectWithTag ("flux").GetComponent<scriptologist> ();
//		flux.Project = project;
//		flux.Cell = cell;
//	}
//
//	void OnGUI()
//	{
//		GUILayout.Label ("Flux Info", EditorStyles.boldLabel);
//		project = EditorGUILayout.TextField ("Project", project);
//		cell = EditorGUILayout.TextField ("Data key / cell", cell);
//		websockets = EditorGUILayout.Toggle ("Live update", websockets);
//
//		if(GUILayout.Button("Update Flux Info"))
//		{
//			UpdateFluxInfo ();
//			GameObject.FindGameObjectWithTag ("flux").GetComponent<scriptologist> ().GetData ();
//			this.Close ();
//		}
//	}
//}