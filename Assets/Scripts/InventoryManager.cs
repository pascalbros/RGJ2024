using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager: MonoBehaviour {
    private PlayerController controller;

    public Transform topIcon;
    public Transform bottomIcon;

    public Portable top;
    public Portable bottom;

    public bool HasKey { get { return (top?.IsKey ?? false) || (bottom?.IsKey ?? false); } }
    void Awake() {
        controller = GetComponent<PlayerController>();
    }

    public void SetTop(Portable value) {
        SetPortable(value, topIcon, ref top);
    }

    public void SetBottom(Portable value) {
        SetPortable(value, bottomIcon, ref bottom);
    }

    private void SetPortable(Portable portable, Transform placeholder, ref Portable old) {
        Debug.Log("old", old);
        Debug.Log("new", portable);
        if (old != null)
            old.gameObject.SetActive(false);
        if (portable != null) {
            if (portable.IsKey) {
                Drop(old);
            } else {
                portable.bigIcon.SetActive(false);
                portable.transform.parent = placeholder;
                portable.transform.localPosition = Vector3.zero;
                portable.smallIcon.SetActive(true);
                portable.gameObject.SetActive(true);
            }
        }
        old = portable;
    }

    public void Drop(bool isTop) {
        Debug.Log("dropping " + isTop);
        if (isTop && top != null)
            Drop(ref top);
        else if (!isTop && bottom != null)
            Drop(ref bottom);
    }

    private void Drop(ref Portable value) {
        Debug.Log("drop", value);
        value.smallIcon.SetActive(false);
        value.transform.parent = null;
        value.transform.position = transform.position;
        value.bigIcon.SetActive(true);
        value.gameObject.SetActive(true);
        value = null;
    }

}
