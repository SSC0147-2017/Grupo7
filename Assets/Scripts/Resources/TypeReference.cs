using System;
using UnityEngine;

namespace Spaceships.Resources {

    [Serializable]
    public class TypeReference<T> : ScriptableObject {

        public TypeVariable<T> Variable;
        public bool UseConstant = true;
        public T ConstantValue;

        public TypeReference() {
        }

        public TypeReference(T value) {
            ConstantValue = value;
            UseConstant = true;
        }

        public TypeReference(TypeVariable<T> variable) {
            Variable = variable;
            UseConstant = false;
        }

        public T Value {
            get { return UseConstant ? ConstantValue : Variable.Value;  }
            set {
                if (UseConstant) {
                    ConstantValue = value;
                } else {
                    Variable.Value = value;
                }
            }
        }

    }

}