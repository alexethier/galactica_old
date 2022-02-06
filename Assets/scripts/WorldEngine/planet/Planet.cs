using System.Collections;
using System.Collections.Generic;

public class Planet : IBaseObject
{

    // Commerce
    // Commerce Tech (industry building, planet building)
    // Maintain Ships
    // Maintain Upgrades
    // Expensive boosts
    // Expensive Purchase Upgrades
    // Planet Defense

    // Science
    // Science Tech (military, exploration, planet building)
    // Ship Research
    // Tech multiplier

    // Culture
    // Culture Tech (Culture, Industry, Exploration, Quests)
    // Cultural defense/offense
    // Military boosts
    // Defense Bonuses
    // Planet Defense

    // Industry
    // Planet Growth
    // Ship Construction

    private int planetId;
    private string name;

    private PlanetBaseTerrain baseTerrain;
    private int currentExotic;
    private int currentHospitable;
    private int currentWonderful;
    private int currentResourceful;

    // Rare Resources
    // Can be consumed to increase planet stats?
    // Later on can be mined and shipped to another planet (teleported from the core to form an orbiting satellite).
    // TODO:
    /*
    private bool hasAdamantium;
    private bool hasUltraHeavyWater;
    private bool hasAlientArtifact;
    private bool hasPrimitiveRace;
    private bool hasDust;
    */
    // etc...

    // OTHER STATS
    private int baseDefenseRating; // Lowers other ratings
    private List<StarLane> starLanes; // Number of connected planets, raises defense
    private Player owner;
    private PlanetSprite planetSprite;

    public Planet(int planetId) {
        string name = Util.GeneratePlanetName();
        this.init(planetId, name);
    }

    public Planet(int planetId, string name) {
        this.init(planetId, name);
    }

    private void init(int planetId, string name) {
        this.planetId = planetId;
        this.name = name;

        // Generate Planet Scores
        this.baseDefenseRating = Util.Select<int>(new List<int>{ 0, 0, 0, 1, 1, 2});
        this.baseTerrain = PlanetBaseTerrain.GenerateRandomBaseTerrain(baseDefenseRating);
        this.currentExotic = this.baseTerrain.GetBaseExoticElements();
        this.currentHospitable = this.baseTerrain.GetBaseHospitableElements();
        this.currentWonderful = this.baseTerrain.GetBaseWonderfulElements();
        this.currentResourceful = this.baseTerrain.GetBaseResourceElements();

        this.starLanes = new List<StarLane>();

        // Create UI element
        this.planetSprite = PlanetSprite.Create(this);
    }

    public int Id() {
        return planetId;
    }

    public string Name() {
        return name;
    }

    public Player Owner() {
        return owner;
    }

    public List<StarLane> StarLanes() {
        return starLanes;
    }

    public void AddStarLane(StarLane starLane) {
        starLanes.Add(starLane);
    }

    public PlanetSprite GetPlanetSprite() {
        return planetSprite;
    }

    public void SetOwner(Player player) {
        owner = player;
    }

    // Terrain Getters
    public int ExoticRating() {
        return currentExotic;
    }

    public int HospitableRating() {
        return currentHospitable;
    }

    public int WonderfulRating() {
        return currentWonderful;
    }

    public int ResourcefulRating() {
        return currentResourceful;
    }

    public int ExoticCap() {
        return baseTerrain.GetProficientExoticElements();
    }

    public int HospitableCap() {
        return baseTerrain.GetProficientHospitableElements();
    }

    public int WonderfulCap() {
        return baseTerrain.GetProficientWonderfulElements();
    }

    public int ResourcefulCap() {
        return baseTerrain.GetProficientResourceElements();
    }
    // End Terrain Getters

    // Terrain Setters
    public void SetExoticRating(int rating) {
        currentExotic = rating;
    }

    public void SetHospitableRating(int rating) {
        currentHospitable = rating;
    }

    public void SetWonderfulRating(int rating) {
        currentWonderful = rating;
    }

    public void SetResourcefulRating(int rating) {
        currentResourceful = rating;
    }

    public void SetPosition(double x, double y) {
        planetSprite.SetPosition(x, y);
        foreach(StarLane starLane in starLanes) {
            starLane.RefreshPosition();
        }
    }
}
