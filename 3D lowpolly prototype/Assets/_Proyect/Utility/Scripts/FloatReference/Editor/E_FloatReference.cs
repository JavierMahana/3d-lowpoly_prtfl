using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomPropertyDrawer(typeof(FloatReference))]
public class E_FloatReference : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        bool useConstant = property.FindPropertyRelative("UseConstant").boolValue;

        //draw label
        position = EditorGUI.PrefixLabel(position, label);

        Rect rect = new Rect(position.position, Vector2.one * 20);
        
        Color[] c = new Color[225];
        for (int i = 0; i < c.Length; i++)
        {
            c[i] = Color.white;
        }
        Texture2D dropDown = new Texture2D(15, 15);
        dropDown.SetPixels(c);


        if (EditorGUI.DropdownButton(rect, new GUIContent(dropDown), FocusType.Keyboard, 
            new GUIStyle() {fixedWidth = 50f, border = new RectOffset(1, 1, 1, 1)}))

        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Constant"), useConstant, () => SetProperty(property, true));

            menu.AddItem(new GUIContent("Variable"), !useConstant, () => SetProperty(property, false));
            menu.ShowAsContext();
        }

        position.position += Vector2.right * 17;
        position.width -= 17;
        float constValue = property.FindPropertyRelative("ConstantValue").floatValue;

        if (useConstant)
        {
            constValue = EditorGUI.DelayedFloatField(position, constValue);
            property.FindPropertyRelative("ConstantValue").floatValue = constValue;
        }
        else
        {
            EditorGUI.ObjectField(position, property.FindPropertyRelative("Variable"), GUIContent.none);
        }
        EditorGUI.EndProperty();
    }



    private void SetProperty(SerializedProperty property, bool value)
    {
        SerializedProperty constantProperty = property.FindPropertyRelative("UseConstant");
        constantProperty.boolValue = value;
        property.serializedObject.ApplyModifiedProperties();
    }
    private Texture GetTexture()
    {
        var textures = Resources.FindObjectsOfTypeAll(typeof(Texture))
            .Where(t => t.name.ToLower().Contains("animationdopesheetkeyframe"))
            .Cast<Texture>().ToList();
        return textures[0];
    }
}
