using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class UnregisterGraphicFromRaycasting : MonoBehaviour
{
    void Start()
    {
        Canvas canvas = FindInParents<Canvas>(gameObject); //searching in parents because there can be multiple canvases

        GraphicRegistry.UnregisterGraphicForCanvas(canvas, GetComponent<Graphic>());
    }

    static public T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;
        T comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        Transform t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }
}
