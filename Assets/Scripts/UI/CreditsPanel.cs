using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsPanel : MonoBehaviour
{
    [SerializeField] float animationDuration = 0.5f;
    [SerializeField] Ease openEase;
    [SerializeField] Ease closeEase;

    public void Open()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.DOLocalMove(new Vector3(0, 0, 0), animationDuration).SetEase(openEase);
    }

    public void Close()
    {
        GetComponent<RectTransform>().DOLocalMove(new Vector3(0, -1080, 0), animationDuration).SetEase(closeEase);
    }
}
