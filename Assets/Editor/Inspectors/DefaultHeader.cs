using UnityEngine;

namespace UnityEditor
{
    public static partial class EditorGUILayoutHelper
    {
        private const float ImageSectionWidth = 44;
        private const float PadingTop = 2;
        private const float PaddingLeft = 35;
        private const float PaddingRight = 60;
        private const float Height = 20;

        private const float LabelYOffset = 4;
        private const float LabelXOffset = 44;

        public static string DrawHeaderTextField(string text)
        {
            Rect overlayRect = new Rect()
            {
                x = PaddingLeft,
                y = PadingTop,
                width = Screen.width - (PaddingLeft + PaddingRight),
                height = Height,
            };

            if (Event.current.type == EventType.Repaint)
                Styles.inspectorBigInner.Draw(overlayRect, GUIContent.none, 0);

            Rect labelRect = new Rect()
            {
                x = LabelXOffset,
                y = LabelYOffset,
                width = overlayRect.width,
                height = overlayRect.height,
            };

            return EditorGUI.TextField(labelRect, text, Styles.textFieldHeader);
        }

        static class Styles
        {
            public static readonly GUIContent open = EditorGUIUtility.TrTextContent("Open");
            public static readonly GUIStyle textFieldHeader = new GUIStyle("LargeLabel");
            public static readonly GUIStyle inspectorBig = new GUIStyle("In BigTitle");
            public static readonly GUIStyle inspectorBigInner = "IN BigTitle inner";
            public static readonly GUIStyle centerStyle = new GUIStyle();
            public static readonly GUIStyle postLargeHeaderBackground = "IN BigTitle Post";

            static Styles()
            {
                centerStyle.alignment = TextAnchor.MiddleCenter;
                // modify header bottom padding on a mutable copy here
                // this was done rather than modifying the style asset itself in order to minimize possible side effects where the style was already used
                inspectorBig.padding.bottom -= 1;

                textFieldHeader.active = EditorStyles.textField.active;
            }
        }
    }
}