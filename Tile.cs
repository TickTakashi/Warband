using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

  public Entity occupant;

  public int terrain_cost = 1;

  public virtual void Awake() {
    Board.InitTile(this);
  }

  public virtual int Cost() {
    return IsOccupied() ? 0 : terrain_cost;
  }

  public virtual bool IsOccupied() {
    return occupant != null;
  }
}
