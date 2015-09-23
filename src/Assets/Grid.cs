using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	public static int l = 5;
	public static int h = 15;
	public static int b = 5;
	public static Transform[,,] grid = new Transform[l,h,b];
	public static int[,,] values = new int[l,h,b];
	public static int score = 0;
	public static bool gameover = false;

	public static Vector3 roundVec3(Vector3 v)
	{
		return new Vector3(Mathf.Round(v.x),
		                   Mathf.Round(v.y),
		                   Mathf.Round(v.z));
	}
	
	public static bool insideBorder(Vector3 pos)
	{
		return ((int)pos.x >= 0 &&
		        (int)pos.x <= 3 &&
		        (int)pos.z >= 0 &&
		        (int)pos.z <= 3 &&
		        (int)pos.y >= 0);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
}
