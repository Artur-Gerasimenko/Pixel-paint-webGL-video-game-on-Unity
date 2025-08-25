using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : PointAction
{
    private bool isMobilePlatform { get { return Application.isMobilePlatform; } }
    // Start is called before the first frame update
    void Start()
    {
        Initialization(); 
        StartCoroutine(CoroutineUpdate());
    }

    private void OnEnable()
    {
        onMouseButtonDown += IsMovingOnPointerUIOverWin;
        onTouchDownMob += IsMovingOnPointerUIOverMob;
        onInputUp += GiveDefaultValue;
    }
    private void OnDisable() 
    {
        onMouseButtonDown -= IsMovingOnPointerUIOverWin;
        onTouchDownMob -= IsMovingOnPointerUIOverMob;
        onInputUp -= GiveDefaultValue;
    }
    private IEnumerator CoroutineUpdate()
    {
        while (true)
        {
            ClickAction();
            switch (isMobilePlatform)
            {
                case false:
                    if (Input.GetMouseButtonDown(0)) onMouseButtonDown?.Invoke();
                    if (Input.GetMouseButtonUp(0)) onInputUp?.Invoke();
                    MouseMovement();
                    WheelScrolling();
                    break;
                case true:
                    if (Input.touchCount > 0)
                    {
                        if (Input.GetTouch(0).phase == TouchPhase.Began) { onTouchDownMob?.Invoke(); }
                        if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled) { onInputUp?.Invoke(); }
                    }
                    SwipingApproximation();
                    break;
            }
            yield return null;
        }
    }
}
