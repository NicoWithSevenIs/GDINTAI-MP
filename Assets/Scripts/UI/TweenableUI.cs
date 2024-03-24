using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenableUI : MonoBehaviour
{




    [Header("Tween Goal")]
    [SerializeField] private LeanTweenType easingStyle;
    [SerializeField] private float duration;
    [SerializeField] private Vector2 goal;
    [SerializeField] private bool ignoreTimeScale;

    private RectTransform subject;
    private Vector2 originalPos;
    private bool inGoal = false;
    private void Awake()
    {
        originalPos = transform.localPosition;
        subject  = GetComponent<RectTransform>();
    }
    public void StartTween()
    {

        LeanTween.moveLocal(subject.gameObject, inGoal? originalPos: goal, duration)
                .setEase(easingStyle)
                .setIgnoreTimeScale(ignoreTimeScale)
                .setOnComplete(() => { inGoal = !inGoal; });
    }

   

    
    
}
