using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Occupiable : MonoBehaviour
{
    protected readonly string[] typeOptions = { "All", "ID" };
    protected int typeIndex = 0;
    protected int targetID = 0;

    [HideInInspector, SerializeField]
    public UnityEvent onPlaced;

    private void OnTriggerEnter(Collider other)
    {
        if (CheckPlaced(other))
        {
            onPlaced.Invoke();
        }
    }

    private bool CheckPlaced(Collider placed)
    {
        Grabbable grabbable = placed.transform.GetComponent<Grabbable>();
        if(grabbable == null) return false;

        switch (typeOptions[typeIndex])
        {
            case "All":
                return true;

            case "ID":
                if (grabbable == null)
                    return false;

                return targetID == grabbable.GetID();

            default:
                return false;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Occupiable))]
    public class OccupiableEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Occupiable occupiable = target as Occupiable;

            serializedObject.Update();

            occupiable.typeIndex = EditorGUILayout.Popup("Target ID", occupiable.typeIndex, occupiable.typeOptions);

            switch (occupiable.typeOptions[occupiable.typeIndex])
            {
                case "ID":
                    occupiable.targetID = EditorGUILayout.IntField("Type", occupiable.targetID);
                    break;
            }

            EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onPlaced"));

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
