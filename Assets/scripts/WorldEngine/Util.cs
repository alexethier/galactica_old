using System.Collections;
using System.Collections.Generic;
using System;

public class Util
{

    private static List<string> PLAYER_NAMES = new List<string>{ "Regalus", "Maul", "Imperium", "Vulcan", "Udari", "Voidlings", "Sectoids", "Asari", "Alterians", "Borg" };
    private static List<string> PLAYER_ADJECTIVES = new List<string>{ "Horrific", "Magnificent", "Wise", "Zealous", "Great", "Slumbering", "Undying", "Vengeful", "Awakened" };
    public static string GeneratePlayerName() {
        string selectedName = PLAYER_NAMES[Util.Random().Next(PLAYER_NAMES.Count)];
        string selectedAdjective = PLAYER_ADJECTIVES[random.Next(PLAYER_ADJECTIVES.Count)];
        return selectedAdjective + " " + selectedName;
    }

    private static List<string> PLANET_NAMES = new List<string>{ "Onterra", "Zeshera", "Pandora", "Oceana", "Axios", "Consul", "Zomg", "Orange", "Gargantua", "Splint", "Semaphore", "Boroque", "Bottle", "Yapple", "Scorn", "Arrakis", "Caladan", "New Terra", "Terminus", "Klendathu" };
    private static HashSet<string> chosenNames = new HashSet<string>();
    public static string GeneratePlanetName() {
        string selectedName = PLANET_NAMES[Util.Random().Next(PLANET_NAMES.Count)];
        if(chosenNames.Count == PLANET_NAMES.Count) {
            throw new Exception("Could not generate a unique planet name.");
        }
        if(chosenNames.Contains(selectedName)) {
            return Util.GeneratePlanetName();
        } else {
            chosenNames.Add(selectedName);
            return selectedName;
        }
    }

    private static Random random;
    public static Random Random() {
        if(random == null) {
            random = new Random();
        }
        return random;
    }

    public static T Select<T>(List<T> list) {
        int index = Util.Random().Next(list.Count);
        return list[index];
    }

    public static T Select<T>(List<T> list, List<T> exclude) where T : IBaseObject {

        int index = Util.Random().Next(list.Count);
        return list[index];
    }
}
