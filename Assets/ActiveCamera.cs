using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCamera : MonoBehaviour
{
    public Camera camera;
}

public delegate void CameraChangeEvent(Camera newCamera);