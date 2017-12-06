using System;
using System.Collections;
using System.Linq;
using Spaceships.Resources;
using UnityEngine;
using Spaceships.Util;

namespace Spaceships {

    // @TODO there's no order to what's happening here
    // @TODO AllPlayers RuntimeSet is unreliable on level transitions
    public class GateEntrance : MonoBehaviour {

        public GameEvent NewLevelLoaded;

        private IEnumerator OnTriggerEnter(Collider other) {
            //@TODO wait for all players in team
            if (other.gameObject.IsPlayer()) {
                if (!GameMode.Instance.EndLevelTransition()) {
                    yield break;
                }

                // Spawn new players
                var level = gameObject.FindOwningLevel();
                var playerEntering = other.GetComponent<PlayerController>();
                foreach (var t in Enum.GetValues(typeof(Team))) {
                    Team team = (Team) t;
                    // @TODO maybe we want to respawn all dead players
                    if (team != playerEntering.Settings.Team) {
                        level.Spawners[team].Spawn();
                    }
                }

                // Finished spawning teams, re-track everyone
                NewLevelLoaded.Raise();

                // @TODO this is temporary, we should rethink getcamerabounds
                foreach (var gate in level.Gates) {
                    if (gate && gate.transform != transform.parent)
                        gate.Close();
                }

                // This is a dirty hack part 1
                transform.parent.GetComponent<Gate>().Block.SetActive(true);
                var ortho = FindObjectOfType<OrthographicLimits>();
                ortho.enabled = false;

                // This is a dirty hack part 2
                yield return new WaitForSeconds(1f);
                transform.parent.GetComponent<Gate>().Close();
                ortho.enabled = true;
            }
        }

    }

}