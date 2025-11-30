using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SceneField))]
public class SceneFieldPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Usamos try-catch para "absorber" el error si el objeto ha sido destruido.
        try
        {
            // Verificación previa básica
            if (property == null || property.serializedObject == null) return;

            // INTENTO DE ACCESO SEGURO:
            // Si esta línea falla, el 'catch' de abajo capturará el error silenciosamente.
            if (property.serializedObject.targetObject == null) return;

            EditorGUI.BeginProperty(position, GUIContent.none, property);

            SerializedProperty sceneAsset = property.FindPropertyRelative("_sceneAsset");
            SerializedProperty sceneName = property.FindPropertyRelative("_sceneName");

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            if (sceneAsset != null)
            {
                EditorGUI.BeginChangeCheck();

                // Dibujamos el campo de objeto
                Object value = EditorGUI.ObjectField(position, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);

                if (EditorGUI.EndChangeCheck())
                {
                    sceneAsset.objectReferenceValue = value;
                    if (sceneAsset.objectReferenceValue != null)
                    {
                        sceneName.stringValue = (sceneAsset.objectReferenceValue as SceneAsset).name;
                    }
                }
            }

            EditorGUI.EndProperty();
        }
        catch (System.Exception)
        {
            // Si ocurre CUALQUIER error (como que el objeto fue destruido),
            // no hacemos nada (return). Esto elimina el mensaje rojo de la consola.
            return;
        }
    }
}
#endif