using System;
using UnityEngine;

namespace Spaceships.Resources {

    [Serializable]
    public abstract class TypeVariable<T> : ScriptableObject {

        private T value;

        public abstract T Value { get; set; }

    }

}