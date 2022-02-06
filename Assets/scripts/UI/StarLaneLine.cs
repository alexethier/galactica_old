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
        GameObject ParentPanel = GameObject.Find("MapPanel");
        StarLaneLine starLaneLine = ParentPanel.AddComponent(typeof(StarLaneLine)) as StarLaneLine;

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
        me = new GameObject();
        me.name = "starlane-" + planet1.Name() + "-" + planet2.Name();
        // Add a Line Renderer to the GameObject
        line = me.AddComponent<LineRenderer>();
        // Set the width of the Line Renderer
        line.startWidth = 0.05F;
        line.endWidth = 0.05F;
        // Set the number of vertex for the Line Renderer
        line.positionCount = 2;

        // More Configs
        line.sortingOrder = 1;
        line.material = new Material (Shader.Find ("Sprites/Default"));
        line.material.color = Color.red; 
    }

    public void RefreshPosition() {

        try {
            PlanetSprite planet1Sprite = planet1.GetPlanetSprite();
            PlanetSprite planet2Sprite = planet2.GetPlanetSprite();

            if(planet1Sprite.transform != null && planet2Sprite.transform != null) {
                // Update position of the two vertex of the Line Renderer
                line.SetPosition(0, planet1Sprite.transform.position);
                line.SetPosition(1, planet2Sprite.transform.position);
            }
        } catch(Exception e) {
            //pass
        }
    }
}