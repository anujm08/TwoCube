using UnityEngine;
using System.Collections;

public class Groupsingle : MonoBehaviour {
	float lastFall = 0;
	int value;
	public GameObject[] groups;
	
	// Use this for initialization
	void Start () 
	{
		foreach (Transform child in transform) 
		{
			if(child.name == "2")
				value = 2;
			else if(child.name == "4")
				value = 4;
			else if(child.name == "8")
				value = 8;
			else if(child.name == "16")
				value = 16;
			else if(child.name == "32")
				value = 32;
		}
	}
	
	bool isValidGridPos() 
	{        
		foreach (Transform child in transform) {
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
	void mergetheblocks()
	{
		foreach (Transform child in transform) 
		{
			Vector3 v = Grid.roundVec3 (child.position);
			if(((int) v.x!=0) && ((int) v.x!=3)  && ((int) v.z!=0) && ((int) v.z!=3) )
			{
				Destroy(Grid.grid[(int)v.x, (int) v.y,(int) v.z].parent.gameObject);
				Grid.grid[(int) v.x,(int) v.y,(int) v.z] = null;
				Grid.score = Grid.score- 2*value;
				Debug.Log ("Score : " + Grid.score);
				return;
			}

			Grid.values[(int)v.x, (int)v.y, (int)v.z]=value;
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
			else 
			{
				if((int) v.y == 5)
				{
					Grid.gameover = true;
					Debug.Log ("gameover");
				}
			}
		}
	}
	
	void updateGrid() {
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
	// Update is called once per frame
	void Update () 
	{
		// Move Left
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			// Modify position
			transform.position += new Vector3(0, 0, -1);
			
			// See if valid
			if (isValidGridPos())
				// Its valid. Update grid.
				updateGrid();
			else
				// Its not valid. revert.
				transform.position += new Vector3(0, 0, 1);
		}
		
		// Move Right
		else if (Input.GetKeyDown(KeyCode.LeftArrow)) 
		{
			// Modify position
			transform.position += new Vector3(0, 0, 1);
			
			// See if valid
			if (isValidGridPos())
				// It's valid. Update grid.
				updateGrid();
			else
				// It's not valid. revert.
				transform.position += new Vector3(0, 0, -1);
		}
		
		// Move Front
		else if (Input.GetKeyDown(KeyCode.UpArrow)) {
			// Modify position
			transform.position += new Vector3(1, 0, 0);
			
			// See if valid
			if (isValidGridPos())
				// It's valid. Update grid.
				updateGrid();
			else
				// It's not valid. revert.
				transform.position += new Vector3(-1, 0, 0);
		}
		
		// Move Back
		else if (Input.GetKeyDown(KeyCode.DownArrow)) {
			// Modify position
			transform.position += new Vector3(-1, 0, 0);
			
			// See if valid
			if (isValidGridPos())
				// It's valid. Update grid.
				updateGrid();
			else
				// It's not valid. revert.
				transform.position += new Vector3(1, 0, 0);
		}
		
		// Move Downwards and Fall
		else if (Input.GetKeyDown(KeyCode.Space) ||(Time.time - lastFall) >= 1)
		{
			// Modify position
			transform.position += new Vector3(0, -1, 0);
			lastFall = Time.time;
			// See if valid
			if (isValidGridPos()) {
				// It's valid. Update grid.
				updateGrid();
			} else {
				// It's not valid. revert.
				transform.position += new Vector3(0, 1, 0);
				mergetheblocks();
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
