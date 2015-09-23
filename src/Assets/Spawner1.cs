using UnityEngine;
using System.Collections;

public class Spawner1 : MonoBehaviour {
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
		int i = Random.Range(0,groups.Length);
		int j = Random.Range(0,groups.Length);
		Instantiate(groups[i], transform.position, Quaternion.identity);
		//Instantiate(groups[j], transform.position + new Vector3(1,0,0), Quaternion.identity);
	}
}