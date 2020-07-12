using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{

    void Update()
    {
        if (Input.anyKey)
        {
            SceneManager.LoadScene (1);
        }
    }
}
