using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{

    [SerializeField] private RectTransform container;

    [Header("Tween Data")]
    [SerializeField] private float TweenTime = 0.1f;
    [SerializeField] private LeanTweenType TweenType = LeanTweenType.linear;


    public void LookAtRootScreen()
    {
        LeanTween.moveLocalX(container.gameObject, 960, 0.1f).setEase(TweenType);
    }

    public void LookAtSelectionScreen()
    {
        LeanTween.moveLocalX(container.gameObject, -960, 0.1f).setEase(TweenType);
        
    }

}
