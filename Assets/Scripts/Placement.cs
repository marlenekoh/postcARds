using Lean.Touch;
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
    private List<GameObject> assets = new List<GameObject>();
    private GameObject selectedObject;

    // Start is called before the first frame update
    void Start()
    {
        //SSTools.ShowMessage("Start", SSTools.Position.top, SSTools.Time.twoSecond);
        Debug.Log("Session ID: " + Session.id);
        selectedObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        //foreach (Transform child in objects.transform)
        //{
        //    string position = child.gameObject.transform.position.ToString();
        //    string rotation = child.gameObject.transform.rotation.ToString();
        //    string scale = child.gameObject.transform.localScale.ToString();
        //    Debug.Log(child.gameObject.tag + ": " + position + ", " + rotation + ", " + scale);
        //}
    }
    
    public void SetObjectToPlace(GameObject obj)
    {
        objectToPlace = obj;
    }

    public void PlaceObject()
    {
        if (placementPoseIsValid)
        {
            //var newObject = Instantiate(objectToPlace, placementPose.position, placementPose.rotation * objectToPlace.transform.rotation);
            GameObject newObject = Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
            newObject.transform.parent = objects.transform;
            assets.Add(newObject);
            setUpLeanTouchScripts(newObject);
        }
    }

    private void setUpLeanTouchScripts(GameObject leantouchObject)
    {
        LeanSelectable leanSelectable = leantouchObject.AddComponent<LeanSelectable>();
        leanSelectable.DeselectOnUp = false;
        leanSelectable.HideWithFinger = false;
        leanSelectable.IsolateSelectingFingers = false;

        LeanScale leanScale = leantouchObject.AddComponent<LeanScale>();
        leanScale.IgnoreStartedOverGui = true;
        leanScale.IgnoreIsOverGui = true;
        leanScale.RequiredFingerCount = 2;
        leanScale.RequiredSelectable = leanSelectable;

        LeanRotateCustomAxis leanRotateCustomAxis = leantouchObject.AddComponent<LeanRotateCustomAxis>();
        leanRotateCustomAxis.IgnoreStartedOverGui = true;
        leanRotateCustomAxis.IgnoreIsOverGui = true;
        leanRotateCustomAxis.RequiredFingerCount = 2;
        leanRotateCustomAxis.RequiredSelectable = leanSelectable;
        leanRotateCustomAxis.Axis = new Vector3(0, -1, 0);
        leanRotateCustomAxis.Space = Space.Self;

        LeanTranslateVertical leanTranslateVertical = leantouchObject.AddComponent<LeanTranslateVertical>();
        leanTranslateVertical.IgnoreStartedOverGui = true;
        leanTranslateVertical.IgnoreIsOverGui = true;
        leanTranslateVertical.RequiredFingerCount = 1;
        leanTranslateVertical.RequiredSelectable = leanSelectable;
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            string position = placementPose.position.ToString();
            string rotation = " R: " + placementPose.rotation.ToString();
            //SSTools.ShowMessage(position + rotation, SSTools.Position.top, SSTools.Time.oneSecond);
            //Debug.Log(position + rotation);
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
        Vector3 screenCenter = arCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        RaycastHit rayHit;

        //SSTools.ShowMessage("Update Pose", SSTools.Position.top, SSTools.Time.oneSecond);
        
        if (Physics.Raycast(arCamera.ScreenPointToRay(screenCenter), out rayHit))
        {
            //SSTools.ShowMessage("Raycast Hit", SSTools.Position.top, SSTools.Time.oneSecond);

            if (rayHit.collider.gameObject.CompareTag("PlacementPlane"))
            {
                //SSTools.ShowMessage("Compare Tag", SSTools.Position.top, SSTools.Time.oneSecond);
                placementPoseIsValid = true;

                placementPose.position = rayHit.point;

                //Vector3 cameraForward = arCamera.transform.forward;
                //Vector3 cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
                //placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            }
        }
    }

    public void RemoveObject()
    {
        Touch touch = Input.GetTouch(0);
        RaycastHit rayHit;

        if (Physics.Raycast(arCamera.ScreenPointToRay(touch.position), out rayHit))
        {
            if (!rayHit.collider.gameObject.CompareTag("PlacementPlane") && !rayHit.collider.gameObject.CompareTag("PlacementIndicator"))
            {
                Destroy(rayHit.transform.gameObject);
            }
        }
    }

    public void SelectObject()
    {
        Touch touch = Input.GetTouch(0);
        RaycastHit rayHit;

        if (Physics.Raycast(arCamera.ScreenPointToRay(touch.position), out rayHit))
        {
            if (!rayHit.collider.gameObject.CompareTag("PlacementPlane") && !rayHit.collider.gameObject.CompareTag("PlacementIndicator"))
            {
                GameObject newSelectedObject = rayHit.collider.gameObject;
                DeselectAllObjects();
                if (selectedObject.Equals(newSelectedObject))
                {
                    selectedObject = null;
                } else {
                    selectedObject = newSelectedObject;
                    Material m = selectedObject.GetComponent<Renderer>().material;
                    m.shader = Shader.Find("SilhouetteDiffuse");
                    m.SetColor("Main Color", Color.white);
                    m.SetColor("Outline Color", Color.yellow);
                    m.SetFloat("Outline width", 0.03f);
                }
            }
        }
    }

    public void DeselectAllObjects()
    {
        foreach (GameObject asset in assets)
        {
            asset.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Texture");
        }
    }

    public void Reset()
    {
        foreach (GameObject furniture in assets)
        {
            Destroy(furniture);
        }

        assets = new List<GameObject>();
    }
}
