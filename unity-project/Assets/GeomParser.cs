using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class GeomParser : MonoBehaviour {

	public string PublicDataKey;
	Mesh mesh;

	bool flippedXAndZ = false; 

	List<Vector3> vertices = new List<Vector3>();
	List<Vector2> uv = new List<Vector2> ();
	List<int> triangles = new List<int>();

	float smallestX;
	float smallestY;
	float smallestZ;
	float largestX;
	float largestY;
	float largestZ;
	float maximumEdgeSize;
	public float MaxSize = 1;
	bool smallestCoordsUnset;

	public void MakeGeometry(JSONObject unknownJson) {
		mesh = gameObject.GetComponent<MeshFilter> ().mesh;

//		if ((json.Substring(0, 1) != "{") && (json.Substring(0, 1) != "[")) {
//			mesh = new Mesh();
//			gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
//			gameObject.GetComponent<MeshRenderer>().material = null;
//			gameObject.GetComponent<MeshFilter> ().mesh = null;
//			print ("Your Flux data is in the wrong format.");
//			print (json.Substring (0, 100));
//			return;
//		} 
//		json = json.Replace ("\n", "");

//		JSONObject unknownJson = new JSONObject(json);

		vertices = new List<Vector3> (); 
		triangles = new List<int> ();

		// Sets smallest coordinates, largest coordinates and max edge size of mesh.
		smallestCoordsUnset = true;
		RecursiveGeometrySizingPass (unknownJson);

		// Set the largest edge size of this mesh.
		maximumEdgeSize = largestX - smallestX;
		maximumEdgeSize = (maximumEdgeSize < (largestY - smallestY)) ? largestY - smallestY : maximumEdgeSize;
		maximumEdgeSize = (maximumEdgeSize < (largestZ - smallestZ)) ? largestZ - smallestZ : maximumEdgeSize;

		// Actually attach geometry to your 
		RecursiveGeometryRecord (unknownJson);

		// Creating an array and assigning mesh.vertices to it is 
		// much faster because of code 'behind the scenes'
		// http://docs.unity3d.com/Manual/Example-CreatingaBillboardPlane.html
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();

		Vector2[] uv = new Vector2[vertices.Count];
		for (int i = 0; i < uv.Length; i++) {
			uv [i] = new Vector2 (vertices [i].x, vertices [i].y + vertices[i].z);
		}
		mesh.uv = uv;

		mesh.RecalculateNormals ();

		gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
		gameObject.GetComponent<MeshFilter> ().mesh = mesh;

		// Flux flips X and Z. The first time geometry is loaded, the 
		if (!flippedXAndZ) {
			flippedXAndZ = true;
			transform.position = new Vector3 (0, 0, 0);
			transform.eulerAngles = (new Vector3 (270, 0, 0));
		}
	}

	void RecursiveGeometryRecord(JSONObject unknownJson) {
		if (unknownJson.type == JSONObject.Type.ARRAY) {
			for (int y = 0; y < unknownJson.list.Count; y++) {
				RecursiveGeometryRecord (unknownJson.list [y]);
			}
		} else if (unknownJson.type == JSONObject.Type.OBJECT) {
			bool itsAMesh = false;
			int facesIdx = 0;
			int verticesIdx = 0;
			for (int i = 0; i < unknownJson.keys.Count; i++) {
				if (unknownJson.keys [i] == "vertices") {
					verticesIdx = i;
					itsAMesh = true;
				} else if (unknownJson.keys [i] == "faces") {
					facesIdx = i;
				}
			}

			if (itsAMesh) {
				// We have to add the face # to the current total # of vertices.
				JSONObject faces = unknownJson.list [facesIdx];
				for (int z = 0; z < faces.list.Count; z++) {
					JSONObject face = faces.list [z];

					if ( face.list.Count == 3 ) {
						triangles.Add ((int)face.list [0].n + vertices.Count);
						triangles.Add ((int)face.list [1].n + vertices.Count);
						triangles.Add ((int)face.list [2].n + vertices.Count);
						// Do it again for the second side of the mesh
						triangles.Add ((int)face.list [2].n + vertices.Count);
						triangles.Add ((int)face.list [1].n + vertices.Count);
						triangles.Add ((int)face.list [0].n + vertices.Count);
					} else if ( face.list.Count > 3 ) {
						for ( var j=0; j+2<face.list.Count; j++) {
							triangles.Add ((int)face.list [0].n + vertices.Count);
							triangles.Add ((int)face.list [j+1].n + vertices.Count);
							triangles.Add ((int)face.list [j+2].n + vertices.Count);
							// Do it again for the second side of the mesh
							triangles.Add ((int)face.list [j+2].n + vertices.Count);
							triangles.Add ((int)face.list [j+1].n + vertices.Count);
							triangles.Add ((int)face.list [0].n + vertices.Count);
						}
					}

					for (int u = 0; u < face.list.Count; u++) {
						triangles.Add ((int)face.list [u].n + vertices.Count);       
					}

					// if triangles isn't divisible by 3, add some padding.
					for (int xy = 0; xy < triangles.Count % 3; xy++) {
						triangles.Add (triangles[triangles.Count-1]);
					}
				}

				JSONObject inspectedVertices = unknownJson.list [verticesIdx];
				for (int i = 0; i < inspectedVertices.list.Count; i++) {
					JSONObject vertex = inspectedVertices.list [i];

					float multiplier = .06f;
					multiplier *= MaxSize;

					vertices.Add (new Vector3 (
						vertex.list [0].n * multiplier,
						vertex.list [1].n * multiplier,
						vertex.list [2].n * multiplier));
//						((vertex.list [0].n) * MaxSize ) / maximumEdgeSize - smallestX, 
//						((vertex.list [0].n) * MaxSize ) / maximumEdgeSize, 
//						((vertex.list [1].n) * MaxSize ) / maximumEdgeSize - smallestY, 
//						((vertex.list [1].n) * MaxSize ) / maximumEdgeSize, 
//						((vertex.list [2].n) * MaxSize ) / maximumEdgeSize - smallestZ));
//						((vertex.list [2].n) * MaxSize ) / maximumEdgeSize));
				}       
			}
		}
	}

	// Find the lowest x, y and z points.
	// Note: this function is not used because it messes up overlapping data keys.
	void RecursiveGeometrySizingPass(JSONObject unknownJson) {
		if (unknownJson.type == JSONObject.Type.ARRAY) {
			for (int y = 0; y < unknownJson.list.Count; y++) {
				RecursiveGeometrySizingPass (unknownJson.list [y]);
			}

		} else if (unknownJson.type == JSONObject.Type.OBJECT) {
			bool itsAMesh = false;
			int verticesIdx = 0;
			for (int i = 0; i < unknownJson.keys.Count; i++) {
				if (unknownJson.keys [i] == "vertices") {
					verticesIdx = i;
					itsAMesh = true;
				} 
			}

			if (itsAMesh) {
				JSONObject inspectedVertices = unknownJson.list [verticesIdx];
				for (int i = 0; i < inspectedVertices.list.Count; i++) {
					JSONObject vertex = inspectedVertices.list [i];
					if (smallestCoordsUnset || vertex.list[0].n < smallestX) smallestX = vertex.list[0].n;
					if (smallestCoordsUnset || vertex.list[1].n < smallestY) smallestY = vertex.list[1].n;
					if (smallestCoordsUnset || vertex.list[2].n < smallestZ) smallestZ = vertex.list[2].n;
					if (smallestCoordsUnset || vertex.list[0].n > largestX) largestX = vertex.list[0].n;
					if (smallestCoordsUnset || vertex.list[1].n > largestY) largestY = vertex.list[1].n;
					if (smallestCoordsUnset || vertex.list[2].n > largestZ) largestZ = vertex.list[2].n;
					smallestCoordsUnset = false;
				}       
			}
		}
	}
}
