using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlanetPlacer 
{
    // Returns normalized coordinates for all planets assuming planets must lie on the first quadrant of a grid from 0 to 1 with players at the corners.
    public static Dictionary<Planet, (double, double)> PlacePlanets(List<Planet> planets) {
        Dictionary<Planet, (double, double)> planetCoordinates = new Dictionary<Planet, (double, double)>();

        List<Planet> playerPlanets = new List<Planet>();
        foreach(Planet planet in planets) {
            if(planet.Owner() != null) {
                playerPlanets.Add(planet);
            }
        }

        int planetPlayerCount = 0;
        foreach(Planet planet in playerPlanets) {
            if(planetPlayerCount == 0) {
                planetCoordinates[planet] = (0, 0);
            } else if(planetPlayerCount == 1) {
                planetCoordinates[planet] = (0, 1);
            } else if(planetPlayerCount == 2) {
                planetCoordinates[planet] = (1, 1);
            } else if(planetPlayerCount == 3) {
                planetCoordinates[planet] = (1, 0);
            } else {
                throw new Exception("Too many players.");
            }
            planetPlayerCount++;
        }

        foreach(Planet playerPlanet in playerPlanets) {
            Dictionary<Planet, double> distances = ComputeDistance(playerPlanet);
            foreach(KeyValuePair<Planet, double> entry in distances) {
                Planet planet = entry.Key;
                if(planet.Owner() == null) {
                    double distance = entry.Value;

                    double x = planetCoordinates[playerPlanet].Item1 * (distance - 0.5) / playerPlanets.Count;
                    double y = planetCoordinates[playerPlanet].Item2 * (distance - 0.5) / playerPlanets.Count;

                    if(!planetCoordinates.ContainsKey(planet)) {
                        planetCoordinates[planet] = (x,y);
                    } else {
                        double xTotal = planetCoordinates[planet].Item1;
                        double yTotal = planetCoordinates[planet].Item2;
                        planetCoordinates[planet] = (x+xTotal,y+yTotal);                     
                    }
                }
            }
        }

        Dictionary<Planet, (double, double)> normalizedPlanetCoordinates = new Dictionary<Planet, (double, double)>();

        foreach(Planet planet in planetCoordinates.Keys) {
            if(planet.Owner() == null) {
                double x = planetCoordinates[planet].Item1;
                double y = planetCoordinates[planet].Item2;
                if(x > 0.5) {
                    x = 0.5;
                }
                if(y > 0.5) {
                    y = 0.5;
                }
                normalizedPlanetCoordinates[planet] = (x+0.5,y+0.5);
            } else {
                normalizedPlanetCoordinates[planet] = planetCoordinates[planet];
            }
        }

        return normalizedPlanetCoordinates;
    }

    // Returns a normalized distance for each planet to the base planet from 0 to 1.
    private static Dictionary<Planet, double> ComputeDistance(Planet basePlanet) {
        double SPREAD_FACTOR = 1.0;
        int DENSE_COUNT = 3;

        int maxDistance = 0;

        Dictionary<Planet, int> distanceMap = new Dictionary<Planet, int>();
        List<Planet> queue = new List<Planet>();
        distanceMap[basePlanet] = 1;
        queue.Add(basePlanet);

        while(queue.Count > 0) {
            Planet planet = queue[0];
            queue.RemoveAt(0);
            int currentDistance = distanceMap[planet];

            foreach(StarLane starLane in planet.StarLanes()) {
                Planet neighbor = starLane.Neighbor(basePlanet);
                if(!distanceMap.ContainsKey(neighbor)) {
                    int newDistance = currentDistance + 1;
                    distanceMap[neighbor] = newDistance;
                    queue.Add(neighbor);
                    if(newDistance > maxDistance) {
                        maxDistance = newDistance;
                    }
                }
            }
        }

        Dictionary<Planet, double> normalizedDistanceMap = new Dictionary<Planet, double>();
        foreach(KeyValuePair<Planet, int> entry in distanceMap) {
            normalizedDistanceMap[entry.Key] = SPREAD_FACTOR * (double)entry.Value / (double)maxDistance;
        }

        Dictionary<Planet, double> spreadDistanceMap = new Dictionary<Planet, double>();
        foreach(KeyValuePair<Planet, double> entry in normalizedDistanceMap) {
            double distance = entry.Value;

            for(int i=1; i < DENSE_COUNT; i++) {
                distance = distance * distance;
            }
            spreadDistanceMap[entry.Key] = distance;
        }

        return spreadDistanceMap;
    }
}
