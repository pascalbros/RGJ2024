using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SelectionPopup: MonoBehaviour {
    public static SelectionPopup Instance { get; private set; }
    [SerializeField] float animDuration;

    [SerializeField] Image topPortable;
    [SerializeField] Image bottomPortable;
    [SerializeField] Image candidatePortable;

    public Image background;
    public float backgroundOpacity = 0.5f;

    private void Awake() {
        if (Instance == null) { Instance = this; }
    }

    [ContextMenu("Show")]
    void Show() {
        Show(null, null, null, Vector3.zero);
    }
    public void Show(Sprite topPortableSprite, Sprite bottomPortableSprite, Sprite candidatePortableSprite, Vector3 rotation) {
        topPortable.sprite = topPortableSprite;
        topPortable.transform.localEulerAngles = rotation;
        bottomPortable.sprite = bottomPortableSprite;
        bottomPortable.transform.localEulerAngles = rotation;
        candidatePortable.sprite = candidatePortableSprite;

        transform.DOLocalMove(new Vector3(0, 0, 0), animDuration).SetEase(Ease.InOutSine);
        background.DOFade(backgroundOpacity, animDuration);
    }
    [ContextMenu("Hide")]
    public void Hide() {
        transform.DOLocalMove(new Vector3(0, -1080, 0), animDuration).SetEase(Ease.InOutSine);
        background.DOFade(0f, animDuration);
    }
}
