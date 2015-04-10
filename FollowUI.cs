using UnityEngine;
using System.Collections;
using UEL;
using System;

public abstract class FollowUI<E> : UELBehaviour where E : MonoBehaviour {

  public CanvasGroup group;
  public Vector2 world_space_offset = new Vector2(0.5f, 0.5f);
  public Vector2 screen_space_offset = Vector2.zero;
  public RectTransform rtrans;

  public E e;

  public bool is_on { get; set; }
  public bool entity_moving { get; set; }
  public bool camera_moving { get; set; }
  public bool is_visible { get { return is_on && !entity_moving &&
    !camera_moving; } }

  public virtual void SetTracked(E e) {
    this.e = e;
    CameraControl cc = Camera.main.transform.parent.GetComponent<CameraControl>();
    cc.CameraMovedEvent += new EventHandler<CameraMovedEventArgs>(OnCameraMoved);
    MaintainPosition();
  }

  private void OnCameraMoved(object sender, CameraMovedEventArgs ea) {
    camera_moving = ea.moving;
    MaintainPosition();
  }

  public void MaintainPosition() {
    Draw(is_visible);
    if (is_visible) {
      Vector2 viewpoint_point = UILocation(world_space_offset);
      RectTransform rt = GetComponent<RectTransform>();
      rt.anchorMin = viewpoint_point;
      rt.anchorMax = viewpoint_point;
      rt.anchoredPosition = screen_space_offset;
    }
  }

  public Vector2 UILocation() {
    return UILocation(new Vector2(0f, 0f));
  }

  public Vector2 UILocation(Vector2 world_space_offset) {
    Vector3 position = e.transform.position + new Vector3(
      Grid.board.tile_size * world_space_offset.x / 2f,
      0,
      Grid.board.tile_size * world_space_offset.y / 2f);
    return Camera.main.WorldToViewportPoint(position);
  }

  public void Draw(bool v) {
    group.alpha = v ? 1f : 0f;
    group.interactable = v;
    group.blocksRaycasts = v;
  }

  public virtual void SetVisibility(bool is_on) {
    this.is_on = is_on;
    MaintainPosition();
  }
}


