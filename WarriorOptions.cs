using UnityEngine;
using System.Collections;

public class WarriorOptions : UIElement {

  public void Start() {
    Visible(false);
  }

  public override void Visible(bool is_visible) {
    gameObject.SetActive(is_visible);
    base.Visible(is_visible);
  }
}
