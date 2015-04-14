using UnityEngine;
using System.Collections;
using System;

public class ScaleWithZoom : MonoBehaviour {

  public float min_scale = 0.5f;
  public float max_scale = 3.33f;
  
  private RectTransform rt;

  public void Start() {
    rt = GetComponent<RectTransform>();
    Grid.camera.CameraMovedEvent += new EventHandler<CameraMovedEventArgs>(OnCameraMoved);
    UIElement uie = GetComponent<UIElement>();
    uie.VisibilityEvent += new EventHandler<VisibilityEventArgs>(OnVisibility); 
  }

  public void OnVisibility(object caller, VisibilityEventArgs e) {
    AdjustScale();
  }

  public void OnCameraMoved(object caller, CameraMovedEventArgs e) {
    AdjustScale();
	}

  public void AdjustScale() {
    rt.localScale = Vector3.Lerp(Vector3.one * min_scale,
      Vector3.one * max_scale,
      (1f - Grid.camera.zoom_level));
  }
}
