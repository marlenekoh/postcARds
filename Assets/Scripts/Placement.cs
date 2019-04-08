using cakeslice;
using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour
{
    private static Placement placementSingleton;
    public GameObject placementIndicator;
    public GameObject objectToPlace;
    public Camera arCamera;
    public GameObject objects;
    public bool debug;
    public GameObject deleteButton;

    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private List<GameObject> assets = new List<GameObject>();
    public GameObject selectedObject;
    private Material[] originalMaterials;

    // Start is called before the first frame update
    void Start()
    {
        //SSTools.ShowMessage("Start", SSTools.Position.top, SSTools.Time.twoSecond);
        Debug.Log("Session ID: " + Session.id);
        selectedObject = null;
        if (placementSingleton == null)
        {
            placementSingleton = this;
        }
        deleteButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlacementPose();
        if (debug)
        {
            placementPoseIsValid = true;
        }
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
            setUpOutlineScripts(newObject);
        }
    }

    private void setUpLeanTouchScripts(GameObject leantouchObject)
    {
        LeanSelectable leanSelectable = leantouchObject.AddComponent<LeanSelectable>();
        leanSelectable.DeselectOnUp = false;
        leanSelectable.HideWithFinger = false;
        leanSelectable.IsolateSelectingFingers = false;
        leanSelectable.placement = placementSingleton;

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

        LeanTranslateY leanTranslateY = leantouchObject.AddComponent<LeanTranslateY>();
        leanTranslateY.IgnoreStartedOverGui = true;
        leanTranslateY.IgnoreIsOverGui = true;
        leanTranslateY.RequiredFingerCount = 3;
        leanTranslateY.RequiredSelectable = leanSelectable;

        LeanTranslateXZ leanTranslateXZ = leantouchObject.AddComponent<LeanTranslateXZ>();
        leanTranslateXZ.IgnoreStartedOverGui = true;
        leanTranslateXZ.IgnoreIsOverGui = true;
        leanTranslateXZ.RequiredFingerCount = 1;
        leanTranslateXZ.RequiredSelectable = leanSelectable;
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

        //SSTools.ShowMessage("Update Pose", SSTools.Position.top, SSTools.Time.oneSecond);

        RaycastHit[] hits = Physics.RaycastAll(arCamera.ScreenPointToRay(screenCenter), float.MaxValue);

        if (hits.Length > 0)
        {
            //SSTools.ShowMessage("Raycast Hit", SSTools.Position.top, SSTools.Time.oneSecond);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.CompareTag("PlacementPlane"))
                {
                    //Debug.Log("hit plane");
                    //SSTools.ShowMessage("Compare Tag", SSTools.Position.top, SSTools.Time.oneSecond);
                    placementPoseIsValid = true;
                    placementPose.position = hits[i].point;

                    break;
                }
                else
                {
                    placementPoseIsValid = false;
                }
            }
        }
        else
        {
            placementPoseIsValid = false;
        }
    }

    public void RemoveObject()
    {
        assets.Remove(selectedObject);
        Destroy(selectedObject);
        selectedObject = null;
    }

    //public void SelectObject()
    //{
    //    if (Input.touchCount > 0)
    //    {
    //        Touch touch = Input.GetTouch(0);
    //        RaycastHit rayHit;

    //        if (Physics.Raycast(arCamera.ScreenPointToRay(touch.position), out rayHit))
    //        {
    //            if (!rayHit.collider.gameObject.CompareTag("PlacementPlane") && !rayHit.collider.gameObject.CompareTag("PlacementIndicator"))
    //            {
    //                deleteButton.SetActive(true);
    //                GameObject newSelectedObject = rayHit.collider.gameObject;
    //                if (selectedObject == null || !selectedObject.Equals(newSelectedObject))
    //                {
    //                    selectedObject = newSelectedObject;
    //                    enableOutlineScripts(selectedObject);
    //                }
    //            }
    //        }
    //    }
    //}


    //public void DeselectObject()
    //{
    //    if (Input.touchCount > 0)
    //    {
    //        Touch touch = Input.GetTouch(0);
    //        RaycastHit rayHit;

    //        if (Physics.Raycast(arCamera.ScreenPointToRay(touch.position), out rayHit))
    //        {
    //            if (!rayHit.collider.gameObject.CompareTag("PlacementPlane") && !rayHit.collider.gameObject.CompareTag("PlacementIndicator"))
    //            {
    //                GameObject deselectedObject = rayHit.collider.gameObject;
    //                if (selectedObject != null && selectedObject.Equals(deselectedObject))
    //                {
    //                    selectedObject = null;
    //                    disableOutlineScripts(deselectedObject);
    //                }
    //            }
    //        }
    //    }
    //}


    //public void enableOutlineScripts(GameObject outlineObject)
    //{
    //    selectedObject = outlineObject;
    //    Outline[] outlines = outlineObject.GetComponentsInChildren<Outline>();
    //    foreach (Outline outline in outlines)
    //    {
    //        Debug.Log("yppppppppppp");
    //        outline.enabled = true;
    //    }
    //}

    private void disableOutlineScripts(GameObject outlineObject)
    {
        Outline[] outlines = outlineObject.GetComponentsInChildren<Outline>();
        foreach (Outline outline in outlines)
        {
            outline.enabled = false;
        }
    }

    public void setUpOutlineScripts(GameObject outlineObject)
    {
        outlineObject.AddComponent<Outline>();
        foreach (Transform t in outlineObject.GetComponentsInChildren<Transform>())
        {
            if (t.gameObject.GetComponent<Renderer>() != null && t.gameObject.GetComponent<Outline>() != null)
            {
                t.gameObject.AddComponent<Outline>();
            }
        }
        disableOutlineScripts(outlineObject);
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
