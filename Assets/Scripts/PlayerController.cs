using Spaceships.Resources;
using Rewired;
using UnityEngine;

namespace Spaceships {
    public class PlayerController : MonoBehaviour {
        public PlayerSettings Settings;

        private Player _control;

        public float TimeAliveClampedTemporaryAndUgly;

        private void OnEnable() {
            TimeAliveClampedTemporaryAndUgly = 0f;
        }

        private void Update() {
            TimeAliveClampedTemporaryAndUgly += Time.deltaTime;
            TimeAliveClampedTemporaryAndUgly = Mathf.Clamp01(TimeAliveClampedTemporaryAndUgly);
        }


        public Player Control {
            get {
                if (!ReInput.isReady) {
                    return null;
                }

                if (_control == null) {
                    _control = ReInput.players.GetPlayer(Settings.Controller);
                }

                return _control;
            }
        }
    }
}