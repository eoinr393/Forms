﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace BGE.Forms
{
    public class SchoolGenerator : School
    {
        public int boidCount;
        public GameObject prefab;

        public bool spawnInTopHemisphere;

        [Range(0, 1)]
        public float spread;
        
        SchoolGenerator()
        {
            boidCount = 200;

            spread = 1.0f;
        }

        void Awake()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }

            //Application.targetFrameRate = 20;
            int maxAudioBoids = 5;
            int audioBoids = 0;

            WorldGenerator wg = FindObjectOfType<WorldGenerator>();

            for (int i = 0; i < boidCount; i++)
            {
                GameObject fish = GameObject.Instantiate<GameObject>(prefab);
                Vector3 unit = UnityEngine.Random.insideUnitSphere;

                Vector3 pos = transform.position + unit*UnityEngine.Random.Range(0, radius*spread);
                if (wg != null)
                {
                    pos.y += wg.Sample(pos.x, pos.z) + 20;
                }
                
                fish.transform.position = pos;
                fish.transform.parent = transform;
                Boid boid = fish.GetComponentInChildren<Boid>();
                if (boid != null)
                {
                    boid.school = this;
                    boid.GetComponent<Constrain>().radius = radius;
                    boid.GetComponent<Constrain>().centreOnPosition = true;

                    boids.Add(boid);
                }

                AudioSource audioSource = fish.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    if (audioBoids < maxAudioBoids)
                    {
                        audioSource.enabled = true;
                        audioSource.loop = true;
                        audioSource.Play();
                        audioBoids++;
                    }
                    else
                    {
                        audioSource.enabled = false;
                    }
                }
            }
        }        
    }
}