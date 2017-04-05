using UnityEngine;

public static class Utilities {

	public static float AngleBetweenPoints (Vector2 a, Vector2 b) {
		return Mathf.Atan2 (a.y - b.y, a.x - b.x) * Mathf.Rad2Deg; 
	}

}
