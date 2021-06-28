using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ConnectFour.Editor
{
    [CustomEditor(typeof(AspectRatioFitterLayoutElement), true)]
    [CanEditMultipleObjects]
    public sealed class AspectRatioFitterLayoutElementEditor : LayoutElementEditor
    {
        private SerializedProperty _aspectMode;
        private SerializedProperty _aspectRatio;
        private SerializedProperty _ignoreLayout;
        private SerializedProperty _minWidth;
        private SerializedProperty _minHeight;
        private SerializedProperty _preferredWidth;
        private SerializedProperty _preferredHeight;
        private SerializedProperty _flexibleWidth;
        private SerializedProperty _flexibleHeight;
        private SerializedProperty _layoutPriority;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_aspectMode);
            EditorGUILayout.PropertyField(_aspectRatio);
            EditorGUILayout.PropertyField(_ignoreLayout);

            if (!_ignoreLayout.boolValue)
            {
                EditorGUILayout.Space();

                bool hideWidthControls = _aspectMode.intValue == (int)AspectRatioFitterLayoutElement.AspectMode.HeightControlsWidth;
                bool hideHeightControls = _aspectMode.intValue == (int)AspectRatioFitterLayoutElement.AspectMode.WidthControlsHeight;
                LayoutElementField(_minWidth, 0, hideWidthControls);
                LayoutElementField(_minHeight, 0, hideHeightControls);
                LayoutElementField(_preferredWidth, t => t.rect.width, hideWidthControls);
                LayoutElementField(_preferredHeight, t => t.rect.height, hideHeightControls);
                LayoutElementField(_flexibleWidth, 1, hideWidthControls);
                LayoutElementField(_flexibleHeight, 1, hideHeightControls);
            }

            EditorGUILayout.PropertyField(_layoutPriority);

            serializedObject.ApplyModifiedProperties();
        }

        protected override void OnEnable()
        {
            _aspectMode = serializedObject.FindProperty("_aspectMode");
            _aspectRatio = serializedObject.FindProperty("_aspectRatio");
            _ignoreLayout = serializedObject.FindProperty("m_IgnoreLayout");
            _minWidth = serializedObject.FindProperty("m_MinWidth");
            _minHeight = serializedObject.FindProperty("m_MinHeight");
            _preferredWidth = serializedObject.FindProperty("m_PreferredWidth");
            _preferredHeight = serializedObject.FindProperty("m_PreferredHeight");
            _flexibleWidth = serializedObject.FindProperty("m_FlexibleWidth");
            _flexibleHeight = serializedObject.FindProperty("m_FlexibleHeight");
            _layoutPriority = serializedObject.FindProperty("m_LayoutPriority");
        }

        private void LayoutElementField(SerializedProperty property, float defaultValue, bool hide)
        {
            LayoutElementField(property, _ => defaultValue, hide);
        }

        private void LayoutElementField(SerializedProperty property, System.Func<RectTransform, float> defaultValue, bool hide)
        {
            if (hide)
            {
                if (property.floatValue >= 0)
                {
                    property.floatValue = -1;
                }

                return;
            }

            Rect position = EditorGUILayout.GetControlRect();
            GUIContent label = EditorGUI.BeginProperty(position, null, property);
            Rect fieldPosition = EditorGUI.PrefixLabel(position, label);
            Rect toggleRect = fieldPosition;
            toggleRect.width = 16;

            Rect floatFieldRect = fieldPosition;
            floatFieldRect.xMin += 16;

            EditorGUI.BeginChangeCheck();

            bool enabled = EditorGUI.ToggleLeft(toggleRect, GUIContent.none, property.floatValue >= 0);

            if (EditorGUI.EndChangeCheck())
            {
                property.floatValue = enabled ? defaultValue((target as LayoutElement)!.transform as RectTransform) : -1;
            }

            if (!property.hasMultipleDifferentValues && property.floatValue >= 0)
            {
                EditorGUIUtility.labelWidth = 4;
                EditorGUI.BeginChangeCheck();

                float newValue = EditorGUI.FloatField(floatFieldRect, new GUIContent(" "), property.floatValue);

                if (EditorGUI.EndChangeCheck())
                {
                    property.floatValue = Mathf.Max(0, newValue);
                }

                EditorGUIUtility.labelWidth = 0;
            }

            EditorGUI.EndProperty();
        }
    }
}
