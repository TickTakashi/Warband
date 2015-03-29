﻿using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	
  // TODO: Make CameraControl extend a control baseclass so that its easier 
  // to distable and enable control.
  public Transform cam;

  public float move_speed = 15f;
  public float threshold = 0.40f;
  public float zoom_level = 0;
  public float zoom_scale = 10f;
  public float zoom_speed = 3f;
  public float zoom_max = 1f;

  private Transform trans;
  private Vector3 zoom_zero;

  void Awake() {
    trans = transform;
    zoom_zero = cam.localPosition;
  }

	void Update () {
    // Handle translation  
    Vector2 m_pos = Input.mousePosition;
    Vector2 center = new Vector2(Screen.width / 2f, Screen.height / 2f);
    Vector2 dir = (-center + m_pos);
    Vector3 translation = trans.forward * dir.y + trans.right * dir.x;

    translation = translation.normalized;

    // TODO: Make the speed scaling more sophisticated.
    float speed_scale = dir.magnitude / Screen.height;
    speed_scale = speed_scale > threshold ? speed_scale : 0f;

    trans.Translate(translation * Time.deltaTime * move_speed * speed_scale,
      Space.World);

    // Handle Zoom
    zoom_level += Input.GetAxis("Mouse ScrollWheel");
    
    if (zoom_level < 0)
      zoom_level = 0;
    if (zoom_level > zoom_max)
      zoom_level = zoom_max;

    cam.position = Vector3.Lerp(cam.position, 
      transform.TransformPoint(zoom_zero) + cam.forward * zoom_level *
      zoom_scale, Time.deltaTime * zoom_speed);
  }
}