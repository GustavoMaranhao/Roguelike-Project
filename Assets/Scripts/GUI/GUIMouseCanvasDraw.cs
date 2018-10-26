using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GUIMouseCanvasDraw : MonoBehaviour
{

    [System.Serializable]
    public class MouseOverlayColor
    {
        public Texture2D idleCursor;
        public Texture2D allyCursor;
        public Texture2D enemyCursor;
    }
    public MouseOverlayColor mouseCursorTexture;

    [System.Serializable]
    public class DragSelectionBox
    {
        public Color innerBoxColor;
        public Color outterBoxColor;
    }
    public DragSelectionBox dragSelectionBox;

    private GUIMouseCursorController cursorController;
    private Vector2 mouseAnchor = Vector2.zero;
    private CursorMode mouseCursorMode = CursorMode.Auto;

	void Start () {
        cursorController = GameObject.FindWithTag(Tags.gameController).GetComponent<GUIMouseCursorController>();
    }

    void OnGUI () {
        #region Cursor Texture Change
        if (cursorController.ObjectUnderMouse == null)
        {
            Cursor.SetCursor(mouseCursorTexture.idleCursor, mouseAnchor, mouseCursorMode);
        }
        else
        {
            switch (cursorController.ObjectUnderMouse.tag)
            {
                case GlobalConstants.PlayerTag:
                    Cursor.SetCursor(mouseCursorTexture.allyCursor, mouseAnchor, mouseCursorMode);
                    break;
                case GlobalConstants.EnemyTag:
                    Cursor.SetCursor(mouseCursorTexture.enemyCursor, mouseAnchor, mouseCursorMode);
                    break;
                default:
                    Cursor.SetCursor(mouseCursorTexture.idleCursor, mouseAnchor, mouseCursorMode);
                    break;
            }
        }
        #endregion

        #region GUI onDrag
        if (cursorController.isMouseDragging)
        {
            var dragStart = cursorController.mouseDragStart;
            var dragEnd = cursorController.mousePosition;

            //Adjusting the inconsistency of transforms and screen
            dragStart.y = Screen.height - dragStart.y;
            dragEnd.y = Screen.height - dragEnd.y;

            //Calculate coreners
            var topLeft = Vector3.Min(dragStart, dragEnd);
            var bottomRight = Vector3.Max(dragStart, dragEnd);

            //Generate the rect size
            var rect = Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);

            GUIUtils.DrawScreenRect(rect, dragSelectionBox.innerBoxColor);
            GUIUtils.DrawScreenRectBorder(rect, 2, dragSelectionBox.outterBoxColor);
        }
        #endregion
    }
}
