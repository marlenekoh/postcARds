using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Session;

public class ReceiverPlacement : MonoBehaviour
{
    public GameObject santaObjects;
    public GameObject rudolphObjects;

    private Dictionary<string, GameObject> allPrefabs = new Dictionary<string, GameObject>();
    private GameObject objects;

    // Start is called before the first frame update
    void Start()
    {
        // inefficient to load all prefabss. How to categorise?
        System.Object[] prefabs = Resources.LoadAll("", typeof(GameObject));

        foreach (System.Object prefab in prefabs)
        {
            GameObject prefabObj = prefab as GameObject;
            //Debug.Log(prefabObj.name);
            allPrefabs.Add(prefabObj.name.ToString(), prefabObj);
        }

        SetObjects();
    }
    
    private void SetObjects()
    {
        Debug.Log("card is : " + Session.cardName);
        if (Session.cardName == "rudolph")
        {
            objects = rudolphObjects;
        } else
        {
            objects = santaObjects;
        }

        List<SceneObject> allObjects = new List<SceneObject>();
        allObjects = Session.sceneObjects;

        for (int i = 0; i < allObjects.Count; i++)
        {
            SceneObject item = allObjects[i];

            string itemTag = item.getTag();
            Vector3 position = new Vector3(item.getPos()[0], item.getPos()[1], item.getPos()[2]); 
            Quaternion rotation = new Quaternion(item.getRot()[0], item.getRot()[1], item.getRot()[2], item.getRot()[3]);
            Vector3 scale = new Vector3(item.getScale()[0], item.getScale()[1], item.getScale()[2]);

            Debug.Log(itemTag);
            //Debug.Log(position);
            //Debug.Log(rotation);
            //Debug.Log(scale);

            GameObject itemToPlace = allPrefabs[itemTag];
            
            GameObject itemPlaced = Instantiate(itemToPlace, objects.transform, false);
            itemPlaced.transform.localPosition = position;
            itemPlaced.transform.localRotation = rotation;
            itemPlaced.transform.localScale = scale;
        }
    }

    public void GotoNextScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
}
