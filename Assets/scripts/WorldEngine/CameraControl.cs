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

    private Transform baseTransform;
    private float camMaxSize;
    private float camMinSize;

    private float slide;
    private float width;
    private float height;

    private GameObject controlCamera;

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
        baseTransform = cam.transform;

        this.slide = 0.8F;
        this.width = cam.pixelRect.width;
        this.height = cam.pixelRect.height;
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

        if(currentSize == camMaxSize && newSize < currentSize) {
            // Apply Camera Transform
            this.AdjustCameraTransform(true);
        } else if(newSize == camMaxSize && newSize > currentSize) {
            // Unapply Camera Transform
            this.AdjustCameraTransform(false);
        }
        currentSize = newSize;
    }

    private void AdjustCameraTransform(bool isAdjust) {
        int direction = -1;
        if(isAdjust) {

            // Create a second camera on the control panel
            GameObject cameraGameObject = new GameObject();
            cameraGameObject.transform.position = new Vector3(0F,0F,0F);
            cameraGameObject.name = "Control Panel Camera";
            Camera camera = cameraGameObject.AddComponent<Camera>();
            camera.CopyFrom(this.cam);
            camera.pixelRect = new Rect(0, 0, (1 - this.slide)*this.width, this.height);
            camera.transform.SetParent(GameObject.Find("Control Panel").transform);
            float z = cameraGameObject.transform.localPosition.z;
            cameraGameObject.transform.localPosition = new Vector3(0,0,z);

            cameraGameObject.SetActive(true);
            this.controlCamera = cameraGameObject;
            //

            direction = 1;
            cam.pixelRect = new Rect((1 - this.slide)*this.width, 0, this.slide*this.width, this.height);

        } else {
            // Destroy secondary camera
            Destroy(this.controlCamera);
            this.controlCamera = null;
            //

            cam.pixelRect = new Rect(0,0,this.width, this.height);

        }
        cam.transform.Translate(new Vector3(direction * 0.25F*(1 - this.slide)*this.width, 0, 0), Space.World);
    }
}
