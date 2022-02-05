using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Universe
{

    private int turnCount;
    private Player mainPlayer;
    private List<Player> players;
    private List<Planet> planets;

    private static Universe universe;
    public static Universe Instance()
    {
        if (universe == null)
        {
            //universe = FindObjectOfType(typeof(Universe)) as Universe;
            universe = new Universe();
            universe.Start();
        }
        return universe;
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting Game!");
        planets = new List<Planet>();

        turnCount = 0;
        int playerCount = 4;
        players = new List<Player>();

        // Create players and initial planets
        for(int i=0; i < playerCount; i++) {
            Player player = null;
            if(i == 0) {
                player = new Player(i, true);
                mainPlayer = player;
            } else {
                player = new Player(i, false);
            }
            players.Add(player);

            Planet homePlanet = new Planet(planets.Count);
            homePlanet.SetExoticRating(3);
            homePlanet.SetHospitableRating(3);
            homePlanet.SetWonderfulRating(3);
            homePlanet.SetResourcefulRating(3);
            homePlanet.SetOwner(player);
            planets.Add(homePlanet);
            player.AddPlanet(homePlanet);
        }

        // Setup other planets
        for(int i=0; i < 12; i++) {
            Planet newPlanet = new Planet(planets.Count);
            planets.Add(newPlanet);
        }

        // Setup StarLanes
        int starLaneCount = 0;
        for(int i=0; i < planets.Count; i++) {
            Planet planet1 = planets[i];
            Planet planet2 = planets[(i + players.Count) % planets.Count];
            StarLane starLane = new StarLane(planet1, planet2);
            planet1.AddStarLane(starLane);
            planet2.AddStarLane(starLane);
            starLaneCount++;
        }

        int attempts = 0;
        while(starLaneCount < (planets.Count * 27) / 20) {
            attempts++;
            if(attempts > 10000) {
                throw new Exception("Could not compute starlanes.");
            }
            
            Planet planet1 = Util.Select(planets);

            if(planet1.StarLanes().Count > 3) {
                continue;
            }

            Planet planet2 = Util.Select(planets);

            if(planet2.StarLanes().Count > 3) {
                continue;
            }

            // Ensure two chosen planets are not both home planets.
            if(planet1.Owner() != null && planet2.Owner() != null) {
                continue;
            }

            // Ensure star lane hasn't already been created.
            bool invalid = false;
            foreach(StarLane planet2StarLane in planet2.StarLanes()) {
                if(planet2StarLane.Contains(planet1)) {
                    invalid = true;
                    break;
                }
            }
            if(invalid) {
                continue;
            }

            StarLane starLane = new StarLane(planet1, planet2);
            planet1.AddStarLane(starLane);
            planet2.AddStarLane(starLane);
            starLaneCount++;    
        }

        // Compute Planet Positions
        Dictionary<Planet, (double, double)> planetCoordinates = PlanetPlacer.PlacePlanets(planets);
        foreach(Planet planet in planetCoordinates.Keys) {
            Debug.Log(planet.Name());
            Debug.Log(planetCoordinates[planet].Item1 + "-" + planetCoordinates[planet].Item2);
            planet.SetPosition(planetCoordinates[planet].Item1, planetCoordinates[planet].Item2);
        }
        //

        /*
        foreach(Planet planet in planets) {
            Debug.Log("");
            Debug.Log("");
            Debug.Log(planet.Name());
            Debug.Log("Exotic: " + planet.ExoticRating() + " / " + planet.ExoticCap());
            Debug.Log("Hospitable: " + planet.HospitableRating() + " / " + planet.HospitableCap());
            Debug.Log("Wonderful: " + planet.WonderfulRating() + " / " + planet.WonderfulCap());
            Debug.Log("Resourceful: " + planet.ResourcefulRating() + " / " + planet.ResourcefulCap());
            foreach(StarLane starLane in planet.StarLanes()) {
                Planet neighbor = starLane.Neighbor(planet);
                Debug.Log("Neighbor: " + neighbor.Name());
            }
        }
        */
    }

    public Player MainPlayer() {
        return mainPlayer;
    }

    public void NextTurn() {
        Debug.Log("Universe: Next Turn Clicked!");
        foreach(Player player in players) {
            player.NextTurn();
        }

        turnCount = turnCount + 1;
    }
}
