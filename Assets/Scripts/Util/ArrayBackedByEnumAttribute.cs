using System;
using UnityEngine;

namespace Spaceships.Util {
    public class ArrayBackedByEnumAttribute : PropertyAttribute {
        public readonly Type EnumType;
        public readonly bool CanHaveNull;

        public ArrayBackedByEnumAttribute(Type enumType, bool canHaveNull = true) {
            EnumType = enumType;
            CanHaveNull = canHaveNull;
        }
    }
}