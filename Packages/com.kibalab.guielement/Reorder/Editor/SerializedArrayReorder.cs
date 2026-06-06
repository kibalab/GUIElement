using UnityEditor;
using UnityEngine;

namespace KIBA_.GUIElement.Reorder.Editor
{
    public static class SerializedArrayReorder
    {
        public static void Move(SerializedObject owner, SerializedProperty arrayProp, int from, int to, Object dirtyTarget, string undoName)
        {
            Undo.RecordObject(dirtyTarget, undoName);
            arrayProp.MoveArrayElement(from, to);
            owner.ApplyModifiedProperties();
            EditorUtility.SetDirty(dirtyTarget);
        }
    }
}
