using UnityEngine;

namespace Spaceships.Resources {
    [CreateAssetMenu(fileName = "All Players", menuName = "Spaceships/Player Selection")]
    public class PlayerSelection : ScriptableObject {
        public PlayerSettings[] Players;
    }
}