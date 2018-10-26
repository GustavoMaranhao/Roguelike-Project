using UnityEngine;

public static class GUIUtils {
    static Texture2D _whiteTexture;
    public static Texture2D WhiteTexture
    {
        get
        {
            if(_whiteTexture == null)
            {
                _whiteTexture = new Texture2D(1, 1);
                _whiteTexture.SetPixel(0, 0, Color.white);
                _whiteTexture.Apply();
            }
            return _whiteTexture;
        }
    }

    public static void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, WhiteTexture);
        GUI.color = Color.white;
    }

    public static void DrawScreenRectBorder (Rect rect, float thickness, Color color)
    {
        //Top
        GUIUtils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        //Left
        GUIUtils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        //Right
        GUIUtils.DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        //Bottom
        GUIUtils.DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    public static Bounds GetViewportBoundsFromWorld(Camera camera, Vector3 screenPosition1, Vector3 screenPosition2)
    {
        var v1 = camera.ScreenToViewportPoint(screenPosition1);
        var v2 = camera.ScreenToViewportPoint(screenPosition2);

        var min = Vector3.Min(v1, v2);
        var max = Vector3.Max(v1, v2);
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        var bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }

    public static Bounds GetViewportBoundsFromViewport(Camera camera, Vector2 viewportPosition1, Vector2 viewportPosition2)
    {
        var min = Vector3.Min(viewportPosition1, viewportPosition2);
        var max = Vector3.Max(viewportPosition1, viewportPosition2);
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        var bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }

    public static bool IsWithinViewport(Camera camera, GameObject gameObject)
    {
        var viewportBounds = GUIUtils.GetViewportBoundsFromViewport(camera, new Vector2(0,0), new Vector2(1,1));
        return viewportBounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
    }
}
