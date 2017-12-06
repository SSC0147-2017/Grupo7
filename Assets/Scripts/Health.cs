using Spaceships.Resources;
using Spaceships.Util;
using UnityEngine;

namespace Spaceships {

    public class Health : MonoBehaviour {

        // Health
        public int HP;

        public int MaxHP = 3;
        public PlayersRuntimeSet AllPlayers;
        public PlayersRuntimeSet DeadPlayers;
        public PlayersRuntimeSet ActivePlayers;

        public bool IsAlive {
            get { return HP > 0; }
        }

        // Player Events
        public GameObject DieEffect;

        public GameEvent PlayerKilled;
        public GameEvent PlayerDamaged;
        public GameEvent PlayerRevived;

        // Player references
        private PlayerController controller;

        private void Awake() {
            controller = gameObject.GetComponent<PlayerController>();
            AllPlayers.Insert(controller); // Before start so camera can track this
            HP = MaxHP;
        }

        private void OnEnable() {
            ActivePlayers.Insert(controller);
            DeadPlayers.Remove(controller);
        }

        private void OnDisable() {
            ActivePlayers.Remove(controller);
            DeadPlayers.Insert(controller);
        }

        private void OnDestroy() {
            ActivePlayers.Remove(controller);
            DeadPlayers.Remove(controller);
            AllPlayers.Remove(controller);
        }

        public void TakeDamage(GameObject instigator, int damage = 1) {
            HP = Mathf.Max(HP - damage, 0);

            if (HP == 0) {  // player killed
                Quaternion rotation = Quaternion.FromToRotation(Vector3.up, Vector3.back);
                Instantiate(DieEffect, transform.position, rotation);
                gameObject.SetActive(false);
                PlayerKilled.Raise(); // will trigger camera un-tracking
            }
        }

        public void Revive() {
            if (!IsAlive) {
                HP = MaxHP;
                gameObject.SetActive(true);
                PlayerRevived.Raise(); // will trigger camera re-tracking
            }
        }

    }

}