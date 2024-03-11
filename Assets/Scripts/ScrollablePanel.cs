using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollablePanel : MonoBehaviour
{


    [Header("Scrollable")]
    [SerializeField] private RectTransform container;
    [SerializeField] private RectTransform offsetTemplate;
    private Vector2 originPos;

    [Header("Scroll Settings")]
    [SerializeField] private int totalPages = 0;
    [SerializeField] private int currentPage = 0;

    [Header("Tween Data")]
    [SerializeField] private float TweenTime = 0.1f;
    [SerializeField] private LeanTweenType TweenType = LeanTweenType.linear;



    public Action OnPageSwitch { get; private set; }
    

    protected void Awake()
    {
        originPos = container.anchoredPosition;
        OnPageSwitch += setPosition;
    }

    public void scrollNext() { 
        setPage(currentPage + 1);
    }
    public void scrollPrev() {
        setPage(currentPage - 1);
    }

    public void setPage(int page)
    {
        currentPage = Math.Clamp(page, 0, totalPages - 1);
        OnPageSwitch?.Invoke();
    }


    private void setPosition()
    {
        float newPos = originPos.x - offsetTemplate.rect.width * currentPage;
        LeanTween.moveLocalX(container.gameObject, newPos, 0.1f).setEase(TweenType);
    }

}
