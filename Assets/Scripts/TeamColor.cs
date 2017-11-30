using System;
using UnityEngine;
using Spaceships.Util;

namespace Spaceships {
    public class TeamColor : MonoBehaviour {
        public bool ApplyOnAwake = true;
        public Team Team;

        public int[] MeshMaterialIndices;
        public bool ApplyOnSprite = false;

        [Serializable]
        public struct OnComponent {
            public string Type;
            public string Field;
        }

        public OnComponent[] Components;

        // @bug applying on start for now
        private void Start() {
            if (ApplyOnAwake) {
                ApplyColor();
            }
        }

        public void ApplyColor() {
            var color = GameMode.Instance.Teams[Team].Color;

            var mesh = GetComponent<MeshRenderer>();
            foreach (int i in MeshMaterialIndices) {
                mesh.materials[i].color = color;
            }

            if (ApplyOnSprite) {
                GetComponent<SpriteRenderer>().color = color;
            }

            foreach (var c in Components) {
                var comp = GetComponent(c.Type);
                comp.GetType().GetField(c.Field).SetValue(comp, color);
            }
        }
    }
}