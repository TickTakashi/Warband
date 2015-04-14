using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Player : MonoBehaviour {
  // TODO: A Player needs a deck, a hand, and possibly some other parameters
  // like max hand size. Players will probably also need to know about all 
  // their owned units.
  public bool turn = true;
  public Warrior captain;

  public int MaxTacticalPoints() {
    int total = 1;

    foreach (Totem t in Grid.board.totems.Keys)
      if (t.player == this)
        total += t.GetTacticalPoints();
    
    return total;
  }

  public void EndTurn() {
    turn = false;
  }

  public void StartTurn() {
    turn = true;
  }

  
  public abstract IEnumerator TakeTurn();
}
