using UnityEngine;
using System.Collections;
using UEL;

public abstract class Entity : UELBehaviour {
  public int health;
  public int max_health;
  public Player owner;
  public Location location { get { return Grid.board.GetLocation(this); } }
  public Transform portrait_cam;

  public void Awake() {
    Grid.board.InitEntity(this);

    if (max_health < health) {
      Debug.LogWarning("Entity - Awake - max_health less than health!");
      max_health = health;
    }
  }

  public void Start() {
    NotifyAll();
  }
}
