using UnityEngine;
using System.Collections;
using UEL;
using System;

public class CameraControl : UELBehaviour {
	
  // TODO: Make CameraControl extend a control baseclass so that its easier 
  // to distable and enable control.
  public Transform cam;

  public float move_speed = 15f;
  public float threshold = 0.40f;
  public float zoom_level = 0;
  public float zoom_scale = 10f;
  public float zoom_speed = 3f;
  public float zoom_max = 1f;
  public float min_speed_perc = 0.5f;

  private Transform trans;
  private Vector3 zoom_zero;


  public event EventHandler<CameraMovedEventArgs> CameraMovedEvent;

  void Awake() {
    trans = transform;
    zoom_zero = cam.localPosition;
  }

	void Update () {
    // Handle translation  
    Vector2 m_pos = Input.mousePosition;
    Vector2 center = new Vector2(Screen.width / 2f, Screen.height / 2f);
    Vector2 dir = (-center + m_pos);
    float speed_percentage = (1 - zoom_level / zoom_max) * 
      (1 - min_speed_perc) + min_speed_perc;
    Vector3 translation = trans.forward * dir.y + trans.right * dir.x;

    translation = translation.normalized;

    // TODO: Make the speed scaling more sophisticated.
    if (Mathf.Abs(dir.x) > Screen.width / 2f ||
      Mathf.Abs(dir.y) > Screen.height / 2f) {
      dir = Vector2.zero;
    }

    float speed_scale = dir.magnitude / Screen.height;
    speed_scale = speed_scale > threshold ? speed_scale : 0f;

    trans.Translate(translation * Time.deltaTime * move_speed * speed_scale *
      speed_percentage, Space.World);

    // Handle Zoom
    zoom_level += Input.GetAxis("Mouse ScrollWheel");
    
    if (zoom_level < 0)
      zoom_level = 0;
    if (zoom_level > zoom_max)
      zoom_level = zoom_max;

    Vector3 target_pos = transform.TransformPoint(zoom_zero) + cam.forward *
      zoom_level * zoom_scale;
    Vector3 delta = target_pos - cam.position;
    cam.position = Vector3.Lerp(cam.position, target_pos, Time.deltaTime * zoom_speed);

    if (speed_scale > threshold || delta.magnitude > Warrior.EPSILLON) {
      CameraMovedEventArgs cmea = new CameraMovedEventArgs();
      cmea.delta = delta;
      CameraMovedEvent(this, cmea);
    }
  }
}

public class CameraMovedEventArgs : EventArgs {
  public Vector3 delta;
}