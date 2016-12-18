using UnityEngine;
using System.Collections.Generic;

namespace BGE.Forms
{
    public class CreatureColors : MonoBehaviour
    {
        public GameObject root;
        // Use this for initialization
        void Start()
        {
            List<Renderer> rs = Utilities.GetRenderersinChildrenRecursive(root);

            foreach (Renderer r in rs)
            {
                if (r.gameObject.layer == 10)
                {
                    r.material.color = Palette.Random();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}