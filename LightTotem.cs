using UnityEngine;
using System.Collections;
using UEL;

public class LightTotem : Totem {

  public int range = 4;
  public Light spot;
  private float cookie_modifier = 1f;
  void Start() {
    // We know the adjacent and the opposite. Find theta.
    base.Start();
    float height = spot.transform.position.y;
    float target_length = (range + 0.5f) * 2f * cookie_modifier;
    float spot_angle = Mathf.Atan(target_length / height) * (180f / Mathf.PI);
    spot.spotAngle = spot_angle * 2f;
  }
}
