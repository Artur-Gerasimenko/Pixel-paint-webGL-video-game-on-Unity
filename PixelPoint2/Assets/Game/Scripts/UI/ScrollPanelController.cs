using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
[RequireComponent(typeof(ScrollRect))]
public class ScrollPanelController : MonoBehaviour
{
    [SerializeField] private UnityEvent Enable, Disable;
    private void OnEnable() => PointAction.onTouchDownMob += BlockScrolling;
  
    private void OnDisable() => PointAction.onTouchDownMob -= BlockScrolling;

    private void BlockScrolling()
    {
        if (PointAction.isMove)
        {
            PointAction.isMove = false;
        }
    }
}
