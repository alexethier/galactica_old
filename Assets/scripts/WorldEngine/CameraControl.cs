using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraControl : MonoBehaviour
{
    public Camera mainCamera; // Set from UI.

    private GameObject controlCamera;
    private GameObject mapCamera;
    
    public float ctrlZoomSensitivity = 10F;
    public float mouseSensitivity = 1;
    public float speed = 300;

    private float mapCameraMaxSize;
    private float mapCameraMinSize;

    private float slide;
    private float width;
    private float height;



    private float zoomBuffer;

    void Start () {
        this.zoomBuffer = 0;

        // Used in camera construction
        this.slide = 0.8F;
        this.width = mainCamera.pixelRect.width;
        this.height = mainCamera.pixelRect.height;

        this.CreateControlCamera(this.mainCamera);
        this.CreateMapCamera(this.mainCamera);

        this.mapCameraMinSize = 100F;
        this.mapCameraMaxSize = this.mapCamera.GetComponent<Camera>().orthographicSize;
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && e.control && e.keyCode == KeyCode.Equals) {
            zoomBuffer += ctrlZoomSensitivity;
            //Debug.Log("Ctrl Zoom in!");
        }
        
        if (e.type == EventType.KeyDown && e.control && e.keyCode == KeyCode.Minus) {
            zoomBuffer -= ctrlZoomSensitivity;
            //Debug.Log("Ctrl Zoom Out!");
        }
    }

    void Update()
    {
        if(Input.mouseScrollDelta.y != 0) {
            //Debug.Log("Mouth scroll is: " + Input.mouseScrollDelta.y);
            zoomBuffer -= Input.mouseScrollDelta.y * mouseSensitivity;
        }

        if(zoomBuffer != 0) {
            AdjustMapCameraZoom(zoomBuffer);
            zoomBuffer = 0;
        }
    }

    private void AdjustMapCameraZoom(float amount) {
        float adjustedAmount = Mathf.Clamp(amount, -20F, 20F);
        float newSize = 0.0F;
        Camera mapCameraComponent = this.mapCamera.GetComponent<Camera>();
        if(amount > 0) {
            newSize = Mathf.MoveTowards(mapCameraComponent.orthographicSize, this.mapCameraMinSize, speed * amount * Time.deltaTime);
            //Debug.Log("Zoom in new size: " + newSize);
        } else {
            newSize = Mathf.MoveTowards(mapCameraComponent.orthographicSize, this.mapCameraMaxSize, -1 * speed * amount * Time.deltaTime);
            //Debug.Log("Zoom out new size: " + newSize);
        }

        mapCameraComponent.orthographicSize = newSize;
    }

    private void CreateControlCamera(Camera baseCamera) {
        GameObject controlCameraObject = new GameObject();
        //controlCameraObject.transform.position = new Vector3(0F,0F,0F);
        controlCameraObject.name = "Control Panel Camera";
        Camera camera = controlCameraObject.AddComponent<Camera>();
        camera.CopyFrom(baseCamera);
        camera.pixelRect = new Rect(0, 0, (1 - this.slide)*this.width, this.height);
        camera.transform.SetParent(GameObject.Find("Control Panel").transform);
        float z = controlCameraObject.transform.localPosition.z;
        controlCameraObject.transform.localPosition = new Vector3(0,0,z);

        controlCameraObject.SetActive(true);
        this.controlCamera = controlCameraObject;
    }

    private void CreateMapCamera(Camera baseCamera) {
        GameObject mapCameraObject = new GameObject();
        //controlCameraObject.transform.position = new Vector3(0F,0F,0F);
        mapCameraObject.name = "Map Panel Camera";
        Camera camera = mapCameraObject.AddComponent<Camera>();
        camera.CopyFrom(baseCamera);
        camera.pixelRect = new Rect((1 - this.slide)*this.width, 0, this.slide*this.width, this.height);
        camera.transform.SetParent(GameObject.Find("Map Panel").transform);
        float z = mapCameraObject.transform.localPosition.z;
        mapCameraObject.transform.localPosition = new Vector3(0,0,z);

        mapCameraObject.SetActive(true);
        this.mapCamera = mapCameraObject;
    }
}
