using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarLane
{
    // private int travelLength; TODO
    private Planet planet1;
    private Planet planet2;

    public StarLane(Planet planet1, Planet planet2) {
        this.planet1 = planet1;
        this.planet2 = planet2;
        //travelLength = 4;
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
}
