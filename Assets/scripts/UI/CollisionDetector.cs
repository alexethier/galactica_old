using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionDetector : MonoBehaviour
{

    private PlanetSprite parent; // Change to interface

    public void SetParent(PlanetSprite obj) {
        this.parent = obj;
    }

    void OnMouseEnter() {
        Debug.Log("TODO: Update parent from PlanetSprite to an interface.")
        parent.OnMouseEnter();
    }

    void OnMouseExit() {
        parent.OnMouseExit();
    }

    void OnMouseDown() {
        parent.OnMouseDown();
    }
}
