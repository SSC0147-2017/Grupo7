using System;
using Spaceships.Util;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Spaceships.Editor {
    [CustomPropertyDrawer(typeof(ArrayBackedByEnumAttribute))]
    public class ArrayBackedByEnumDrawer : PropertyDrawer {
        public const string ArrayFieldName = "Array";

        private bool _expanded = true;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            // Get attribute
            var attr = (ArrayBackedByEnumAttribute) attribute;
            Assert.IsTrue(attr.EnumType.IsEnum);

            // Get enum data
            var names = Enum.GetNames(attr.EnumType);
            var values = Enum.GetValues(attr.EnumType);

            // Resize array
            var array = property.FindPropertyRelative(ArrayFieldName);
            if (array == null) {
                Debug.LogError("Can't find " + ArrayFieldName + " on " + property.type + ".\n"
                               + "Did you forget to inherit from ClassWithArray?");
                return;
            }
            if (array.arraySize != names.Length) {
                array.serializedObject.Update();
                array.arraySize = names.Length;
                array.serializedObject.ApplyModifiedProperties();
            }

            // Draw group
            EditorGUI.BeginProperty(position, label, property);
            label.text += " (by " + attr.EnumType.Name + ")";
            _expanded = EditorGUI.Foldout(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), _expanded, label,
                true);
            float offsetY = EditorGUI.GetPropertyHeight(array, false) + EditorGUIUtility.standardVerticalSpacing;

            // Draw children
            EditorGUI.indentLevel++;
            if (_expanded) {
                Assert.AreEqual(array.arraySize, names.Length);
                for (int i = 0; i < array.arraySize; ++i) {
                    var element = array.GetArrayElementAtIndex(i);
                    var elementHeight = EditorGUI.GetPropertyHeight(element, true);
                    var elementRect = new Rect(position.x, position.y + offsetY, position.width, elementHeight);

                    EditorGUI.PropertyField(elementRect, element, new GUIContent(names[i]));

                    offsetY += elementHeight + EditorGUIUtility.standardVerticalSpacing;

                    // check if enum value is valid
                    Assert.AreEqual((int) values.GetValue(i), i,
                        "Expected " + attr.EnumType + "." + names[i] + " == " + i);
                }
            }
            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            var array = property.FindPropertyRelative(ArrayFieldName);
            float height = EditorGUI.GetPropertyHeight(array, false) + EditorGUIUtility.standardVerticalSpacing;
            if (array == null) {
                Debug.LogError("Can't find " + ArrayFieldName + " on " + property.type + ".\n"
                               + "Did you forget to inherit from ClassWithArray?");
            } else if (_expanded) {
                for (int i = 0; i < array.arraySize; i++) {
                    height += EditorGUI.GetPropertyHeight(array.GetArrayElementAtIndex(i), true) +
                              EditorGUIUtility.standardVerticalSpacing;
                }
            }
            return height;
        }
    }
}