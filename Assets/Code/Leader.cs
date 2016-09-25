using UnityEngine;
using System.Collections.Generic;

struct JointParam
{
    public float bondDamping;
    public float angularBondDamping;
    public int joint;

    public JointParam(int joint, float bondDamping, float angularBondDamping)
    {
        this.joint = joint;
        this.bondDamping = bondDamping;
        this.angularBondDamping = angularBondDamping;
    }
}

public class Leader : MonoBehaviour {

    public bool autoAssignFollowers = true;
    public List<GameObject> followers = new List<GameObject>();

    List<float> bondDistances = new List<float>();

    Dictionary<int, JointParam> jointParams = new Dictionary<int, JointParam>();

    public float bondDamping;
    public float angularBondDamping;

    void Start()
    {
        Transform prevFollower;
        bondDistances.Clear();

        if (autoAssignFollowers)
        {
            followers.Clear();
            for (int i = 0; i < transform.parent.childCount; i++)
            {
                GameObject child = transform.parent.GetChild(i).gameObject;
                if (child != this.gameObject)
                {
                    followers.Add(child);
                }
            }
        }

        for (int i = 0; i < followers.Count; i++)
        {
            if (i == 0)
            {
                prevFollower = this.transform;
            }
            else
            {
                prevFollower = followers[i - 1].transform;
            }

            Transform follower = followers[i].transform;
            bondDistances.Add(Vector3.Distance(prevFollower.position, follower.position));
        }
    }
    
	void FixedUpdate ()
    {
        Transform prevFollower;
        for (int i = 0 ; i < followers.Count; i++)
        {
            if (i == 0)
            {
                prevFollower = this.transform;
            }
            else
            {
                prevFollower = followers[i - 1].transform;
            }

            Transform follower = followers[i].transform;

            DelayedMovement(prevFollower, follower, bondDistances[i], i);            
        }
    }

    

    void DelayedMovement(Transform prevFollower, Transform follower, float bondDistance, int i)
    {
        /*
        float bondDamping;
        float angularBondDamping;

        //if (jointParams[i] == null)
        {
            bondDamping = this.bondDamping;
            angularBondDamping = this.angularBondDamping;
        }
        else
        {

        }


        Vector3 wantedPosition = Utilities.TransformPointNoScale(new Vector3(0, 0, -bondDistance), prevFollower.transform);
        follower.transform.position = Vector3.Lerp(follower.transform.position, wantedPosition, Time.deltaTime * bondDamping);

        Quaternion wantedRotation = Quaternion.LookRotation(prevFollower.position - follower.transform.position, prevFollower.up);
        follower.transform.rotation = Quaternion.Slerp(follower.transform.rotation, wantedRotation, Time.deltaTime * angularBondDamping);
        */
    }
}
