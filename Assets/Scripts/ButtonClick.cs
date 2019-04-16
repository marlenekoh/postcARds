using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            RaycastHit rayHit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out rayHit))
            {
                GameObject hitObject = rayHit.collider.gameObject;
                if (hitObject.CompareTag("3DButton"))
                {

                }
            }
        }
    }
}
