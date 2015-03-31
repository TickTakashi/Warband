using UnityEngine;
using System.Collections;
using UEL;

public abstract class Entity : UELBehaviour {
  public int health;
  public Player owner;

  public void Awake() {
    Board.InitEntity(this);
  } 
}
