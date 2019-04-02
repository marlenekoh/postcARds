using Lean.Touch;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureManager : MonoBehaviour
{
    public static void select(GameObject selectedObject)
    {
        Material m = selectedObject.GetComponent<Renderer>().material;
        m.shader = Shader.Find("Outlined/Silhouetted Diffuse");
        m.SetColor("Main Color", Color.white);
        m.SetColor("Outline Color", Color.yellow);
        m.SetFloat("Outline width", 0.03f);
    }

    public static void deselect(GameObject deselectedObject)
    {
        deselectedObject.GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Texture");
    }

    public static void setUpLeanTouchScripts(GameObject leantouchObject)
    {
        LeanSelectable ls = leantouchObject.AddComponent<LeanSelectable>();
        ls.DeselectOnUp = false;
        ls.HideWithFinger = false;
        ls.IsolateSelectingFingers = false;
        ls.OnSelect.AddListener(delegate { select(leantouchObject); });
    }
}
