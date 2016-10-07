﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BGE
{
    public class FPSController : MonoBehaviour
    {
        public GameObject mainCamera;
        public float speed = 50.0f;
        // Use this for initialization
        void Start()
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        void Yaw(float angle)
        {
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.up);
            transform.rotation = rot * transform.rotation;
        }

        void Roll(float angle)
        {
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = rot * transform.rotation;
        }

        void Pitch(float angle)
        {
            float invcosTheta1 = Vector3.Dot(transform.forward, Vector3.up);
            float threshold = 0.95f;
            if ((angle > 0 && invcosTheta1 < (-threshold)) || (angle < 0 && invcosTheta1 > (threshold)))
            {
                return;
            }

            // A pitch is a rotation around the right vector
            Quaternion rot = Quaternion.AngleAxis(angle, transform.right);

            transform.rotation = rot * transform.rotation;
        }

        void Walk(float units)
        {
            transform.position += mainCamera.transform.forward * units;
        }

        void Fly(float units)
        {
            transform.position += Vector3.up * units;
        }

        void Strafe(float units)
        {
            transform.position += mainCamera.transform.right * units;
            
        }

        // Update is called once per frame
        void Update()
        {
            float mouseX, mouseY;
            float speed = this.speed;

            float runAxis = 0; // Input.GetAxis("Run Axis");

            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (Input.GetKey(KeyCode.LeftShift) || runAxis != 0)
            {
                speed *= 10.0f;
            }

            if (Input.GetKey(KeyCode.W))
            {
                Walk(Time.deltaTime * speed);
            }

            if (Input.GetKey(KeyCode.S))
            {
                Walk(-Time.deltaTime * speed);
            }

            if (Input.GetKey(KeyCode.A))
            {
                Strafe(-Time.deltaTime * speed);
            }

            if (Input.GetKey(KeyCode.D))
            {
                Strafe(Time.deltaTime * speed);
            }
            
            if (Input.GetKey(KeyCode.R))
            {
                Fly(Time.deltaTime * speed);
            }
            if (Input.GetKey(KeyCode.F))
            {
                Fly(-Time.deltaTime * speed);
            }

            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");


            Yaw(mouseX);
            float contYaw = 0; // Input.GetAxis("Yaw Axis");
            float contPitch = 0; //Input.GetAxis("Pitch Axis");
            Yaw(contYaw);

            Pitch(-mouseY);
            Pitch(contPitch);
            
            float contWalk = 0; // Input.GetAxis("Walk Axis");
            float contStrafe = 0; // Input.GetAxis("Strafe Axis");
            if (Mathf.Abs(contWalk) > 0.1f)
            {
                Walk(-contWalk * speed * Time.deltaTime);
            }
            if (Mathf.Abs(contStrafe) > 0.1f)
            {
                Strafe(contStrafe * speed * Time.deltaTime);
            }
        }
    }
}