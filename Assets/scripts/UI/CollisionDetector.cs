using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionDetector : MonoBehaviour, ICollisionDetector
{

    private ICollisionDetector parent;

    public void SetParent(ICollisionDetector obj) {
        this.parent = obj;
    }

    public void OnMouseEnter() {
        parent.OnMouseEnter();
    }

    public void OnMouseExit() {
        parent.OnMouseExit();
    }

    public void OnMouseDown() {
        parent.OnMouseDown();
    }
}

public interface ICollisionDetector
{
    void OnMouseEnter();

    void OnMouseExit();

    void OnMouseDown();
}
