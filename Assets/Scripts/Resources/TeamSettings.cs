using UnityEngine;
using Spaceships.Util;

namespace Spaceships.Resources {
    [CreateAssetMenu(fileName = "Team Settings", menuName = "Spaceships/Team Settings")]
    public class TeamSettings : ScriptableObject {
        [Header("Fixed data")] public string Name;
        public Team Team;
        public Side Side;
        public Color Color;

        [Header("Dynamic data")] public Role Role = Role.Attacker;
    }
}