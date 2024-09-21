using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class AnimatedButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] UnityEvent onClick;

    [Header("Animation Settings")]
    [SerializeField] float clickAnimDuration = 0.3f;
    [SerializeField] float clickAnimScale = 0.8f;

    [SerializeField] float hoverAnimDuration = 0.2f;
    [SerializeField] float hoverAnimScale = 1.1f;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        transform.DOScale(clickAnimScale, clickAnimDuration / 2).SetEase(Ease.InSine);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
        transform.DOScale(hoverAnimScale, hoverAnimDuration / 2);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit");
        transform.DOScale(1f, hoverAnimDuration / 2);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
        transform.DOScale(1f, clickAnimScale / 2).SetEase(Ease.OutSine).OnComplete(() => onClick.Invoke());
    }
}
