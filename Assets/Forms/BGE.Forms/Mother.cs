using UnityEngine;
using System.Collections.Generic;

namespace BGE.Forms
{
    public class Mother : MonoBehaviour
    {
        public int gridWidth = 4;
        public float density = 0.2f;
        public float playerMaxDistance = 20000;

        public GameObject[] prefabs;
        public GameObject player;
        private WorldGenerator wg;
        // Use this for 

        private int dice;

        private List<GameObject> babies = new List<GameObject>();

        void Awake()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }
            player = GameObject.FindWithTag("Player");
            wg = FindObjectOfType<WorldGenerator>();

            if (player == null)
            {
                Debug.Log("No player!");
                return;
            }
            System.Random r = new System.Random(42);
            
            float cellWidth = playerMaxDistance/gridWidth;
            float c = playerMaxDistance/2.0f;
            int dice = (int)Utilities.RandomRange(r, 0, prefabs.Length);
            for (int row = 0; row < gridWidth; row++)
            {
                for (int col = 0; col < gridWidth; col++)
                {
                    if (r.NextDouble() > density)
                    {
                        break;
                    }
                    
                    Vector3 spawnPos = new Vector3();
                    spawnPos.x = -c + player.transform.position.x + (col*cellWidth);
                    spawnPos.z = -c + player.transform.position.z + (row*cellWidth);

                    if (Vector3.Distance(player.transform.position
                            , spawnPos) > playerMaxDistance)
                    {
                        break;
                    }

                    Vector3 cell = spawnPos/wg.cellSize;
                    if (wg != null)
                    {
                        spawnPos.y = Utilities.RandomRange(r, 200, 1000) + wg.Sample(cell.x, cell.z);
                    }
                    
                    GameObject go = GameObject.Instantiate(prefabs[dice % prefabs.Length]);
                    go.transform.position = spawnPos;
                    babies.Add(go);
                    go.transform.parent = this.transform;
                    dice++;
                }
            }
        }

        void Start () {
	        
        }
	
        // Update is called once per frame
        void Update () {
	
        }
    }
}