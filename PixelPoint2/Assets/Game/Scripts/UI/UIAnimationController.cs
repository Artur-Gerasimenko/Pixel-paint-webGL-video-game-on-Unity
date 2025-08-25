using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationController : MonoBehaviour
{
    
    [Space,SerializeField] private AnimationClip MainAnimation;

    private float time { get; set; } = 0f;
    private Animator MyAnimator { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        MyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ToGoNextAnimationAfterPlay()
    {
        MyAnimator.SetTrigger(MainAnimation.name);

        Invoke("EndMethod", MainAnimation.length);
        //while (time < MainAnimation.length)
        //{
        //    time += Time.deltaTime;
        //}
    }
    private void EndMethod()
    {
        time = default;
        MyAnimator.SetTrigger("Normal");
    }
}
