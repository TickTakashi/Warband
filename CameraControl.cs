using UnityEngine;
using System.Collections;
using UEL;
using System;

public class CameraControl : UELBehaviour {
	
  // TODO: Make CameraControl extend a control baseclass so that its easier 
  // to distable and enable control.
  public Transform cam;

  public float move_speed = 15f;
  public float threshold = 0.10f;
  public float zoom_level = 0;
  public float zoom_scale = 10f;
  public float zoom_speed = 3f;
  public float zoom_max = 1f;
  public float min_speed_perc = 0.5f;
  public bool can_control = true;
  private Transform trans;
  private Vector3 zoom_zero;

  private bool moving = false;

  public event EventHandler<CameraMovedEventArgs> CameraMovedEvent;

  void Awake() {
    trans = transform;
    zoom_zero = cam.localPosition;
    Vector3 target_pos = transform.TransformPoint(zoom_zero) + cam.forward *
        zoom_level * zoom_scale;
    cam.position = target_pos;
  }

	void Update () {
    if (can_control) {
      float vert_px_thresh = (Screen.height / 2f) - Screen.height * threshold;
      float hori_px_thresh = (Screen.width / 2f) - Screen.height * threshold;

      // Handle translation  
      Vector2 m_pos = Input.mousePosition;
      Vector2 center = new Vector2(Screen.width / 2f, Screen.height / 2f);
      Vector2 dir = (-center + m_pos);
      float speed_perc = (1 - zoom_level / zoom_max) *
        (1 - min_speed_perc) + min_speed_perc;


      // TODO: Make the speed scaling more sophisticated.
      float abs_x = Mathf.Abs(dir.x);
      float abs_y = Mathf.Abs(dir.y);
      if (!((abs_x < Screen.width / 2f && abs_x > hori_px_thresh) ||
        (abs_y < Screen.height / 2f && abs_y > vert_px_thresh))) {
        dir = Vector2.zero;
      }

      Vector3 translation = trans.forward * dir.y + trans.right * dir.x;
      translation = translation.normalized;

      Vector3 final = translation * Time.deltaTime * move_speed * speed_perc;
      if (Grid.board.GetTile(trans.position + final) == null) {
        Location old_loc = Grid.board.CalculateLocation(trans.position);
        Vector3 old_pos = Grid.board.CalculatePosition(old_loc);
        
        Location best = old_loc;
        float min = float.MaxValue;
        foreach (Location l in old_loc.Range(1, 1)) {
          Tile t = Grid.board.GetTile(l);
          if (t != null) {
            float dist = Vector3.Angle((-old_pos + t.transform.position).normalized,
              final);
            if (dist < min) {
              best = l;
              min = dist;
            }
          }
        }

        if (min < 90f) {
          Vector3 edge_dir = (-old_pos + Grid.board.CalculatePosition(best)).normalized;
          final = edge_dir * Time.deltaTime * move_speed * speed_perc;
        } else {
          final = Vector3.zero;
        }
      }

      trans.Translate(final, Space.World);
      

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

      bool in_motion = dir.magnitude > 0 || delta.magnitude > 0.1f;

      if (in_motion) {
        moving = true;
        CameraMovedEventArgs cmea = new CameraMovedEventArgs();
        cmea.moving = moving;
        if (CameraMovedEvent != null)
          CameraMovedEvent(this, cmea);
      }

      if (moving && !in_motion) {
        moving = false;
        CameraMovedEventArgs cmea = new CameraMovedEventArgs();
        cmea.moving = moving;
        if (CameraMovedEvent != null)
          CameraMovedEvent(this, cmea);
      }
    }
  }
}

public class CameraMovedEventArgs : EventArgs {
  public bool moving; 
}