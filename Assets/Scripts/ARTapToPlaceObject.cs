using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARTapToPlaceObject : MonoBehaviour {

    public GameObject placementIndicator;
    public GameObject objectToPlace;
    public Camera ARCamera;
    
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private List<GameObject> furnitures = new List<GameObject>();

	void Start ()
    {
	}

	void Update ()
    {

	}

    public void SetObjectToPlace(GameObject obj)
    {
        objectToPlace = obj;
    }

    public void PlaceObject()
    {
        GameObject createdObject = Instantiate(objectToPlace, placementPose.position, placementPose.rotation * objectToPlace.transform.rotation);
        furnitures.Add(createdObject);
    }
    

    public void Reset()
    {
        foreach (GameObject furniture in furnitures)
        {
            Destroy(furniture);
        }

        furnitures = new List<GameObject>();
    }
}
