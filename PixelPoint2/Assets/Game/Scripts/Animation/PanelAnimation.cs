using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[ExecuteInEditMode]
public class PanelAnimation : MonoBehaviour
{
    [SerializeField] private AnimationCurve _moveOpenAnimation;
    [SerializeField] private AnimationCurve _moveCloseAnimation;
    [SerializeField] private Canvas _canvas;

    private Vector3 _closePosition;
    private Vector3 _openPosition;

    private float _reatTime,_timeLength = 0;

    float value;

    private bool isClose = true;
    private bool isEnded = true;
    private void Start()
    {
        _closePosition = transform.position;
        _openPosition = new Vector3(_canvas.transform.position.x, _canvas.transform.position.y,transform.position.z);

        _moveOpenAnimation.keys = ChangedKeys(_moveOpenAnimation.keys, _closePosition.y, _openPosition.y);
        _moveCloseAnimation.keys = ChangedKeys(_moveCloseAnimation.keys, _openPosition.y, _closePosition.y);
    }
    private Keyframe[] ChangedKeys(Keyframe[] keys,float FirstIdexValue,float LastIndexValue)
    {
        Keyframe[] newKeys = new Keyframe[keys.Length];

        for (int index = 0; index < keys.Length; index++)
        {
            newKeys[index] = keys[index];
        }

        newKeys[0].value = FirstIdexValue;

        if(FirstIdexValue > LastIndexValue)newKeys[newKeys.Length - 2].value = LastIndexValue - 200f;
        else newKeys[newKeys.Length - 2].value = FirstIdexValue - 200f;

        newKeys[newKeys.Length - 1].value = LastIndexValue;

        return newKeys;
    }
    public void OpenPanelAnimation()
    {
        if (isClose && isEnded)
        {
            isEnded = false;
            isClose = false;
            _timeLength = _moveOpenAnimation.keys[_moveOpenAnimation.keys.Length - 1].time;
            StartCoroutine(AppearedPanel(_moveOpenAnimation));
        }
    }
    public void ClosePanelAnimation()
    {
        if (!isClose && isEnded)
        {
            isEnded = false;
            isClose = true;
            _timeLength = _moveCloseAnimation.keys[_moveCloseAnimation.keys.Length - 1].time;

            StartCoroutine(AppearedPanel(_moveCloseAnimation));
        }
    }
    private IEnumerator AppearedPanel(AnimationCurve animation)
    {
        while (_reatTime <= _timeLength)
        {
            _reatTime += Time.deltaTime;

            value = animation.Evaluate(_reatTime);
            transform.position = new Vector3(transform.position.x,value, transform.position.z);

            yield return null;
        }
        isEnded = true;
        _reatTime = 0;
    }
}
