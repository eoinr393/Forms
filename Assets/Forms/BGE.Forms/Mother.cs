using UnityEngine;
using System.Collections.Generic;

namespace BGE.Forms
{
    public class Mother : MonoBehaviour
    {
        public int gridWidth = 10;
        public float density = 0.2f;
        public float playerMaxDistance = 5000;

        public GameObject[] prefabs;
        public GameObject player;
        private WorldGenerator wg;
        // Use this for initialization

        private List<GameObject> babies = new List<GameObject>();

        void Awake()
        {
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
            for (int row = 0; row < gridWidth; row++)
            {
                for (int col = 0; col < gridWidth; col++)
                {
                    Vector3 spawnPos = new Vector3();
                    spawnPos.x = -c + player.transform.position.x + (col*cellWidth);
                    spawnPos.z = -c + player.transform.position.z + (row*cellWidth);
                    if (wg != null)
                    {
                        spawnPos.y = wg.Sample(spawnPos.x, spawnPos.z) + 1000;
                    }

                    int dice = (int) Utilities.RandomRange(r, 0, prefabs.Length);
                    GameObject go = GameObject.Instantiate(prefabs[dice]);
                    go.transform.position = spawnPos;
                    babies.Add(go);
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