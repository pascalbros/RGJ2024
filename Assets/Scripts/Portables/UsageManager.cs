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

    void UpdateCounter() {
        if (portable.IsConsumable) {
            numRenderer.sprite = sprites[portable.usages];
        } else {
            numRenderer.gameObject.SetActive(false);
        }
    }
}
