using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsageManager : MonoBehaviour
{
    public NumberSprite sprites;
    public Portable portable;
    public SpriteRenderer numRenderer;

    void Awake() {
        if (portable == null) 
            portable = GetComponent<Portable>();
    }

    void OnEnable() {
        UpdateCounter();
    }

    public void SetPortable(Portable portable) {
        if (this.portable && this.portable.usageManager == this) 
            this.portable.usageManager = null;

        this.portable = portable;
        if (portable == null)
            numRenderer.gameObject.SetActive(false);
        else {
            portable.usageManager = this;
            UpdateCounter();
        }
    }

    public void UpdateCounter() {
        if (portable == null) return;
        if (portable.IsConsumable) {
            numRenderer.gameObject.SetActive(true);
            numRenderer.sprite = sprites[portable.usages];
        } else {
            numRenderer.gameObject.SetActive(false);
        }
    }
}
