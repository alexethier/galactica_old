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
                planetCoordinates[planet] = (0.1, 0.1);
            } else if(planetPlayerCount == 1) {
                planetCoordinates[planet] = (0.1, 0.9);
            } else if(planetPlayerCount == 2) {
                planetCoordinates[planet] = (0.9, 0.9);
            } else if(planetPlayerCount == 3) {
                planetCoordinates[planet] = (0.9, 0.1);
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

        int LOOPS = 1;
        // Loop through all Star Lanes and pull planets closer that are too far apart.
        for(int i=0; i < LOOPS; i++) {
            Dictionary<Planet, (double, double)> updatePlanetCoordinates = new Dictionary<Planet, (double, double)>();
            foreach(Planet planet in normalizedPlanetCoordinates.Keys) {
                updatePlanetCoordinates[planet] = (0.0,0.0);
            }

            double PULL_FACTOR = 0.5;
            double PULL_POWER = 1.2; //0.1;
            foreach(Planet planet in normalizedPlanetCoordinates.Keys) {
                if(planet.Owner() == null) {
                    foreach(StarLane starLane in planet.StarLanes()) {
                        Planet neighbor = starLane.Neighbor(planet);
                        double xDiff = normalizedPlanetCoordinates[planet].Item1 - normalizedPlanetCoordinates[neighbor].Item1;
                        double yDiff = normalizedPlanetCoordinates[planet].Item2 - normalizedPlanetCoordinates[neighbor].Item2;
                        double distance = Math.Max(0.00001, Math.Sqrt(Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2)));

                        if(distance > 0.2) {
                            double xUpdate = -1 * Math.Pow(distance, PULL_POWER) * Math.Cos(xDiff / distance) * PULL_FACTOR;
                            double yUpdate = -1 * Math.Pow(distance, PULL_POWER) * Math.Sin(yDiff / distance) * PULL_FACTOR;

                            double xNew = updatePlanetCoordinates[planet].Item1 + xUpdate;
                            double yNew = updatePlanetCoordinates[planet].Item2 + yUpdate;
                            updatePlanetCoordinates[planet] = (xNew, yNew);
                        }
                    }
                }
            }

            double MIN_DISTANCE_PUSH_FACTOR = 0.5;
            double MIN_DISTANCE = 0.2;

            int LOOP2 = 1;
            if(i == LOOPS - 1) {
                LOOP2 = 3;
            }
            for(int j=0; j < LOOP2; j++) {
                // Loop through all planet pairs and push them apart. (Warning O^2 time)
                    foreach(Planet planet1 in normalizedPlanetCoordinates.Keys) {
                        foreach(Planet planet2 in normalizedPlanetCoordinates.Keys) {
                            if(planet1.Owner() == null && planet1.Id() != planet2.Id()) {
                                double xDiff = normalizedPlanetCoordinates[planet1].Item1 - normalizedPlanetCoordinates[planet2].Item1;
                                double yDiff = normalizedPlanetCoordinates[planet1].Item2 - normalizedPlanetCoordinates[planet2].Item2;
                                double distance = Math.Max(0.00001, Math.Sqrt(Math.Pow(xDiff, 2) + Math.Pow(yDiff, 2)));

                                if(distance < MIN_DISTANCE) {
                                    //Debug.Log("Close: " + normalizedPlanetCoordinates[planet1].Item1  + "," + normalizedPlanetCoordinates[planet1].Item2 + " and " + normalizedPlanetCoordinates[planet2].Item1 + "," + normalizedPlanetCoordinates[planet2].Item2);
                                    double xUpdate = MIN_DISTANCE_PUSH_FACTOR * Math.Cos(xDiff / distance) * (MIN_DISTANCE-distance);
                                    double yUpdate = MIN_DISTANCE_PUSH_FACTOR * Math.Sin(yDiff / distance) * (MIN_DISTANCE-distance);
                                    //Debug.Log("Pushing " + planet1.Name() + " from " + planet2.Name() + " with current distance: " + distance + " amount: " + xUpdate + " " + yUpdate);

                                    double xNew = updatePlanetCoordinates[planet1].Item1 + xUpdate;
                                    double yNew = updatePlanetCoordinates[planet1].Item2 + yUpdate;
                                    updatePlanetCoordinates[planet1] = (xNew, yNew);
                                }
                            }
                        }
                    }
                

                foreach(Planet planet in updatePlanetCoordinates.Keys) {
                    if(planet.Owner() == null) {
                        double xNew = normalizedPlanetCoordinates[planet].Item1 + updatePlanetCoordinates[planet].Item1 + 0.08*Util.Random().NextDouble();
                        double yNew = normalizedPlanetCoordinates[planet].Item2 + updatePlanetCoordinates[planet].Item2 + 0.08*Util.Random().NextDouble();
                        if(xNew < 0) {
                            xNew = 0.2*Util.Random().NextDouble();
                        }
                        if(xNew > 1) {
                            xNew = 1 - 0.2*Util.Random().NextDouble();
                        }
                        if(yNew < 0) {
                            yNew = 0.2*Util.Random().NextDouble();
                        }
                        if(yNew > 1) {
                            yNew = 1 - 0.2*Util.Random().NextDouble();
                        }
                        normalizedPlanetCoordinates[planet] = (xNew, yNew);
                    }
                }
            }
        }

        //Debug.Log("Planet coordinates:");
        //foreach(Planet planet in normalizedPlanetCoordinates.Keys) {
        //    Debug.Log(planet.Name() + "|  " + normalizedPlanetCoordinates[planet].Item1 + " :: " + normalizedPlanetCoordinates[planet].Item2);
        //}

        return normalizedPlanetCoordinates;
    }

    // Returns a normalized distance for each planet to the base planet from 0 to 1.
    private static Dictionary<Planet, double> ComputeDistance(Planet basePlanet) {
        double SPREAD_FACTOR = 1.0;
        //int DENSE_COUNT = 3;

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

        return normalizedDistanceMap;

        /*
        Dictionary<Planet, double> spreadDistanceMap = new Dictionary<Planet, double>();
        foreach(KeyValuePair<Planet, double> entry in normalizedDistanceMap) {
            double distance = entry.Value;

            for(int i=1; i < DENSE_COUNT; i++) {
                distance = distance * distance;
            }
            spreadDistanceMap[entry.Key] = distance;
        }

        return spreadDistanceMap;
        */
    }
}
