using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour
{
    public GameObject placementIndicator;
    public GameObject objectToPlace;
    public Camera arCamera;
    public GameObject objects;

    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private List<GameObject> furnitures = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        SSTools.ShowMessage("Start", SSTools.Position.top, SSTools.Time.twoSecond);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    public void SetObjectToPlace(GameObject obj)
    {
        objectToPlace = obj;
    }

    public void PlaceObject()
    {
        var newObject = Instantiate(objectToPlace, placementPose.position, placementPose.rotation * objectToPlace.transform.rotation);
        newObject.transform.parent = objects.transform;
        furnitures.Add(newObject);
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            string position = placementPose.position.ToString();
            string rotation = " R: " + placementPose.rotation.ToString();
            SSTools.ShowMessage(position + rotation, SSTools.Position.top, SSTools.Time.oneSecond);
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
       // Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        Vector3 screenCenter = arCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        RaycastHit rayHit;

        //if (Physics.Raycast(Camera.current.ScreenPointToRay(screenCenter), out rayHit))
        if (Physics.Raycast(arCamera.ScreenPointToRay(screenCenter), out rayHit))
        {
           // SSTools.ShowMessage("Raycast Hit", SSTools.Position.top, SSTools.Time.oneSecond);

            if (rayHit.collider.gameObject.CompareTag("PlacementPlane"))
            {
                //SSTools.ShowMessage("Compare Tag", SSTools.Position.top, SSTools.Time.oneSecond);
                placementPoseIsValid = true;

                placementPose.position = rayHit.point;

                Vector3 cameraForward = arCamera.transform.forward;
                Vector3 cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
                placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            }
        }
    }

    public void RemoveObject()
    {
        Touch touch = Input.GetTouch(0);
        RaycastHit rayHit;

        if (Physics.Raycast(arCamera.ScreenPointToRay(touch.position), out rayHit))
        {
            if (!rayHit.collider.gameObject.CompareTag("PlacementPlane"))
            {
                Destroy(rayHit.transform.gameObject);
            }
        }
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
