﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;


public class SchoolGenerator : School
{
    public int boidCount;
    public GameObject boidPrefab;

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
        //Application.targetFrameRate = 20;
        int maxAudioBoids = 5;
        int audioBoids = 0;

        for (int i = 0; i < boidCount; i++)
        {
            Boid boid = GameObject.Instantiate<GameObject>(boidPrefab).GetComponent<Boid>();
            Vector3 unit = UnityEngine.Random.insideUnitSphere;
            if (spawnInTopHemisphere)
            {
                unit.y = Mathf.Abs(unit.y);
            }
            boid.transform.position = transform.position + unit * UnityEngine.Random.Range(0, radius * spread);
            boid.transform.parent = transform;
            boid.school = this;
            boid.testField = "AAA";
            boids.Add(boid);
            AudioSource audioSource = boid.GetComponent<AudioSource>();
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