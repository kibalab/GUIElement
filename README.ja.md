[한국어](README.md) | [English](README.en.md) | **日本語**

---

# GUIElement

GUIElement は、Unity Editor 拡張ツールで再利用するための小さな Editor 専用 IMGUI ユーティリティパッケージです。

現在のリリースでは、並び替え可能なエディターリスト用のヘルパーを提供します。

- `ReorderableListView`: カスタム IMGUI リストのドラッグアンドドロップ並び替えを処理します。
- `SerializedArrayReorder`: `SerializedProperty` 配列の移動を Undo と Dirty 処理付きで適用します。
- `DragHandleGUI`: リスト行用のコンパクトなドラッグハンドルを描画します。

## インストール

VPM リポジトリに登録された後、VCC からインストールします。

埋め込み開発では、このパッケージフォルダーを Unity プロジェクトへコピーします。

```text
Packages/com.kibalab.guielement
```

## クイック例

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

## リリース設定

このリポジトリは KIBALAB VPM パッケージテンプレートのリリースワークフローを使用します。

公開前に GitHub リポジトリ設定を確認してください。

| 項目 | 値 |
| --- | --- |
| Repository Variable `PACKAGE_NAME` | `com.kibalab.guielement` |
| Repository Variable `VPM_BACKEND_URL` | `https://vpm.kiba.red` |
| Repository Secret `VPM_API_KEY` | VPM backend API key |

`release.yml` は `PACKAGE_NAME` が未設定の場合でも `com.kibalab.guielement` を既定値として使用します。

## リリース

Git タグは `package.json` の `version` と一致している必要があります。

```bash
git tag 0.1.0
git push origin 0.1.0
```

## ライセンス

MIT
