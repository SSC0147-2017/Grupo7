using System.Collections;
using Cinemachine;
using Spaceships.Resources;
using UnityEngine;
using Spaceships.Util;

namespace Spaceships {
    public class TeamSpawner : MonoBehaviour {
        public Team Team;
        // public Transform[] SpawnPoints;
        
        public void Spawn() {
            if (!gameObject.activeInHierarchy) {
                return;
            }
            TeamSettings teamSettings = GameMode.Instance.Teams[Team];
            Level level = gameObject.FindOwningLevel();

            // Select prefab for role
            GameObject prefab = level.Prefabs[teamSettings.Role];

            var allPlayerSettings = GameMode.Instance.PlayerSelection.Players;
            for (int i = 0; i < allPlayerSettings.Length; ++i) {
                if (allPlayerSettings[i].Team == Team) {
                    var playerSettings = allPlayerSettings[i];

                    // create player
                    var obj = Instantiate(prefab, transform.position, transform.rotation);
                    obj.name = "Player " + allPlayerSettings[i].Name;

                    var player = obj.GetComponent<PlayerController>();
                    player.Settings = playerSettings;

                    var colors = obj.GetComponentsInChildren<TeamColor>();
                    foreach (var color in colors) {
                        color.Team = Team;
                    }

                    GameMode.Instance.PlayersAlive.Add(player);
                }
            }
        }
    }
}