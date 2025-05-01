using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera playerCamera = null;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        SetCameraPosition();
    }
    public void SetCameraPosition()
    {
        Vector3 result = Vector3.zero;
        result = target.position;
        result.z = -1;
        result.y = -2;
        if(result.x > 3.5f){
            result.x = 3.5f;
        }
        else if(result.x < -3.5f){
            result.x = -3.5f;
        }
        transform.position = result;
    }
}
