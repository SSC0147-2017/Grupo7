using System;
using UnityEngine;
using Spaceships.Util;

namespace Spaceships {
    public class Level : MonoBehaviour {
        [Serializable]
        public class PrefabArray : ClassWithArray<GameObject> { }

        [ArrayBackedByEnum(typeof(Role))] public PrefabArray Prefabs;

        [Serializable]
        public class SpawnArray : ClassWithArray<TeamSpawner> { }

        [ArrayBackedByEnum(typeof(Team))] public SpawnArray Spawners;

        [Serializable]
        public class GateArray : ClassWithArray<Gate> { }

        [ArrayBackedByEnum(typeof(Side))] public GateArray Gates;

        [HideInInspector] private BoxCollider _box;

        public Bounds Bounds {
            get {
                if (!_box) {
                    _box = GetComponent<BoxCollider>();
                }
                return _box.bounds;
            }
        }
    }
}