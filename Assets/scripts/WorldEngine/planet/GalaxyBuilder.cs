using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GalaxyBuilder
{

    private Dictionary<Planet, (double, double)> planetPositions;

    public GalaxyBuilder() {
        planetPositions = new Dictionary<Planet, (double, double)>();
    }

    private void BuildPlanets(int desiredPlanetCount) {

        Dictionary<Planet, (double, double)> initialPlanetPositions = new Dictionary<Planet, (double, double)>();

// First create a grid
        int LENGTH = 6;
        int orientation = 0; // 0 -> right, 1 down, 2 left, 3 up
        int roundCount = 0; // Counter for increasing step size
        double stepSize = 1.0;
        double stepCount = 0.0;
        double planetProbability = 1;

        int x = 0;
        int y = 0;

        int count = 0;
        while(initialPlanetPositions.Count < desiredPlanetCount) {

            // TODO: Switch to a hex based map instead of square for a better pattern.

            /*
            double distance = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            double scaler = 1.0 / (0.1 + distance);
            double scaledPlanetProbability = planetProbability * scaler;
            if(scaler < 0.5) {
                scaledPlanetProbability = 0.5 * planetProbability;
            }
            Debug.Log("Prob: " + planetProbability + " distance: " + distance + " scaler: " + scaler + " scaled prob: " + scaledPlanetProbability);
            */
            // The best maps fill all possible grid positions in a spiraling grid pattern. The above scale factors do more harm than good.
            double scaledPlanetProbability = planetProbability; 

            if(Util.Random().NextDouble() < scaledPlanetProbability) {
                Planet planet = new Planet(initialPlanetPositions.Count);
                initialPlanetPositions[planet] = (x,y);
            }

            if(orientation == 0) {
                x = x + 1;
            } else if(orientation == 1) {
                y = y - 1;
            } else if(orientation == 2) {
                x = x - 1;
            } else if(orientation == 3) {
                y = y + 1;
            } else {
                throw new Exception("Unknown orientation");
            }

            stepCount++;
            if(stepCount == stepSize) {
                orientation = (orientation + 1) % 4;
                roundCount++;
                stepCount = 0;
            }

            if(roundCount == 2) {
                roundCount = 0;
                stepSize++;
            }
        }

        //Debug.Log("Step Size: " + stepSize + " step size unit: " + (1.0 / stepSize));
        double BORDER_FRAME_THRESHOLD = (1 / stepSize) * 0.3; //1 / stepSize; // Will crash for very small step sizes
        double BORDER_SHIFT = 1.0;
        double JIGGLE_FACTOR = 1.35; // Higher is less jiggle
        // Shift and scale planet locations
        foreach (KeyValuePair<Planet, (double, double)> entry in initialPlanetPositions) {
            Planet planet = entry.Key;
            double xPos = entry.Value.Item1;
            double yPos = entry.Value.Item2;

            // Map coordinates onto a 1 unit long rectangle in the first quadrant.
            double xNew = (xPos + stepSize / 2.0) / (stepSize * BORDER_SHIFT);
            double yNew = (yPos + stepSize / 2.0) / (stepSize * BORDER_SHIFT);

            // Jiggle planet locations a little
            xNew = xNew + ((Util.Random().NextDouble() - 0.5) / (stepSize * JIGGLE_FACTOR));
            yNew = yNew + ((Util.Random().NextDouble() - 0.5) / (stepSize * JIGGLE_FACTOR));

            // Move planets away from borders.
            
            if(xNew < BORDER_FRAME_THRESHOLD) {
                xNew = xNew + BORDER_FRAME_THRESHOLD;
            } else if(xNew > 1 - BORDER_FRAME_THRESHOLD) {
                xNew = xNew - BORDER_FRAME_THRESHOLD;
            }
            if(yNew < BORDER_FRAME_THRESHOLD) {
                yNew = yNew + BORDER_FRAME_THRESHOLD;
            } else if(yNew > 1 - BORDER_FRAME_THRESHOLD) {
                yNew = yNew - BORDER_FRAME_THRESHOLD;
            }
            
            
            planet.SetPosition(xNew, yNew);
            planetPositions[planet] = (xNew, yNew);
        }
    }

    private void BuildStarLanes() {
        foreach (Planet planet1 in planetPositions.Keys) {
            int starLaneCount = Util.Select<int>(new List<int>{ 2, 2, 3, 3, 3, 4});
            List<Planet> orderedPlanets = this.GetClosestPlanets(planet1);

            for(int i=0; i < starLaneCount; i++) {
                Planet planet2 = orderedPlanets[i];
                if(!this.AreNeighbors(planet1, planet2)) {
                    StarLane starLane = new StarLane(planet1, planet2);
                    planet1.AddStarLane(starLane);
                    planet2.AddStarLane(starLane);
                }
            }
        }
    }

    private bool AreNeighbors(Planet planet1, Planet planet2) {
        foreach(StarLane starLane in planet1.StarLanes()) {
            if(starLane.Neighbor(planet1).Id() == planet2.Id()) {
                return true;
            }
        }
        return false;
    }

    private List<Planet> GetClosestPlanets(Planet planet) {
        Dictionary<Double, Planet> distanceMap = new Dictionary<Double, Planet>();
        Vector3 position = planet.GetPlanetSprite().Position();

        foreach (Planet otherPlanet in planetPositions.Keys) {
            if(otherPlanet.Id() != planet.Id()) {
                Vector3 otherPosition = otherPlanet.GetPlanetSprite().Position();
                double distance = Math.Sqrt(Math.Pow(position.x - otherPosition.x, 2) + Math.Pow(position.y - otherPosition.y, 2));
                distanceMap[distance] = otherPlanet;
            }
        }

        List<Double> orderedDistances = new List<Double>(distanceMap.Keys);
        orderedDistances.Sort();
        List<Planet> orderedPlanets = new List<Planet>();
        foreach(Double distance in orderedDistances) {
            orderedPlanets.Add(distanceMap[distance]);
        }

        return orderedPlanets;
    }

    private bool ValidateStarLanes() {

        HashSet<Planet> connectedPlanets = new HashSet<Planet>();

        foreach(Planet planet1 in planetPositions.Keys) {
            Queue<Planet> checkPlanetQueue = new Queue<Planet>();
            checkPlanetQueue.Enqueue(planet1);

            while(checkPlanetQueue.Count > 0) {
                Planet planet = checkPlanetQueue.Dequeue();
                foreach(StarLane starLane in planet.StarLanes()) {
                    Planet neighbor = starLane.Neighbor(planet);
                    if(!connectedPlanets.Contains(neighbor)) {
                        connectedPlanets.Add(neighbor);
                        checkPlanetQueue.Enqueue(neighbor);
                    }
                }
            }

            break; //Only need to check first key in collection.
        }

        return connectedPlanets.Count == planetPositions.Count;
    }

    public void Build(int desiredPlanetCount) {
        this.BuildPlanets(desiredPlanetCount);
        this.BuildStarLanes();
        bool valid = this.ValidateStarLanes();
        if(!valid) {
            // TODO: Generate a universe in a way that ensures all planets are connected.
            throw new Exception("Planets not connected, Universe generation failed.");
        }
    }
}
