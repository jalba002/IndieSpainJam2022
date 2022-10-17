// #if UNITY_EDITOR
//
// using CosmosDefender;
// using UnityEditor;
// using UnityEngine;
//
// [CustomEditor(typeof(PurchaseableSpellModifierData))]
// [CanEditMultipleObjects]
// public class PurchaseableSpellModifierDataEditor : Editor
// {
//     SerializedProperty desc;
//     
//     /*
//         private string description;
//         private Sprite thumbnail;
//         private int price;
//         private bool canBePurchased = true;
//         public T modifier;
//      */
//
//     void OnEnable()
//     {
//         desc = serializedObject.FindProperty("description");
//     }
//
//     public override void OnInspectorGUI()
//     {
//         base.DrawDefaultInspector();
//         return;
//         serializedObject.Update();
//         GUIStyle style = new GUIStyle(GUI.skin.textArea)
//         {
//             richText = true, 
//             wordWrap = true, 
//             fontSize = 24,
//             border = new RectOffset(20,20,20,20),
//             padding = new RectOffset(5,5,5,5)
//         };
//         desc.stringValue = EditorGUILayout.TextArea(desc.stringValue, style);
//         serializedObject.ApplyModifiedProperties();
//     }
// }
// #endif