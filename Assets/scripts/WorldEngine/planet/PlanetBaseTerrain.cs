using System;
using System.Collections;
using System.Collections.Generic;

public class PlanetBaseTerrain
{
    private Dictionary<string, int> exoticTerrainMap;
    private Dictionary<string, int> hospitableTerrainMap;
    private Dictionary<string, int> wonderfulTerrainMap;
    private Dictionary<string, int> resourcefulTerrainMap;
    // TODOs: Certain techs can unlock new resources to be harvested.

    private static string BASE_KEY = "base.key";
    private static string PROFICIENT_KEY = "proficient.key";

    // TODO Advanced - Certain techs will unlock advanced stats
    /*
    private ADVANCED_EXOTIC_ELEMENTS_KEY = "advanced.exotic.key";
    private ADVANCED_HOSPITABLE_AREA_KEY = "advanced.hospitable.key";
    private ADVANCED_WONDERFUL_AREA_KEY = "advanced.wonderful.key";
    private ADVANCED_RESOURCEFUL_KEY = "advanced.resourceful.key";
    */

    public PlanetBaseTerrain(Dictionary<string, int> exoticTerrainMap, Dictionary<string, int> hospitableTerrainMap, Dictionary<string, int> wonderfulTerrainMap, Dictionary<string, int> resourcefulTerrainMap) {
        this.exoticTerrainMap = exoticTerrainMap;
        this.hospitableTerrainMap = hospitableTerrainMap;
        this.wonderfulTerrainMap = wonderfulTerrainMap;
        this.resourcefulTerrainMap = resourcefulTerrainMap;
    }

    public static PlanetBaseTerrain GenerateRandomBaseTerrain(int defenseRating) {
        Dictionary<string, int> exoticTerrainMap = new Dictionary<string, int>();
        Dictionary<string, int> hospitableTerrainMap = new Dictionary<string, int>();
        Dictionary<string, int> wonderfulTerrainMap = new Dictionary<string, int>();
        Dictionary<string, int> resourcefulTerrainMap = new Dictionary<string, int>();        
        List<Dictionary<string, int>> terrains = new List<Dictionary<string, int>>{ exoticTerrainMap, hospitableTerrainMap, wonderfulTerrainMap, resourcefulTerrainMap };

        List<int> baseValuesList = new List<int>{ 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 4, 4, 5 };
        foreach(Dictionary<string,int> terrain in terrains) {
            terrain[BASE_KEY] = Util.Select<int>(baseValuesList);
        }
        
        List<int> proficientModifierList = new List<int>{ 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 6, 7, 7, 7, 8, 9 };
        foreach(Dictionary<string, int> terrain in terrains) {
            terrain[PROFICIENT_KEY] = Math.Max(2 + terrain[BASE_KEY], Util.Select<int>(proficientModifierList));
        }

        // Lower planet's stats by defense rating
        for(int i=0; i < 3; i++) {
            Dictionary<string, int> terrain = Util.Select<Dictionary<string, int>>(terrains);
            terrain[PROFICIENT_KEY] = Math.Max(1 + terrain[BASE_KEY], terrain[PROFICIENT_KEY] - defenseRating);
        }

        return new PlanetBaseTerrain(exoticTerrainMap, hospitableTerrainMap, wonderfulTerrainMap, resourcefulTerrainMap);        
    }

    // Base
    public int GetBaseExoticElements() {
        return exoticTerrainMap[BASE_KEY];
    }
    public int GetBaseHospitableElements() {
        return hospitableTerrainMap[BASE_KEY];
    }
    public int GetBaseWonderfulElements() {
        return wonderfulTerrainMap[BASE_KEY];
    }
    public int GetBaseResourceElements() {
        return resourcefulTerrainMap[BASE_KEY];
    }

    // Proficient
    public int GetProficientExoticElements() {
        return exoticTerrainMap[PROFICIENT_KEY];
    }
    public int GetProficientHospitableElements() {
        return hospitableTerrainMap[PROFICIENT_KEY];
    }
    public int GetProficientWonderfulElements() {
        return wonderfulTerrainMap[PROFICIENT_KEY];
    }
    public int GetProficientResourceElements() {
        return resourcefulTerrainMap[PROFICIENT_KEY];
    }

    // Advanced
    /*
    public int getAdvancedExoticElements() {
        return exoticTerrainMap[ADVANCED_EXOTIC_ELEMENTS_KEY];
    }
    public int getAdvancedHospitableElements() {
        return hospitableTerrainMap[ADVANCED_HOSPITABLE_AREA_KEY];
    }
    public int getAdvancedWonderfulElements() {
        return wonderfulTerrainMap[ADVANCED_WONDERFUL_AREA_KEY];
    }
    public int getAdvancedResourceElements() {
        return resourcefulTerrainMap[ADVANCED_RESOURCEFUL_KEY];
    }
    */
}
