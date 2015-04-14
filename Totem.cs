using UnityEngine;
using System.Collections;

public class Totem : Entity {

  public int tactical_points = 1;

  public int GetTacticalPoints() {
    return tactical_points;
  }

  public override void OnTurnBegin(object o, BeginTurnEventArgs e) {
     Trigger();
  }

  public override void Die() {
    Grid.board.RemoveEntity(this);

    // TODO: Fade out the object instead of just deleting it.
    
    this.gameObject.SetActive(false); 
  }

  // TODO: Convert Totem to an abstract class, and change this to an abstract.
  public virtual void Trigger() {}
}
