﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogError(GetComponent<Collider>().bounds.size);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
