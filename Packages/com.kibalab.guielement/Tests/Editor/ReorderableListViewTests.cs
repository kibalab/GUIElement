using System.Reflection;
using KIBA_.GUIElement.Reorder.Editor;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace KIBA_.GUIElement.Tests.Editor
{
    public sealed class ReorderableListViewTests
    {
        private static object Call(ReorderableListView view, string method, params object[] args) =>
            typeof(ReorderableListView).GetMethod(method, BindingFlags.Instance | BindingFlags.NonPublic).Invoke(view, args);

        [Test]
        public void EmptyList_DoesNotDrawAnInsertionMarker()
        {
            var view = new ReorderableListView("GUIElement.Test.Empty");
            Assert.DoesNotThrow(() => Call(view, "DrawInsertionMarker", 0));
        }

        [Test]
        public void DropBounds_RejectHorizontalAndVerticalMisses()
        {
            var view = new ReorderableListView("GUIElement.Test.Bounds");
            view.AddItemRect(new Rect(10, 20, 100, 20));
            view.AddItemRect(new Rect(10, 50, 100, 20));
            Assert.That(Call(view, "ContainsDropPosition", new Vector2(30, 45)), Is.True);
            Assert.That(Call(view, "ContainsDropPosition", new Vector2(200, 30)), Is.False);
            Assert.That(Call(view, "ContainsDropPosition", new Vector2(30, 100)), Is.False);
            view.ClearItemRects();
            Assert.That(Call(view, "ContainsDropPosition", new Vector2(30, 30)), Is.False);
        }

        [TestCase(5f, 0)]
        [TestCase(35f, 1)]
        [TestCase(80f, 2)]
        public void InsertionIndex_UsesRowMidpoints(float mouseY, int expected)
        {
            var view = new ReorderableListView("GUIElement.Test.Insert");
            view.AddItemRect(new Rect(10, 20, 100, 20));
            view.AddItemRect(new Rect(10, 50, 100, 20));
            Assert.That(Call(view, "ComputeInsertIndex", mouseY), Is.EqualTo(expected));
        }
    }
}
