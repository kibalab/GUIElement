[한국어](README.md) | **English** | [日本語](README.ja.md)

---

# GUIElement

GUIElement is a small editor-only IMGUI utility package for Unity editor tooling.

The current release provides reusable helpers for reorderable editor lists.

- `ReorderableListView`: handles drag-and-drop reordering for custom IMGUI lists.
- `SerializedArrayReorder`: moves `SerializedProperty` array elements with Undo and dirty handling.
- `DragHandleGUI`: draws a compact drag handle for list rows.

## Installation

Install this package from VCC after it is registered in the VPM repository.

For embedded development, copy this package folder into a Unity project:

```text
Packages/com.kibalab.guielement
```

## Quick Example

```csharp
using KIBA_.GUIElement.Reorder.Editor;
using UnityEditor;
using UnityEngine;

public sealed class ExampleWindow : EditorWindow
{
    private readonly ReorderableListView _listView = new ReorderableListView("example-list");

    private void OnGUI()
    {
        _listView.ClearItemRects();

        for (var i = 0; i < 3; i++)
        {
            var row = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
            _listView.AddItemRect(row);

            var handle = new Rect(row.x, row.y, 18f, row.height);
            if (DragHandleGUI.Draw(handle))
            {
                _listView.BeginReorderDrag(i);
            }

            EditorGUI.LabelField(new Rect(row.x + 22f, row.y, row.width - 22f, row.height), $"Item {i}");
        }

        _listView.DrawTailDropZone();
        _listView.HandleDragAndDrop((from, to) => Debug.Log($"Move {from} to {to}"));
    }
}
```

## Release Setup

This repository uses the KIBALAB VPM package template release workflow.

Before publishing, confirm these GitHub repository settings:

| Item | Value |
| --- | --- |
| Repository Variable `PACKAGE_NAME` | `com.kibalab.guielement` |
| Repository Variable `VPM_BACKEND_URL` | `https://vpm.kiba.red` |
| Repository Secret `VPM_API_KEY` | VPM backend API key |

`release.yml` falls back to `com.kibalab.guielement` when `PACKAGE_NAME` is not configured.

## Releasing

The Git tag must match `package.json`'s `version`.

```bash
git tag 0.1.0
git push origin 0.1.0
```

## License

MIT
