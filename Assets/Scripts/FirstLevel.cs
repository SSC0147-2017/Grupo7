using UnityEngine;

public class FirstLevel : MonoBehaviour {
    public Level Level;

    void Awake() {
        var levels = FindObjectsOfType<Level>();
        if (!Level) {
            Debug.LogError("First Level is invalid, activating the first one found");
            Level = levels[0];
        }

        for (int i = 0; i < levels.Length; i++) {
            levels[i].gameObject.SetActive(Level == levels[i]);
        }
    }
}