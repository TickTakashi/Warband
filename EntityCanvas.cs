using UnityEngine;
using System.Collections;
using System;
using UEL;

public class EntityCanvas : UELBehaviour {
  public Entity entity;
  public UIElement menu;
  public HealthBarUI health;

  public void Start() {
    entity.PositionChangeEvent += new EventHandler<PositionChangeEventArgs>(OnPositionChange);
    OnPositionChange(null, new PositionChangeEventArgs());
  }

  public void OnPositionChange(object caller, PositionChangeEventArgs e) {
    if (e.moving) {
      menu.Visible(false);
    }
    
    FaceCamera();
  }

  public void Visible(bool is_visible) {
    menu.Visible(is_visible);
  }

  public void FaceCamera() {
    Transform cam_trans = Camera.main.transform;
    transform.rotation = Quaternion.Euler(cam_trans.rotation.eulerAngles.x, 225f, 0);
  }
}
