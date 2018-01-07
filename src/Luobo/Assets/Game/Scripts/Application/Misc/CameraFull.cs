using UnityEngine;
using System.Collections;

public class CameraFull : MonoBehaviour
{
    public Camera[] myCameraS;
    public float widthF = 480f;
    public float heightF = 800f;
    // Use this for initialization  
    void Start()
    {
        foreach (Camera myCamera in myCameraS)
        {
            if (myCamera != null)
                myCamera.aspect = widthF / heightF;
        }
    }

}