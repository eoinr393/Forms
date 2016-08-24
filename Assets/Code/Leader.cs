using UnityEngine;
using System.Collections.Generic;

public class Leader : MonoBehaviour {

    public bool autoAssignFollowers = true;
    public List<GameObject> followers = new List<GameObject>();

    List<float> bondDistances = new List<float>();

    [Range(0, 1000)]
    public float bondDamping = 30.0f;

    [Range(0, 50)]
    public float angularBondDamping = 7.0f;
    
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

            DelayedMovement(prevFollower, follower, bondDistances[i]);            
        }
    }

    void DelayedMovement(Transform prevFollower, Transform follower, float bondDistance)
    {
        Follower f = follower.gameObject.GetComponent<Follower>();
        float bondDamping = this.bondDamping;
        float angularBondDamping = this.angularBondDamping;

        if (f != null)
        {
            bondDamping = f.bondDamping;
            angularBondDamping = f.angularBondDamping;
        }

        Vector3 wantedPosition = Utilities.TransformPointNoScale(new Vector3(0, 0, -bondDistance), prevFollower.transform);
        follower.transform.position = Vector3.Lerp(follower.transform.position, wantedPosition, Time.deltaTime * bondDamping);

        Quaternion wantedRotation = Quaternion.LookRotation(prevFollower.position - follower.transform.position, prevFollower.up);
        follower.transform.rotation = Quaternion.Slerp(follower.transform.rotation, wantedRotation, Time.deltaTime * angularBondDamping);
    }
}
