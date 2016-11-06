﻿using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class JointParam
{
    public float bondDamping = 25;
    public float angularBondDamping = 2;    

    public JointParam(float bondDamping, float angularBondDamping)
    {
        this.bondDamping = bondDamping;
        this.angularBondDamping = angularBondDamping;
    }
}

public class SpineAnimator : MonoBehaviour {
    public bool autoAssignBones = true;    

    public List<GameObject> bones = new List<GameObject>();

    List<Vector3> bondOffsets = new List<Vector3>();
    List<Quaternion> startRotations = new List<Quaternion>();

    public List<JointParam> jointParams = new List<JointParam>();

    public float bondDamping;
    public float angularBondDamping;

    public Vector3 centerOfMass;
    public Quaternion averageRotation;

    void Start()
    {
        Transform prevFollower;
        bondOffsets.Clear();

        if (autoAssignBones)
        {
            bones.Clear();
            startRotations.Add(transform.rotation);
            Transform parent = (transform.parent.childCount > 1) ? transform.parent : transform.parent.parent;
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                GameObject child = transform.parent.GetChild(i).gameObject;
                if (child != this.gameObject)
                {
                    bones.Add(child);
                    startRotations.Add(child.transform.rotation);
                }
            }
        }

        for (int i = 0; i < bones.Count; i++)
        {
            if (i == 0)
            {
                prevFollower = this.transform;
            }
            else
            {
                prevFollower = bones[i - 1].transform;
            }

            Transform follower = bones[i].transform;
            Vector3 offset = follower.position - prevFollower.position;
            offset = Quaternion.Inverse(prevFollower.transform.rotation) * offset;
            bondOffsets.Add(offset);

        }
    }
    
	void FixedUpdate ()
    {
        centerOfMass = Vector3.zero ; 
        Transform prevFollower;
        for (int i = 0 ; i < bones.Count; i++)
        {
            if (i == 0)
            {
                prevFollower = this.transform;
            }
            else
            {
                prevFollower = bones[i - 1].transform;
            }

            Transform follower = bones[i].transform;

            DelayedMovement(prevFollower, follower, bondOffsets[i], i);
            centerOfMass += follower.position;
        }
        centerOfMass /= bones.Count;
    }
    
    void DelayedMovement(Transform target, Transform follower, Vector3 bondOffset, int i)
    {
        float bondDamping;
        float angularBondDamping;

        if (jointParams.Count > i)
        {
            JointParam jp = jointParams[i];
            bondDamping = jp.bondDamping;
            angularBondDamping = jp.angularBondDamping;
        }
        else
        {
            bondDamping = this.bondDamping;
            angularBondDamping = this.angularBondDamping;
        }

        
        Vector3 wantedPosition = Utilities.TransformPointNoScale(bondOffset, target.transform);
        follower.transform.position = Vector3.Lerp(follower.transform.position, wantedPosition, Time.deltaTime * bondDamping);

        Quaternion wantedRotation = Quaternion.LookRotation(target.position - follower.transform.position, target.up);
        //wantedRotation = startRotations[i + 1] * wantedRotation;

        wantedRotation = target.transform.rotation;
        follower.transform.rotation = Quaternion.Slerp(follower.transform.rotation, wantedRotation, Time.deltaTime * angularBondDamping);
    }
}
