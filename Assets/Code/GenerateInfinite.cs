﻿using UnityEngine;
using System.Collections;

class Tile
{
	public GameObject theTile;
	public float creationTime;

	public Tile(GameObject t, float ct)
	{
		theTile = t;
		creationTime = ct;
	}
}

class GeneratedMesh
{
    public Vector3[] vertices;
    public Vector3[] normals;
    public Vector2[] uvs;
    public Color[] colours;
    public int[] triangles;
}


public class GenerateInfinite : MonoBehaviour {
	public GameObject player;

	int planeSize = 10;
	int halfTilesX = 5;
	int halfTilesZ = 5;
	
	Vector3 startPos;

	Hashtable tiles = new Hashtable();

    Sampler[] samplers;

    void Awake()
    {
        samplers = GetComponents<Sampler>();
        if (samplers == null)
        {
            Debug.Log("Sampler is null! Add a sampler to the NoiseForm");
        }
        Random.seed = 42;
    }


    // Use this for initialization
    void Start () 
	{
		this.gameObject.transform.position = Vector3.zero;
		startPos = Vector3.zero;

		float updateTime = Time.realtimeSinceStartup;

		for(int x = -halfTilesX; x < halfTilesX; x++)
		{
			for(int z = -halfTilesZ ; z < halfTilesZ ; z++)
			{
				Vector3 pos = new Vector3((x * planeSize+startPos.x),
											0,
										  (z * planeSize+startPos.z));
				GameObject t = (GameObject) GenerateTile(pos); // Instantiate(plane, pos, 
					               //Quaternion.identity);

				string tilename = "Tile_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString();
				t.name = tilename;
				Tile tile = new Tile(t, updateTime);
				tiles.Add(tilename, tile);
			}
		}
	}

    GameObject GenerateTile(Vector3 position)
    {

        GameObject tile = new GameObject();
        tile.transform.parent = this.transform;
        MeshRenderer renderer = tile.AddComponent<MeshRenderer>();
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = true;
        Mesh mesh = tile.AddComponent<MeshFilter>().mesh;
        mesh.Clear();
        tile.AddComponent<MeshCollider>();        
        tile.transform.position = position;

        int verticesPerSegment = 6;

        int vertexCount = verticesPerSegment * ((int)planeSize) * ((int)planeSize);

        GeneratedMesh gm = new GeneratedMesh();

        gm.vertices = new Vector3[vertexCount];
        gm.normals = new Vector3[vertexCount];
        gm.uvs = new Vector2[vertexCount];
        gm.triangles = new int[vertexCount];

        gm.colours = new Color[vertexCount];

        int vertex = 0;

        // What cell is x and z for the bottom left of this tile in world space
        Vector3 tileBottomLeft = new Vector3();
        tileBottomLeft.x = -(planeSize) / 2;
        tileBottomLeft.z = -(planeSize) / 2;

        for (int z = 0; z < planeSize; z++)
        {
            for (int x = 0; x < planeSize; x++)
            {
                int startVertex = vertex;
                // Calculate some stuff
                Vector3 cellBottomLeft = tileBottomLeft + new Vector3(x * planeSize, 0, z * planeSize);
                Vector3 cellTopLeft = tileBottomLeft + new Vector3(x * planeSize, 0, (z + 1) * planeSize);
                Vector3 cellTopRight = tileBottomLeft + new Vector3((x + 1) * planeSize, 0, (z + 1) * planeSize);
                Vector3 celBottomRight = tileBottomLeft + new Vector3((x + 1) * planeSize, 0, z * planeSize);

                // Add all the samplers together to make the height
                Vector3 cellWorldCoords = position + tileBottomLeft + new Vector3(x * planeSize, 0, z * planeSize);
                foreach (Sampler sampler in samplers)
                {
                    cellBottomLeft.y += sampler.Sample(cellWorldCoords.x, cellWorldCoords.z);
                    cellTopLeft.y += sampler.Sample(cellWorldCoords.x, cellWorldCoords.z + planeSize);
                    cellTopRight.y += sampler.Sample(cellWorldCoords.x + planeSize, cellWorldCoords.z + planeSize);
                    celBottomRight.y += sampler.Sample(cellWorldCoords.x + planeSize, cellWorldCoords.z);
                }

                // Make the vertices
                gm.vertices[vertex++] = cellBottomLeft;
                gm.vertices[vertex++] = cellTopLeft;
                gm.vertices[vertex++] = cellTopRight;
                gm.vertices[vertex++] = cellTopRight;
                gm.vertices[vertex++] = celBottomRight;
                gm.vertices[vertex++] = cellBottomLeft;

                // Make the normals, UV's and triangles                
                for (int i = 0; i < 6; i++)
                {
                    int vertexIndex = startVertex + i;
                    gm.triangles[vertexIndex] = vertexIndex;
                    gm.uvs[vertexIndex] = new Vector2(x / planeSize, z / planeSize);
                }
            }
        }
        mesh.vertices = gm.vertices;
        mesh.uv = gm.uvs;
        mesh.triangles = gm.triangles;
        return tile;
    }

		// Update is called once per frame
	void Update () 
	{
		//determine how far the player has moved since last terrain update
		int xMove = (int)(player.transform.position.x - startPos.x);
		int zMove = (int)(player.transform.position.z - startPos.z);
		
		if(Mathf.Abs(xMove) >= planeSize || Mathf.Abs(zMove) >= planeSize)
		{
			float updateTime = Time.realtimeSinceStartup;
			
			//force integer position and round to nearest tilesize
			int playerX = (int)(Mathf.Floor(player.transform.position.x/planeSize)*planeSize);
			int playerZ = (int)(Mathf.Floor(player.transform.position.z/planeSize)*planeSize);

			for(int x = -halfTilesX; x < halfTilesX; x++)
			{
				for(int z = -halfTilesZ ; z < halfTilesZ ; z++)
				{
					Vector3 pos = new Vector3((x * planeSize + playerX),
												0,
											  (z * planeSize + playerZ));
					
					string tilename = "Tile_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString();
					
					if(!tiles.ContainsKey(tilename))
					{
                        GameObject t = GenerateTile(pos);//(GameObject) Instantiate(plane, pos, 
						               //Quaternion.identity);
						t.name = tilename;
						Tile tile = new Tile(t, updateTime);
						tiles.Add(tilename, tile);
					}
					else
					{
						(tiles[tilename] as Tile).creationTime = updateTime;
					}
				}
			}

			//destroy all tiles not just created or with time updated
			//and put new tiles and tiles to be kepts in a new hashtable
			Hashtable newTerrain = new Hashtable();
			foreach(Tile tls in tiles.Values)
			{
				if(tls.creationTime != updateTime)
				{
					//delete gameobject
					Destroy(tls.theTile);
				}
				else
				{
					newTerrain.Add(tls.theTile.name, tls);
				}
			}
			//copy new hashtable contents to the working hashtable
			tiles = newTerrain;

			startPos = player.transform.position;
		}

	}
}
