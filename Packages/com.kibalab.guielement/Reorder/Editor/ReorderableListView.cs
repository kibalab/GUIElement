using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KIBA_.GUIElement.Reorder.Editor
{
    public sealed class ReorderableListView
    {
        private readonly string _dragKey;
        private readonly List<Rect> _itemRects = new List<Rect>();
        private Rect _tailDropRect;
        private int _dragSourceIndex = -1;
        private int _dragInsertIndex = -1;
        private readonly float _tailDropZoneHeight;


        public ReorderableListView(string dragKey, float tailDropZoneHeight = 22f)
        {
            _dragKey = dragKey;
            _tailDropZoneHeight = tailDropZoneHeight;
        }


        public void ClearItemRects()
        {
            _itemRects.Clear();
            _tailDropRect = default;
        }


        public void AddItemRect(Rect rect)
        {
            _itemRects.Add(rect);
        }


        public Rect DrawTailDropZone()
        {
            var r = GUILayoutUtility.GetRect(0f, _tailDropZoneHeight, GUILayout.ExpandWidth(true));
            UnityEditor.EditorGUI.DrawRect(r, new Color(0, 0, 0, 0));
            _tailDropRect = r;
            return r;
        }


        public void BeginReorderDrag(int index)
        {
            DragAndDrop.PrepareStartDrag();
            DragAndDrop.SetGenericData(_dragKey, index);
            DragAndDrop.StartDrag("Move Item");
            _dragSourceIndex = index;
        }


        public void HandleDragAndDrop(Action<int, int> onReorder)
        {
            var e = Event.current;
            var data = DragAndDrop.GetGenericData(_dragKey);


            if ((e.type == EventType.DragUpdated || e.type == EventType.MouseDrag) && data is int src)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                _dragSourceIndex = src;
                _dragInsertIndex = ComputeInsertIndex(e.mousePosition.y);
                e.Use();
            }
            else if (e.type == EventType.Repaint && _dragSourceIndex >= 0 && _dragInsertIndex >= 0)
            {
                DrawInsertionMarker(_dragInsertIndex);
            }
            else if (e.type == EventType.DragPerform && data is int src2)
            {
                DragAndDrop.AcceptDrag();
                int dst = Mathf.Clamp(_dragInsertIndex, 0, _itemRects.Count);
                if (dst > src2) dst -= 1;
                if (dst != src2 && dst >= 0 && dst <= _itemRects.Count - 1)
                {
                    onReorder?.Invoke(src2, dst);
                }

                ClearDragState();
                e.Use();
            }
            else if (e.type == EventType.DragExited)
            {
                ClearDragState();
            }
        }

        private int ComputeInsertIndex(float mouseY)
        {
            if (_itemRects.Count == 0) return 0;
            for (int i = 0; i < _itemRects.Count; i++)
            {
                var mid = _itemRects[i].y + _itemRects[i].height * 0.5f;
                if (mouseY < mid) return i;
            }

            var last = _itemRects[_itemRects.Count - 1];
            if (mouseY >= last.yMax || _tailDropRect.Contains(new Vector2(last.x, mouseY))) return _itemRects.Count;
            return _itemRects.Count - 1;
        }


        private void DrawInsertionMarker(int insertIndex)
        {
            Rect lineRect;
            if (insertIndex <= 0)
            {
                var first = _itemRects[0];
                lineRect = new Rect(first.x, first.y - 1f, first.width, 2f);
            }
            else if (insertIndex >= _itemRects.Count)
            {
                var last = _itemRects[_itemRects.Count - 1];
                lineRect = new Rect(last.x, last.yMax - 1f, last.width, 2f);
            }
            else
            {
                var prev = _itemRects[insertIndex - 1];
                lineRect = new Rect(prev.x, prev.yMax - 1f, prev.width, 2f);
            }

            var col = EditorGUIUtility.isProSkin ? new Color(0.25f, 0.6f, 1f, 0.9f) : new Color(0.1f, 0.4f, 0.9f, 0.9f);
            UnityEditor.EditorGUI.DrawRect(lineRect, col);
        }


        private void ClearDragState()
        {
            _dragSourceIndex = -1;
            _dragInsertIndex = -1;
            DragAndDrop.SetGenericData(_dragKey, null);
        }
    }


    public static class DragHandleGUI
    {
        public static bool Draw(Rect rect)
        {
            var id = GUIUtility.GetControlID(FocusType.Passive);
            var e = Event.current;
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Pan);
            if (e.type == EventType.Repaint)
            {
                var c = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.3f) : new Color(0f, 0f, 0f, 0.4f);
                var r = new Rect(rect.x + 4, rect.y + (rect.height - 10f) * 0.5f, rect.width - 8f, 10f);
                var step = 2f;
                for (float y = 0; y < r.height; y += step) UnityEditor.EditorGUI.DrawRect(new Rect(r.x, r.y + y, r.width, 1f), c);
            }

            if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
            {
                GUIUtility.hotControl = id;
                e.Use();
                return true;
            }

            if (e.type == EventType.MouseUp && GUIUtility.hotControl == id)
            {
                GUIUtility.hotControl = 0;
                e.Use();
            }

            return false;
        }
    }
}
