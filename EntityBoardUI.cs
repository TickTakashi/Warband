using UnityEngine;
using System.Collections;
using UEL;
using System.Collections.Generic;
using System;

public class EntityBoardUI : MonoBehaviour {

  private Dictionary<Entity, HealthBarUI> health_bars;
  public Transform health_bar_prefab;


  public void Start() {
    health_bars = new Dictionary<Entity, HealthBarUI>();
    CameraControl cc = Camera.main.transform.parent.GetComponent<CameraControl>();
    cc.CameraMovedEvent += new EventHandler<CameraMovedEventArgs>(OnCameraMoved);
    UpdateHealthBars();
  }

  // TODO Handler
  public void OnCameraMoved(object cc, CameraMovedEventArgs e) {
    UpdateHealthBars();
  }

  public void UpdateHealthBars() {
    foreach(Warrior e in Grid.board.warriors.Keys) {
      if (!health_bars.ContainsKey(e)) {
        Transform hbt = Instantiate(health_bar_prefab) as Transform;
        hbt.SetParent(transform, false);
        HealthBarUI hb = hbt.GetComponent<HealthBarUI>();
        hb.SetEntity(e);
        health_bars[e] = hb;
      }

      health_bars[e].UpdateBar();
    }
  }

}
