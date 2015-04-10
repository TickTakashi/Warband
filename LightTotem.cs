using UnityEngine;
using System.Collections;
using UEL;

public class LightTotem : Totem {
  public int range = 4;
  public Light spot_hq;

  public void Start() {
    Transform source = spot_hq.transform; 
    float height = source.position.y;
    float target_length = (range + 0.5f) * Grid.board.tile_size;
    float spot_angle = Mathf.Atan(target_length / height) * (180f / Mathf.PI);
    spot_hq.spotAngle = spot_angle * 2f;

    spot_hq.color = GameManager.player_colors[owner].SetSaturation(0.4f);
  }
}
