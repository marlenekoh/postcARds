using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using LitJson;


public static class Session
{
    public class Object
    {
        public string tag = "";
        public List<float> pos;
        public List<float> rot;
        public List<float> scale;

        public Object(string tag, List<float> pos, List<float> rot, List<float> scale)
        {
            this.tag = tag;
            this.pos = pos;
            this.rot = rot;
            this.scale = scale;
        }
        public string getTag()
        {
            return this.tag;
        }
        public List<float> getPos()
        {
            return this.pos;
        }
        public List<float> getRot()
        {
            return this.rot;
        }
        public List<float> getScale()
        {
            return this.scale;
        }
    }

    public static bool hasRecord = false;
    public static string id = "";
    public static List<Object> sceneObjects = new List<Object>();

    public static void CreateSession(JsonData jsonvale)
    {
        hasRecord = (jsonvale["has_record"].ToString() == "true");
        id = jsonvale["id"].ToString();

        JsonData jsonSceneData= JsonMapper.ToObject(jsonvale["scene_data"].ToString());

        for (int i = 0; i < jsonSceneData["Objects"].Count; i++)
        {
            string objTag = jsonSceneData["Objects"][i]["tag"].ToString();
            List<float> objPos = ProcessjsonT(jsonSceneData["Objects"][i]["pos"].ToString());
            List<float> objRot = ProcessjsonT(jsonSceneData["Objects"][i]["rot"].ToString());
            List<float> objScale = ProcessjsonT(jsonSceneData["Objects"][i]["scale"].ToString());
            sceneObjects.Add(new Object(objTag, objPos, objRot, objScale));
        }
    }

    public static List<float> ProcessjsonT(string jsonT)
    {
        List<float> transformVals = new List<float>();
        MatchCollection matches = Regex.Matches(jsonT, "[^,(?! )]+");
        foreach (Match match in matches)
        {
            transformVals.Add(float.Parse(match.ToString()));
        }

        return transformVals;
    }

    // TODO: Call ResetSession after submission
    public static void ResetSession()
    {
        hasRecord = false;
        id = "";
        sceneObjects = new List<Object>();

    }
}
