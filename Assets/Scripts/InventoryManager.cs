using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
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
        if (old != null) {
            if (old.IsKey)
                Recover(old);
            else
                Discard(old);
        }
        if (portable != null) {
            portable.bigIcon.SetActive(false);
            portable.transform.parent = placeholder;
            portable.transform.localPosition = Vector3.zero;
            var zRotation = Mathf.Abs(transform.eulerAngles.z % 360);
            bool horiz = Mathf.Approximately(zRotation, 0f) || Mathf.Approximately(zRotation, 180f);
            portable.smallIcon.SetActive(horiz);
            portable.smallVertIcon.SetActive(!horiz);
            portable.gameObject.SetActive(true);
        }
        old = portable;
    }

    public void Drop(bool isTop) {
        if (isTop && top != null) {
            Recover(top);
            top = null;
        }
        else if (!isTop && bottom != null) {
            Recover(bottom);
            bottom = null;
        }
    }

    public void Discard(Portable value) {
        value.gameObject.SetActive(false);
    }

    public void Recover(Portable value) {
        value.smallIcon.SetActive(false);
        value.smallVertIcon.SetActive(false);
        value.transform.parent = null;
        value.transform.position = transform.position;
        value.bigIcon.SetActive(true);
        value.gameObject.SetActive(true);
    }

}
