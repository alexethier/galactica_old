using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControl
{

    private static float defaultMapPanelHeight;

    public static void OpenPlanetDetailsPanel(Planet planet) {
        GameObject mapPanel = GameObject.Find("Map Panel");
        RectTransform mapPanelRectTransform = mapPanel.GetComponent<RectTransform>();
        defaultMapPanelHeight = mapPanelRectTransform.rect.height;
        float mapPanelVerticalShrink = 0.2F*defaultMapPanelHeight;
        mapPanelRectTransform.offsetMin = new Vector2(mapPanelRectTransform.offsetMin.x, mapPanelVerticalShrink);

        //GameObject cameraControlGameObject = GameObject.Find("Map Panel Camera");
        //CameraControl cameraControl = cameraControlGameObject.GetComponent<CameraControl>();
        CameraControl.AdjustMapCameraRect(0F, 0F, 0.4F*defaultMapPanelHeight, -0.4F*defaultMapPanelHeight);
        // Resize map panel
        // Resize map camera
        // Create new planet details panel
    }
}
