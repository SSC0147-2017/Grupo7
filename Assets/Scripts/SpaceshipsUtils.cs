using System;
using UnityEngine;

public class SpaceshipsUtils {
    public static bool IsPlayer(GameObject obj) {
        return obj.CompareTag("Player");
    }

    public static Level FindLevel(GameObject obj) {
        var level = obj.GetComponentInParent<Level>();
        if (!level) {
            throw new NullReferenceException("Can't find level");
        }
        return level;
    }
}

public enum PlayerRole {
    None,
    Attacker,
    Defender
}

public enum Direction {
    None,
    Forward,
    Right,
    Up
}