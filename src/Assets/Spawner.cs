using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	public GameObject[] groups;
	// Use this for initialization
	void Start () 
	{
		spawnNext ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void spawnNext()
	{
		int i = Random.Range (0, groups.Length);
		//int j = Random.Range (0, groups.Length);
		//GameObject clone;

		//clone = Instantiate (groups [i], transform.position, Quaternion.identity);
		//clone.position += new Vector3 (1, 0, 0);
		if(!Grid.gameover)
		Instantiate (groups [i], transform.position + new Vector3(1,0,0), Quaternion.identity);
	}
}

