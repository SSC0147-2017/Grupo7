using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Spaceships.Util {
    public static class SpaceshipsUtils {
        public static bool IsPlayer(this GameObject obj) {
            return obj.CompareTag("Player");
        }

        public static Level FindOwningLevel(this GameObject obj) {
            var level = obj.GetComponent<Level>();
            if (level) {
                return level;
            }

            level = obj.GetComponentInParent<Level>();
            if (!level) {
                throw new NullReferenceException("Can't find level");
            }
            return level;
        }

        public static BoxCollider FindOwningLevelBounds(this GameObject obj) {
            return obj.FindOwningLevel().GetComponent<BoxCollider>();
        }

        public static Vector3 GetVector(this Side side) {
            Vector3 result;
            switch (side) {
                case Side.Left:
                    result = Vector3.left;
                    break;
                case Side.Right:
                    result = Vector3.right;
                    break;
                case Side.Up:
                    result = Vector3.up;
                    break;
                case Side.Down:
                    result = Vector3.down;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("side", side, null);
            }
            return result;
        }

        public static Side GetOpposite(this Side side) {
            Side result;
            switch (side) {
                case Side.Left:
                    result = Side.Right;
                    break;
                case Side.Right:
                    result = Side.Left;
                    break;
                case Side.Up:
                    result = Side.Down;
                    break;
                case Side.Down:
                    result = Side.Up;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return result;
        }

        // @TODO find only objects below parent in hierarchy
        public static void DestroyTemporaries(this GameObject parent) {
            var temporaries = GameObject.FindGameObjectsWithTag("Temporary");
            foreach (var temp in temporaries) {
                Object.Destroy(temp);
            }
        }

        public static T RandomItem<T>(this IList<T> list) {
            return list[Random.Range(0, list.Count)];
        }
    }

    public enum Side {
        Left,
        Right,
        Up,
        Down
    }

    public enum Team {
        Red,
        Blue
    }

    public enum Role {
        Attacker,
        Defender
    }
}