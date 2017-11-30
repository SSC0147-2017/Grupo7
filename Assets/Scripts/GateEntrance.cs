using System;
using System.Collections;
using UnityEngine;
using Spaceships.Util;

namespace Spaceships {
    public class GateEntrance : MonoBehaviour {
        private IEnumerator OnTriggerEnter(Collider other) {

            //@TODO wait for all players in team
            if (other.gameObject.IsPlayer()) {
                if (!GameMode.Instance.EndLevelTransition()) {
                    yield break;
                }

                var level = gameObject.FindOwningLevel();
                var playerEntering = other.GetComponent<PlayerController>();
                foreach (var t in Enum.GetValues(typeof(Team))) {
                    Team team = (Team) t;
                    // @TODO Maybe we want to respawn all dead players
                    if (team != playerEntering.Settings.Team) {
                        level.Spawners[team].Spawn();
                    }
                }

                // @TODO this is temporary, we should rethink
                // getcamerabounds so it smoothly transitions
                foreach (var gate in level.Gates) {
                    if (gate && gate.transform != transform.parent)
                        gate.Close();
                }
                transform.parent.GetComponent<Gate>().Block.SetActive(true);
                var ortho = FindObjectOfType<OrthographicLimits>();
                ortho.enabled = (false);
                yield return new WaitForSeconds(2f);
                transform.parent.GetComponent<Gate>().Close();
                ortho.enabled = (true);
            }
        }
    }
}