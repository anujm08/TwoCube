using UnityEngine;
using System.Collections;

public class Replaced : MonoBehaviour {

	public GameObject[] groups;
	int value;
	// Use this for initialization
	void Start () 
	{
		//Debug.Log ("Hello"+ transform.name);
		if(transform.name == "2(Clone)")
			value = 2;
		else if(transform.name == "4(Clone)")
			value = 4;
		else if(transform.name == "8(Clone)")
			value = 8;
		else if(transform.name == "16(Clone)")
			value = 16;
		else if(transform.name == "32(Clone)")
			value = 32;
		else if(transform.name == "64(Clone)")
			value = 64;
		else if(transform.name == "128(Clone)")
			value = 128;
		else if(transform.name == "256(Clone)")
			value = 256;
		else if(transform.name == "512(Clone)")
			value = 512;
		else if(transform.name == "1024(Clone)")
			value = 1024;
		else if(transform.name == "2048(Clone)")
			value = 2048;
		//Debug.Log ("Hello"+ value);
		Vector3 v = Grid.roundVec3 (transform.position);
		Grid.grid [(int)v.x, (int)v.y, (int)v.z] = transform;
		Grid.values [(int)v.x, (int)v.y, (int)v.z] = value;

		if((int)v.y == 0)
			return;
		if(Grid.values[(int)v.x, (int) v.y-1,(int) v.z] == value)
		{
			//value = 2*value;
			//replace a block by another block 
			// update value in grid
		
			Destroy(Grid.grid[(int)v.x, (int) v.y,(int) v.z].gameObject);
			Destroy(Grid.grid[(int)v.x, (int) v.y-1,(int) v.z].gameObject);
			Grid.score = Grid.score + 2*value;
			Debug.Log ("Score : " + Grid.score);
			Instantiate (groups[0], v + new Vector3(0,-1,0), Quaternion.identity);
			//Grid.grid[(int) v.x,(int) v.y,(int) v.z] = null; start of new object creted will do it
			Grid.grid[(int) v.x,(int) v.y,(int) v.z] = null;
			Grid.values[(int)v.x, (int)v.y, (int)v.z]=0;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
