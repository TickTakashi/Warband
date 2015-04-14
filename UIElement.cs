using UnityEngine;
using System.Collections;
using UEL;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UIElement : UELBehaviour {
  public event EventHandler<VisibilityEventArgs> VisibilityEvent;

  public CanvasGroup canvas_group;
  private RectTransform _rtrans;
  public RectTransform rtrans {
    get {
      if (_rtrans == null) { _rtrans = GetComponent<RectTransform>(); }
      return _rtrans;
    }
  }

  public List<UIElement> children;

  public virtual void Visible(bool is_visible) {
    if (VisibilityEvent != null)
      VisibilityEvent(this, new VisibilityEventArgs(is_visible));
    canvas_group.alpha = is_visible ? 1f: 0f;
    canvas_group.blocksRaycasts = is_visible;
    canvas_group.interactable = is_visible;
    ChildrenVisible(is_visible);
  }

  public virtual void ChildrenVisible(bool is_visible) {
    foreach (UIElement uie in children) {
      uie.Visible(is_visible);
    }
  }

  public void FaceCamera() {
    Transform cam_trans = Camera.main.transform;
    transform.rotation = Quaternion.Euler(cam_trans.rotation.eulerAngles.x, 225f, 0);
  }
}

public class VisibilityEventArgs : EventArgs {
  public bool visibility;

  public VisibilityEventArgs(bool visibility) {
    this.visibility = visibility;
  }
}

