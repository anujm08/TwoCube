using UnityEngine;
using System.Collections;

public class Group : MonoBehaviour 
{
	float lastFall = 0;
	bool allowmovement1 = true;
	bool allowmovement2 = true;
	public GameObject[] groups;
	Transform[] childs = new Transform[2];

	int[] value = new int[2];
	// Use this for initialization
	void Start () 
	{
		childs[0] = this.gameObject.transform.GetChild(0);
		childs[1] = this.gameObject.transform.GetChild(1);
		for(int i = 0; i<2 ; i++) 
		{
			if(childs[i].name == "2")
				value[i] = 2;
			else if(childs[i].name == "4")
				value[i] = 4;
			else if(childs[i].name == "8")
				value[i] = 8;
			else if(childs[i].name == "16")
				value[i] = 16;
			else if(childs[i].name == "32")
				value[i] = 32;
		}
		//Debug.Log ("Hello"+ transform.name+ value[0]+value[1]);
		//Debug.Log ("Hello"+ transform.name);

	}
	
	bool isValidGridPos() 
	{        
		foreach (Transform child in transform) 
		{
			Vector3 v = Grid.roundVec3(child.position);
			
			// Not inside Border?
			if (!Grid.insideBorder(v))
				return false;
			
			// Block in grid cell (and not part of same group)?
			if (Grid.grid[(int)v.x, (int)v.y, (int)v.z] != null &&
			    Grid.grid[(int)v.x, (int)v.y, (int)v.z].parent != transform)
				return false;
		}
		return true;
	}
	
	bool isValid(int i)
	{
		Vector3 v = Grid.roundVec3(childs[i].position);

		if(!Grid.insideBorder(v))
			return false;

		if (Grid.grid[(int)v.x, (int)v.y, (int)v.z] != null)
			return false;
		return true;
	}
	
	void updateGrid() 
	{
		// Remove old children from grid
		for (int y = 0; y < Grid.h; ++y)
			for (int x = 0; x < Grid.b; ++x)
				for (int z = 0; z< Grid.l; ++z)
					if (Grid.grid[x, y, z] != null)
						if (Grid.grid[x, y, z].parent == transform)
							Grid.grid[x, y, z] = null;
		
		// Add new children to grid
		foreach (Transform child in transform) 
		{
			Vector3 v = Grid.roundVec3(child.position);
			Grid.grid[(int)v.x, (int)v.y, (int)v.z] = child;
		}        
	}

	void mergetheblocks(int i)
	{
		Vector3 v = Grid.roundVec3 (childs[i].position);
		childs [i].parent = null;
		if(((int) v.x!=0) && ((int) v.x!=3)  && ((int) v.z!=0) && ((int) v.z!=3) )
		{
			Destroy(Grid.grid[(int)v.x, (int) v.y,(int) v.z].gameObject);
			Grid.grid[(int) v.x,(int) v.y,(int) v.z] = null;
			Grid.score = Grid.score - 2*value[i];
			Debug.Log ("Score : " + Grid.score);
			return;
		}
		Grid.values[(int)v.x, (int)v.y, (int)v.z]=value[i];
		
		if((int)v.y == 0)
			return;
		if (Grid.values [(int)v.x, (int)v.y - 1, (int)v.z] == value [i]) {
			//value = 2*value;
			//replace a block by another block 
			// update value in grid
			Destroy (Grid.grid [(int)v.x, (int)v.y, (int)v.z].gameObject);
			Destroy (Grid.grid [(int)v.x, (int)v.y - 1, (int)v.z].gameObject);
			Grid.score = Grid.score + 2 * value [i];
			Debug.Log ("Score : " + Grid.score);
			Instantiate (groups [i], v + new Vector3 (0, -1, 0), Quaternion.identity);
			//Grid.grid[(int) v.x,(int) v.y,(int) v.z] = null; start of new object creted will do it
			Grid.grid [(int)v.x, (int)v.y, (int)v.z] = null;
			Grid.values [(int)v.x, (int)v.y, (int)v.z] = 0;
		} 
		else 
		{
			if((int) v.y == 5)
			{
				Grid.gameover = true;
				Debug.Log ("gameover");
			}
		}
	}

	// Update is called once per frame
	//bool allowmovement=true;
	void Update () 
	{
		// Move Left
		//if(allowmovement==true)
		if (allowmovement1 && allowmovement2) 
		{
			if (Input.GetKeyDown (KeyCode.RightArrow))
			{
				// Modify position
				transform.position += new Vector3 (0, 0, -1);
				
				// See if valid
				if (isValidGridPos ())
					// Its valid. Update grid.
					updateGrid ();
				else
					// Its not valid. revert.
					transform.position += new Vector3 (0, 0, 1);
			}
			
			// Move Right
			else if (Input.GetKeyDown (KeyCode.LeftArrow)) 
			{
				// Modify position
				transform.position += new Vector3 (0, 0, 1);
				
				// See if valid
				if (isValidGridPos ())
					// It's valid. Update grid.
					updateGrid ();
				else
					// It's not valid. revert.
					transform.position += new Vector3 (0, 0, -1);
			}
			
			// Move Front
			else if (Input.GetKeyDown (KeyCode.UpArrow)) 
			{
				// Modify position
				transform.position += new Vector3 (1, 0, 0);
				
				// See if valid
				if (isValidGridPos ())
					// It's valid. Update grid.
					updateGrid ();
				else
					// It's not valid. revert.
					transform.position += new Vector3 (-1, 0, 0);
			}
			
			// Move Back
			else if (Input.GetKeyDown (KeyCode.DownArrow)) 
			{
				// Modify position
				transform.position += new Vector3 (-1, 0, 0);
				
				// See if valid
				if (isValidGridPos ())
					// It's valid. Update grid.
					updateGrid ();
				else
					// It's not valid. revert.
					transform.position += new Vector3 (1, 0, 0);
			}
			
			// Move Downwards and Fall
			else if (Input.GetKeyDown (KeyCode.Space) || (Time.time - lastFall) >= 1) 
			{
				// Modify position
				transform.position += new Vector3 (0, -1, 0);
				lastFall = Time.time;
				// See if valid
				if (isValidGridPos ()) 
				{
					// It's valid. Update grid.
					updateGrid ();
				} 
				else 
				{
					// It's not valid. revert.
					transform.position += new Vector3 (0, 1, 0);
					//Transform child1 = this.gameObject.transform.GetChild(0);
					//Transform child2 = this.gameObject.transform.GetChild(1);
					foreach (Transform child in transform)
					{
						child.position += new Vector3(0, -1, 0);
					}
					if (!isValid(0))
					{
						// It's valid. Update grid.
						childs[0].position += new Vector3(0,1,0);
						allowmovement1=false;
						mergetheblocks(0);
					}
					if(!isValid(1))
					{
						childs[1].position += new Vector3(0,1,0);
						allowmovement2=false;
						mergetheblocks(1);
					}
					updateGrid();
					if(!(allowmovement1||allowmovement2))
					{
						// Spawn next Group
						FindObjectOfType<Spawner> ().spawnNext ();
						
						// Disable script
						enabled = false;

					}
					// It's not valid. revert.
					// Clear filled horizontal lines
					//Grid.deleteFullRows();
					
					// Spawn next Group
					//FindObjectOfType<Spawner>().spawnNext();
					
					// Disable script
					//enabled = false;	
					// Clear filled horizontal lines
					//Grid.deleteFullRows();
					
					
				}
			}
		}
		else if(allowmovement1)
		{
			if (Input.GetKeyDown(KeyCode.Space) ||(Time.time - lastFall) >= 0.03)
			{
				// Modify position
				//Transform child1 = this.gameObject.transform.GetChild(0);
				childs[0].position +=new Vector3(0,-1,0);
				lastFall = Time.time;
				// See if valid
				if (isValid(0))
				{
					// It's valid. Update grid.
					updateGrid();
				}
				else 
				{
					// It's not valid. revert.
					childs[0].position += new Vector3(0, 1, 0);
					allowmovement1=false;	
					mergetheblocks(0);
					// Clear filled horizontal lines
					//Grid.deleteFullRows();
					
					// Spawn next Group
					FindObjectOfType<Spawner>().spawnNext();
					
					// Disable script
					enabled = false;
				}	
			}
		}
		else if(allowmovement2)
		{
			if (Input.GetKeyDown(KeyCode.Space) ||(Time.time - lastFall) >= 0.03)
			{
				// Modify position
				//Transform child2 = this.gameObject.transform.GetChild(1);
				childs[1].position +=new Vector3(0,-1,0);
				lastFall = Time.time;
				// See if valid
				if (isValid(1))
				{
					// It's valid. Update grid.
					updateGrid();
				}
				else 
				{
					// It's not valid. revert.
					childs[1].position += new Vector3(0, 1, 0);
					allowmovement2=false;	
					mergetheblocks(1);
					// Clear filled horizontal lines
					//Grid.deleteFullRows();
					
					// Spawn next Group
					FindObjectOfType<Spawner>().spawnNext();
					
					// Disable script
					enabled = false;
				}	
			}
		}
	}
}
