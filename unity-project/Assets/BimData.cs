using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BimData : MonoBehaviour {

	public string bimInfo = "";
	string previousKey = "";
	bool recordNextValue = false;
	List<string> importantKeys = new List<string> {
		"Solar Heat Gain Coefficient",
		"Construction Type Id",
		"OmniClass Number",
		"Assembly Code",
	};

	public void SetBimData(JSONObject bim) {
		bimInfo = "";
		RecursiveBimInfoParse (bim);
	}

	public string GetBimData() {
		return bimInfo;
	}

	// Recursively looks inside a JSON object for values associated with keys in
	// the list "importantKeys." 
	// This is almost certainly not the best way to gather bim data, but it will 
	// work when taken from a key populated by Revit data.
	void RecursiveBimInfoParse(JSONObject unknownJson) {
		if (unknownJson.type == JSONObject.Type.ARRAY || 
			unknownJson.type == JSONObject.Type.OBJECT) {
			for (int y = 0; y < unknownJson.list.Count; y++) {
				if (unknownJson.type == JSONObject.Type.OBJECT) {
					previousKey = unknownJson.keys [y];
					if (importantKeys.Contains(unknownJson.keys[y])) {
						bimInfo += unknownJson.keys [y];
						importantKeys.Remove (unknownJson.keys [y]);
						recordNextValue = true;
					}
				}
				RecursiveBimInfoParse (unknownJson.list [y]);
			}
		} else if (unknownJson.type == JSONObject.Type.STRING) {
			if (recordNextValue) {
				recordNextValue = false;
				bimInfo += "\n" + unknownJson.str + "\n\n";
			}
		} else if (unknownJson.type == JSONObject.Type.NUMBER) {
			if (recordNextValue) {
				recordNextValue = false;
				bimInfo += "\n" + unknownJson.n + "\n\n";
			}
		}
	}

}
