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

    public void Build(int desiredPlanetCount) {
        // First create a grid
        int LENGTH = 6;
        int orientation = 0; // 0 -> right, 1 down, 2 left, 3 up
        int roundCount = 0; // Counter for increasing step size
        double stepSize = 1.0;
        double stepCount = 0.0;
        double planetProbability = 1;

        int x = 0;
        int y = 0;

        Debug.Log("Generating spiral grid.");

        int count = 0;
        while(planetPositions.Count < desiredPlanetCount) {

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
                Planet planet = new Planet(planetPositions.Count);
                planetPositions[planet] = (x,y);
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

        Debug.Log("Step Size: " + stepSize + " step size unit: " + (1.0 / stepSize));
        double BORDER_FRAME_THRESHOLD = (1 / stepSize) * 0.3; //1 / stepSize; // Will crash for very small step sizes
        double BORDER_SHIFT = 1.0;
        double JIGGLE_FACTOR = 1.35; // Higher is less jiggle
        // Shift and scale planet locations
        foreach (KeyValuePair<Planet, (double, double)> entry in planetPositions) {
            double xPos = entry.Value.Item1;
            double yPos = entry.Value.Item2;

            // Map coordinates onto a 1 unit long rectangle in the first quadrant.
            double xNew = (xPos + stepSize / 2.0) / (stepSize * BORDER_SHIFT);
            double yNew = (yPos + stepSize / 2.0) / (stepSize * BORDER_SHIFT);

            Debug.Log("Planet Base " + entry.Key.Name() + " " + xNew + "," + yNew);

            // Jiggle planet locations a little
            xNew = xNew + ((Util.Random().NextDouble() - 0.5) / (stepSize * JIGGLE_FACTOR));
            yNew = yNew + ((Util.Random().NextDouble() - 0.5) / (stepSize * JIGGLE_FACTOR));

            Debug.Log("Planet Randomized " + entry.Key.Name() + " " + xNew + "," + yNew);

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
            
            
            entry.Key.SetPosition(xNew, yNew);
        }
    }
}
