# GUIElement

Editor-only IMGUI utility elements for Unity editor tooling.

## APIs

### `ReorderableListView`

Tracks item rects, handles Unity `DragAndDrop`, draws an insertion marker, and reports a source and destination index.

```csharp
var view = new ReorderableListView("my-list");
view.ClearItemRects();
view.AddItemRect(rowRect);
view.DrawTailDropZone();
view.HandleDragAndDrop((from, to) => MoveItem(from, to));
```

### `DragHandleGUI`

Draws a compact drag handle and returns `true` when the handle starts a drag.

```csharp
if (DragHandleGUI.Draw(handleRect))
{
    view.BeginReorderDrag(index);
}
```

### `SerializedArrayReorder`

Moves an array element on a `SerializedProperty` and records Undo for the target object.

```csharp
SerializedArrayReorder.Move(serializedObject, arrayProperty, from, to, target, "Reorder Items");
```

## Namespace

```csharp
using KIBA_.GUIElement.Reorder.Editor;
```

## Requirements

- Unity 2021.3 or newer
- Editor only

## License

MIT
