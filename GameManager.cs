using UnityEngine;
using System.Collections;
using UEL;

public class GameManager : UELBehaviour {
  public Player[] players;
  public int current_player = 0;

  public Player Current() {
    return players[current_player];
  }

  public void EndTurn() {
    current_player = (current_player + 1) % players.Length;
    NotifyAll();
  }
}
