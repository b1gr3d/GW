using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
public class UtilityParser : MonoBehaviour {
	public string[] parse(string str) {
		if (str == null || str == "") {
			return null;
		}
		string[] tokens =  str.Split(' ');
		return tokens;
	}
}
