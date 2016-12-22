using UnityEngine;
using System.Collections.Generic;

namespace BGE.Forms
{
    public class CreatureColors : MonoBehaviour
    {
        public GameObject root;
        public int seed = 42; 
        // Use this for initialization
        void Start()
        {
            RecolorScene();
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                seed++;
                RecolorScene();
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                seed--;
                RecolorScene();
            }
            CreatureManager.Log("Color seed: " + seed);
        }

        private List<Renderer> renderers;

        List<Renderer> GetRenderers()
        {
            return renderers ?? (renderers = Utilities.GetRenderersinChildrenRecursive(root));
        }

        private TrailRenderer[] trailRenderers;

        TrailRenderer[] GetTrailRendereres()
        {
            return trailRenderers ?? (trailRenderers = FindObjectsOfType<TrailRenderer>());
        }

        void RecolorScene()
        {
            List<Renderer> rs = GetRenderers();
            Palette p = new Palette(seed, 10);

            foreach (Renderer r in rs)
            {

                if (r.materials[0].name.Contains("Trans"))
                {
                    continue;
                }

                // The square fish
                if (r.gameObject.layer == 9)
                {
                    r.material.color = p.colors[0];
                }

                // The big blues
                if (r.gameObject.layer == 10)
                {
                    r.material.color = p.colors[1];
                }

                // The Tenticle creatures
                if (r.gameObject.layer == 12)
                {
                    r.material.color = p.colors[2];
                }

                // The Formation
                if (r.gameObject.layer == 13)
                {
                    r.material.color = p.colors[3];
                }

                // The Flying Creatures
                if (r.gameObject.layer == 14)
                {
                    r.material.color = p.colors[4];
                }
                // The Tenticle Flowers
                if (r.gameObject.layer == 15)
                {
                    r.material.color = p.colors[5];
                }

                // The Sardines
                if (r.gameObject.layer == 16)
                {
                    r.material.color = p.colors[8];
                    TrailRenderer[] trs = GetTrailRendereres();
                    foreach (var tr in trs)
                    {
                        tr.material.SetColor("_TintColor", p.colors[9]);
                    }
                }

            }
            GameOfLifeTextureGenerator tg = FindObjectOfType<GameOfLifeTextureGenerator>();
            if (tg != null)
            {
                tg.backGround = p.colors[6];
                tg.foreGround = p.colors[7];
            }

            Camera[] cameras = FindObjectsOfType<Camera>();
            foreach (var c in cameras)
            {
                c.backgroundColor = p.colors[8];
                RenderSettings.fogColor = p.colors[8];
            }
        }
    }
}