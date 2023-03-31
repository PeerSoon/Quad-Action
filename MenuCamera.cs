using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public GameObject menuCameraGameObject;
    
    void Update()
    {
        transform.position = target.position + offset;
    }
}
