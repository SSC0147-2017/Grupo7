using System.Runtime.CompilerServices;
using UnityEngine;

public class SpaceshipsUtils {
    public static bool IsPlayer(GameObject obj) {
        return obj.CompareTag("Player");
    }

    public static Level FindLevel(GameObject obj) {
        var level = obj.GetComponentInParent<Level>();
        if (!level) {
            Debug.Log("FindLevel returned null", obj);
        }
        return level;
    }
}

public enum PlayerRole {
    None,
    Attacker,
    Defender
}