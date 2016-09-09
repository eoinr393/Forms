using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class JointParam
{
    public float bondDamping;
    public float angularBondDamping;    

    public JointParam(float bondDamping, float angularBondDamping)
    {
        this.bondDamping = bondDamping;
        this.angularBondDamping = angularBondDamping;
    }
}

public class SpineAnimator : MonoBehaviour {

    public bool autoAssignBones = true;
    
    public List<GameObject> bones = new List<GameObject>();

    List<float> bondDistances = new List<float>();

    public List<JointParam> jointParams = new List<JointParam>();

    public float bondDamping;
    public float angularBondDamping;

    void Start()
    {
        Transform prevFollower;
        bondDistances.Clear();

        if (autoAssignBones)
        {
            bones.Clear();
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                GameObject child = transform.parent.GetChild(i).gameObject;
                if (child != this.gameObject)
                {
                    bones.Add(child);
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
            bondDistances.Add(Vector3.Distance(prevFollower.position, follower.position));
        }
    }
    
	void FixedUpdate ()
    {
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

            DelayedMovement(prevFollower, follower, bondDistances[i], i);            
        }
    }

    

    void DelayedMovement(Transform prevFollower, Transform follower, float bondDistance, int i)
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


        Vector3 wantedPosition = Utilities.TransformPointNoScale(new Vector3(0, 0, -bondDistance), prevFollower.transform);
        follower.transform.position = Vector3.Lerp(follower.transform.position, wantedPosition, Time.deltaTime * bondDamping);

        Quaternion wantedRotation = Quaternion.LookRotation(prevFollower.position - follower.transform.position, prevFollower.up);
        follower.transform.rotation = Quaternion.Slerp(follower.transform.rotation, wantedRotation, Time.deltaTime * angularBondDamping);
    }
}
