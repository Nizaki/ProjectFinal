using UnityEngine;
using System.Collections;

public class FollowClick : MonoBehaviour
{
    public AnimationCurve LeftClick;
    public AnimationCurve RightClick;

    public EasyTween TweenToControl;
    public Transform RootCanvas;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            MoveToMouseClick(LeftClick);
        else if (Input.GetMouseButtonDown(1)) MoveToMouseClick(RightClick);
    }

    private void MoveToMouseClick(AnimationCurve animationCurve)
    {
        var mouseAnchor = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        mouseAnchor = new Vector3(mouseAnchor.x * Screen.width / RootCanvas.localScale.x,
            mouseAnchor.y * Screen.height / RootCanvas.localScale.y, 0f);

        if (!TweenToControl.IsObjectOpened())
            TweenToControl.SetAnimationPosition(TweenToControl.rectTransform.anchoredPosition, (Vector2) mouseAnchor,
                animationCurve, animationCurve);
        else
            TweenToControl.SetAnimationPosition((Vector2) mouseAnchor, TweenToControl.rectTransform.anchoredPosition,
                animationCurve, animationCurve);

        TweenToControl.OpenCloseObjectAnimation();
    }
}