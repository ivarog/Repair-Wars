using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    void Start()
    {
        Camera.main.orthographicSize = 5.0f;
        float minimumWidth = Camera.main.orthographicSize * 9/16;

        float actualResolution = (float)(Screen.height) / (float)Screen.width;

        if(actualResolution > 16/9)
        {
            Camera.main.orthographicSize = minimumWidth * actualResolution;
        }
    }
}
