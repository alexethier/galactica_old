using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraControl : MonoBehaviour
{
    public static CameraControl me;

    public Camera mainCamera; // Set from UI.

    private GameObject controlCamera;
    private GameObject mapCamera;
    
    public float ctrlZoomSensitivity = 10F;
    public float mouseSensitivity = 10;
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

        //this.CreateControlCamera(this.mainCamera);
        this.mapCamera = this.CreateMapCamera(this.mainCamera);
        this.controlCamera = this.CreateControlCamera(this.mainCamera);
        //this.mapCamera = this.CreateCamera(this.mainCamera, "Map Panel Camera", GameObject.Find("Map Panel"));
        //this.controlCamera = this.CreateCamera(this.mainCamera, "Control Panel Camera", GameObject.Find("Control Panel"));

        this.mapCameraMinSize = 100F;
        this.mapCameraMaxSize = this.mapCamera.GetComponent<Camera>().orthographicSize;
        me = this;
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
            newSize = Mathf.MoveTowards(mapCameraComponent.orthographicSize, mapCameraMinSize, speed * amount * Time.deltaTime);
            //Debug.Log("Zoom in new size: " + newSize);
        } else {
            newSize = Mathf.MoveTowards(mapCameraComponent.orthographicSize, mapCameraMaxSize, -1 * speed * amount * Time.deltaTime);
            //Debug.Log("Zoom out new size: " + newSize);
        }

        mapCameraComponent.orthographicSize = newSize;
    }

    /*
    private GameObject CreateCamera(Camera baseCamera, string name, GameObject parent) {
        GameObject controlCameraObject = new GameObject();
        controlCameraObject.name = name;
        Camera camera = controlCameraObject.AddComponent<Camera>();
        camera.CopyFrom(baseCamera);
        camera.pixelRect = parent.GetComponent<RectTransform>().rect;
        camera.transform.SetParent(parent.transform);
        float z = controlCameraObject.transform.localPosition.z;
        controlCameraObject.transform.localPosition = new Vector3(0,0,z);

        controlCameraObject.SetActive(true);
        return controlCameraObject;
    }
    */

    private GameObject CreateMapCamera(Camera baseCamera) {
        GameObject mapCameraObject = new GameObject();
        mapCameraObject.name = "Map Panel Camera";
        Camera camera = mapCameraObject.AddComponent<Camera>();
        camera.CopyFrom(baseCamera);
        camera.pixelRect = new Rect((1 - this.slide)*this.width, 0, this.slide*this.width, this.height);
        camera.transform.SetParent(GameObject.Find("Map Panel").transform);
        float z = mapCameraObject.transform.localPosition.z;
        mapCameraObject.transform.localPosition = new Vector3(0,0,z);

        mapCameraObject.SetActive(true);
        return mapCameraObject;
    }

    private GameObject CreateControlCamera(Camera baseCamera) {
        GameObject controlPanel = GameObject.Find("Control Panel");

        GameObject controlCameraObject = new GameObject();
        //controlCameraObject.transform.position = new Vector3(0F,0F,0F);
        controlCameraObject.name = "Control Panel Camera";
        Camera camera = controlCameraObject.AddComponent<Camera>();
        camera.CopyFrom(baseCamera);
        camera.pixelRect = new Rect(0, 0, (1 - this.slide)*this.width, this.height);
        camera.transform.SetParent(controlPanel.transform);
        float z = controlCameraObject.transform.localPosition.z;
        controlCameraObject.transform.localPosition = new Vector3(0,0,z);

        controlCameraObject.SetActive(true);
        return controlCameraObject;
    }
    



    public GameObject MapCamera() {
        return mapCamera;
    }

    public static void AdjustMapCameraRect(float xOffsetMin, float xOffsetMax, float yOffsetMin, float yOffsetMax) {
        me.MapCamera().GetComponent<Camera>().pixelRect = new Rect((1 - me.slide)*me.width + xOffsetMin, 0 + yOffsetMin, me.slide*me.width + xOffsetMax, me.height + yOffsetMax);
    }
}
