**한국어** | [English](README.en.md) | [日本語](README.ja.md)

---

# GUIElement

GUIElement는 Unity Editor 확장 도구에서 재사용하기 위한 작은 IMGUI 유틸리티 패키지입니다.

현재 배포판은 에디터 전용 배열 재정렬 UI를 제공합니다.

- `ReorderableListView`: 커스텀 IMGUI 리스트에서 드래그 앤 드롭 재정렬을 처리합니다.
- `SerializedArrayReorder`: `SerializedProperty` 배열 이동을 Undo/Dirty 처리와 함께 적용합니다.
- `DragHandleGUI`: 리스트 항목의 드래그 핸들을 그립니다.

## 설치

VPM/VCC 저장소에 이 패키지가 등록된 뒤 VCC에서 설치합니다.

직접 Unity 프로젝트에 임베드하려면 이 저장소의 패키지 폴더를 복사합니다.

```text
Packages/com.kibalab.guielement
```

## 빠른 사용 예시

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

## 배포 설정

이 저장소는 KIBALAB VPM 패키지 템플릿의 릴리스 워크플로우를 사용합니다.

릴리스 전에 GitHub 저장소 설정에서 다음 값을 확인하세요.

| 항목 | 값 |
| --- | --- |
| Repository Variable `PACKAGE_NAME` | `com.kibalab.guielement` |
| Repository Variable `VPM_BACKEND_URL` | `https://vpm.kiba.red` |
| Repository Secret `VPM_API_KEY` | VPM 백엔드 API 키 |

`release.yml`은 `PACKAGE_NAME`이 없을 때도 `com.kibalab.guielement`를 기본값으로 사용합니다.

## 릴리스

`package.json`의 `version`과 Git 태그가 일치해야 합니다.

```bash
git tag 0.1.0
git push origin 0.1.0
```

## 라이선스

MIT
