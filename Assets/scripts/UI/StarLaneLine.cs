using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StarLaneLine : MonoBehaviour
{

    private GameObject me;
    private Planet planet1;
    private Planet planet2;

    private LineRenderer line; // Line Renderer

    public static StarLaneLine Create(StarLane starLane) {
        GameObject parentPanel = GameObject.Find("MapPanel");
        StarLaneLine starLaneLine = parentPanel.AddComponent(typeof(StarLaneLine)) as StarLaneLine;
        //starLaneLine.transform.SetParent(parentPanel.transform);
        //starLaneLine.SetParent(parentPanel);
        starLaneLine.SetPlanet1(starLane.Planet1());
        starLaneLine.SetPlanet2(starLane.Planet2());

        return starLaneLine;
    }

    public void SetPlanet1(Planet planet1) {
        this.planet1 = planet1;
    }

    public void SetPlanet2(Planet planet2) {
        this.planet2 = planet2;
    }

    // Start is called before the first frame update
    void Start()
    {
        Color color = new Color(0.3f, 0.4f, 0.9f, 0.8f);

        me = new GameObject();
        GameObject parentPanel = GameObject.Find("MapPanel");
        me.name = "starlane-" + planet1.Name() + "-" + planet2.Name();
        // Add a Line Renderer to the GameObject
        line = me.AddComponent<LineRenderer>();
        // Set the width of the Line Renderer
        line.startColor = color;
        line.endColor = color;
        line.startWidth = 1F;//0.05F;
        line.endWidth = 1F;//0.05F;
        // Set the number of vertex for the Line Renderer
        line.positionCount = 2;
        line.useWorldSpace = true;    

        // More Configs
        line.material = new Material (Shader.Find("Sprites/Default"));
        line.material.color = color;

        
        line.SetPosition(0, planet1.GetPlanetSprite().transform.position);
        line.SetPosition(1, planet2.GetPlanetSprite().transform.position);
        Debug.Log(me.name + ": " + planet1.GetPlanetSprite().transform.position);
        Debug.Log(me.name + ": " + planet2.GetPlanetSprite().transform.position);
        

        //me.SetParent(parentPanel);
        //RectTransform rectTransform = me.GetComponent<RectTransform>();
        //rectTransform.SetParent(parentPanel.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
        //RectTransform parentTransform = parentPanel.GetComponent<RectTransform>();
        me.transform.SetParent(parentPanel.transform);

        this.RefreshPosition();
    }

    public void RefreshPosition() {
        if(line != null) {

            PlanetSprite planet1Sprite = planet1.GetPlanetSprite();
            PlanetSprite planet2Sprite = planet2.GetPlanetSprite();

            if(planet1Sprite.transform != null && planet2Sprite.transform != null) {

                // Update position of the two vertex of the Line Renderer
                line.SetPosition(0, planet1Sprite.GetGameObject().transform.position);
                line.SetPosition(1, planet2Sprite.GetGameObject().transform.position);

                Debug.Log(me.name + " set position to: " + planet1Sprite.GetGameObject().transform.position);
                Debug.Log(me.name + " set position to: " + planet2Sprite.GetGameObject().transform.position);
            }
        }

    }
}