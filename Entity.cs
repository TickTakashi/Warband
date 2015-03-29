using UnityEngine;
using System.Collections;

public abstract class Entity : MonoBehaviour {
  public int health;
  public Player owner;

  public void Awake() {
    Board.InitEntity(this);
  } 
}
