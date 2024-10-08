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

    public void Show(SpriteRenderer topPortableSprite, SpriteRenderer bottomPortableSprite, SpriteRenderer candidatePortableSprite) {
        topPortable.sprite = topPortableSprite.sprite;
        topPortable.transform.rotation = topPortableSprite.transform.rotation;
        topPortable.transform.localScale = new Vector3(topPortableSprite.flipX ? -1 : 1, topPortableSprite.flipY ? -1 : 1, 1);

        bottomPortable.sprite = bottomPortableSprite.sprite;
        bottomPortable.transform.rotation = bottomPortableSprite.transform.rotation;
        bottomPortable.transform.localScale = new Vector3(bottomPortableSprite.flipX ? -1 : 1, bottomPortableSprite.flipY ? -1 : 1, 1);

        candidatePortable.sprite = candidatePortableSprite.sprite;
        candidatePortable.transform.rotation = candidatePortableSprite.transform.rotation;
        candidatePortable.transform.localScale = new Vector3(candidatePortableSprite.flipX ? -1 : 1, candidatePortableSprite.flipY ? -1 : 1, 1);

        transform.DOLocalMove(new Vector3(0, 0, 0), animDuration).SetEase(Ease.InOutSine);
        background.DOFade(backgroundOpacity, animDuration);
    }
    [ContextMenu("Hide")]
    public void Hide() {
        transform.DOLocalMove(new Vector3(0, -1080, 0), animDuration).SetEase(Ease.InOutSine);
        background.DOFade(0f, animDuration);
    }
}
