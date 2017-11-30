using UnityEngine;

namespace Spaceships {
    public class Gate : MonoBehaviour {
        public GameObject Block;
        public GameObject Entrance;

        public void OpenForLeaving() {
            Block.SetActive(false);
        }

        public void OpenForEntering() {
            Block.SetActive(false);
            Entrance.SetActive(true);
        }

        public void Close() {
            Block.SetActive(true);
            Entrance.SetActive(false);
        }
    }
}