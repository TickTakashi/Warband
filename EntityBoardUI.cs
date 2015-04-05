using UnityEngine;
using System.Collections;
using UEL;
using System.Collections.Generic;

public class EntityBoardUI : MonoBehaviour, IObserver {

  private Dictionary<Entity, HealthBar> health_bars;
  public Transform health_bar_prefab;


  public void Start() {
    health_bars = new Dictionary<Entity, HealthBar>();
    //Grid.board.Attach(this);
    CameraControl cc = Camera.main.transform.parent.GetComponent<CameraControl>();
    cc.Attach(this);
    Notify();
  }

  public void Notify() {
    foreach(Warrior e in Grid.board.warriors.Keys) {
      if (!health_bars.ContainsKey(e)) {
        Transform hbt = Instantiate(health_bar_prefab) as Transform;
        hbt.SetParent(transform, false);
        HealthBar hb = hbt.GetComponent<HealthBar>();
        hb.SetEntity(e);
        health_bars[e] = hb;
      }

      health_bars[e].Notify();
    }
  }

}
