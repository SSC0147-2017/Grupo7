using System;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {
    public Color Color;
    public int ID = -1;
    public PlayerRole Role = PlayerRole.None;

    public void Start() {
        // Find level
        Level level = SpaceshipsUtils.FindLevel(gameObject);
        if (!level) {
            return;
        }

        // Select prefab for role
        GameObject prefab;
        switch (Role) {
            case PlayerRole.Attacker:
                prefab = level.AttackerPrefab;
                break;
            case PlayerRole.Defender:
                prefab = level.DefenderPrefab;
                break;

            case PlayerRole.None:
            default:
                Debug.LogError("PlayerSpaner Invalid PlayerRole, won't spawn", this);
                return; // Won't spawn, return early
        }
        if (!prefab) {
            Debug.LogError("Prefab for" + Role + "is invalid", this);
            return;
        }

        // Spawn player
        var player = Instantiate(prefab, transform.position, transform.rotation);
        level.Players.Add(player);
        var follow = level.Camera.GetComponent<CameraFollow>();
        if (follow) {
            follow.Targets.Add(player.transform);
        }
        player.name = "Player " + (ID + 1);

        // Assign input
        var input = player.GetComponent<PlayerInput>();
        if (input) {
            input.PlayerID = ID;
        }

        // Assign color
        var customColor = player.GetComponent<CustomColor>();
        if (customColor) {
            customColor.Color = Color;
        }
    }
}