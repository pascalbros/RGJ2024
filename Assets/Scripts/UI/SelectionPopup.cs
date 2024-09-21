using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SelectionPopup : MonoBehaviour
{
    [SerializeField] float animDuration;

    [SerializeField] Image topPortable;
    [SerializeField] Image bottomPortable;
    [SerializeField] Image[] candidatePortable;

    [ContextMenu("Show")]
    void Show()
    {
        Show(null, null, null, Vector3.zero);
    }
    public void Show(Sprite topPortableSprite, Sprite bottomPortableSprite, Sprite candidatePortableSprite, Vector3 rotation)
    {
        topPortable.sprite = topPortableSprite;
        topPortable.transform.localEulerAngles = rotation;
        bottomPortable.sprite = bottomPortableSprite;
        bottomPortable.transform.localEulerAngles = rotation;
        for (int i = 0; i < candidatePortable.Length; i++)
        {
            candidatePortable[i].sprite = candidatePortableSprite;
        }

        transform.DOLocalMove(new Vector3(0, 0, 0), animDuration).SetEase(Ease.InOutSine);
    }
    [ContextMenu("Hide")]
    public void Hide()
    {
        transform.DOLocalMove(new Vector3(0, -1080, 0), animDuration).SetEase(Ease.InOutSine);
    }
}
