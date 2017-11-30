using System.Collections.Generic;
using UnityEngine;

namespace Spaceships {
    public class SmoothFollow : MonoBehaviour {
        public bool FollowPlayers = false;
        public List<Transform> Targets;
        public float Smoothness = 10;
        private Vector3 _lastTargetPosition;

        void AggregateTarget(Transform t, ref Vector2 mid, ref int numActive) {
            if (transform.gameObject.activeInHierarchy) {
                Vector3 pos = t.position;

                mid.x += pos.x;
                mid.y += pos.y;
                numActive++;
            }
        }

        void LateUpdate() {
            Vector2 mid = Vector2.zero;
            int numActive = 0;
            for (int i = 0; i < Targets.Count; i++) {
                AggregateTarget(Targets[i], ref mid, ref numActive);
            }

            if (FollowPlayers) {
                var players = GameMode.Instance.PlayersAlive;
                for (int i = 0; i < players.Count; i++) {
                    AggregateTarget(players[i].transform, ref mid, ref numActive);
                }
            }

            if (numActive > 0) {
                mid.x /= numActive;
                mid.y /= numActive;
            } else {
                mid = Vector2.zero;
            }

            // follow
            Vector3 target = new Vector3(mid.x, mid.y, transform.position.z);
            Vector3 current;
            current.x = Follow(transform.position.x, target.x, _lastTargetPosition.x, 1f / Smoothness, Time.deltaTime);
            current.y = Follow(transform.position.y, target.y, _lastTargetPosition.y, 1f / Smoothness, Time.deltaTime);
            current.z = target.z;

            // Set position
            _lastTargetPosition = target;
            transform.position = current;
        }

        private static float Follow(float current, float target, float lastTarget, float speed, float dt) {
            float t = dt * speed;
            float v = (target - lastTarget) / t;
            float f = current - lastTarget + v;
            return target - v + f * Mathf.Exp(-t);
        }
    }
}