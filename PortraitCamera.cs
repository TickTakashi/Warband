using UnityEngine;
using System.Collections;
using UEL;
using System;

public class PortraitCamera : MonoBehaviour {

  void Start() {
    Grid.selector.CharacterSelectedEvent += new EventHandler<CharacterSelectedEventArgs>(OnCharacterSelected);
  }

  public void OnCharacterSelected(object o, CharacterSelectedEventArgs e) {
    UpdatePortrait();
  }

  public void UpdatePortrait() {
    Warrior selected = Grid.selector.selected;
    transform.SetParent(selected.portrait_cam);
    transform.localPosition = Vector3.zero;
    transform.localRotation = Quaternion.identity;
  }
}