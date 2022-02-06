using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarLane
{
    // private int travelLength; TODO
    private Planet planet1;
    private Planet planet2;

    private StarLaneLine starLaneLine;

    public StarLane(Planet planet1, Planet planet2) {
        this.planet1 = planet1;
        this.planet2 = planet2;
        //travelLength = 4;

        // Create UI element
        starLaneLine = StarLaneLine.Create(this);
    }

    public Planet Neighbor(Planet inputPlanet) {
        if(planet1.Id() == inputPlanet.Id()) {
            return planet2;
        } else {
            return planet1;
        }
    }

    public bool Contains(Planet planet) {
        if(planet1.Id() == planet.Id() || planet2.Id() == planet.Id()) {
            return true;
        } else {
            return false;
        }
    }

    public Planet Planet1() {
        return planet1;
    }

    public Planet Planet2() {
        return planet2;
    }

    public void RefreshPosition() {
        starLaneLine.RefreshPosition();
    }
}
