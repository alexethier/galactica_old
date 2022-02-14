using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraControl : MonoBehaviour
{
    public Camera cam;
    
    public float ctrlZoomSensitivity = 10F;
    public float mouseSensitivity = 1;
    public float speed = 300;
    public float currentSize;
    float targetZoom;

    private float camMaxSize;
    private float camMinSize;

    void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.control && e.keyCode == KeyCode.Equals) {
            this.AdjustCameraZoom(ctrlZoomSensitivity);
            //Debug.Log("Ctrl Zoom in!");
        }
        
        if (e.type == EventType.KeyDown && e.control && e.keyCode == KeyCode.Minus) {
            this.AdjustCameraZoom(-1 * ctrlZoomSensitivity);
            //Debug.Log("Ctrl Zoom Out!");
        }
    }

    void Update()
    {
        if(Input.mouseScrollDelta.y != 0) {
            //Debug.Log("Mouth scroll is: " + Input.mouseScrollDelta.y);
            targetZoom -= Input.mouseScrollDelta.y * mouseSensitivity;
            this.AdjustCameraZoom(targetZoom);
        }
        cam.orthographicSize = currentSize;
    }

    void Start () {
        camMinSize = 100F;
        camMaxSize = cam.orthographicSize;
        currentSize = cam.orthographicSize;
    }

    private void AdjustCameraZoom(float amount) {
        float adjustedAmount = Mathf.Clamp(amount, -20F, 20F);
        float newSize = 0.0F;
        if(amount > 0) {
            newSize = Mathf.MoveTowards(cam.orthographicSize, camMinSize, speed * amount * Time.deltaTime);
            //Debug.Log("Zoom in new size: " + newSize);
        } else {
            newSize = Mathf.MoveTowards(cam.orthographicSize, camMaxSize, -1 * speed * amount * Time.deltaTime);
            //Debug.Log("Zoom out new size: " + newSize);
        }
        //Debug.Log("Old Size: " + cam.orthographicSize + " -> " + newSize);
        currentSize = newSize;
    }
}
