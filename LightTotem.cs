using UnityEngine;
using System.Collections;
using UEL;

public class LightTotem : Totem {

  public int range = 4;
  public Light spot;

  void Start() {
    // We know the adjacent and the opposite. Find theta.
    float height = spot.transform.position.y;
    float spot_angle = Mathf.Atan(((float)range * 2 + 1) / height) * (180f / Mathf.PI);
    spot.spotAngle = spot_angle * 2f;
  }
}
