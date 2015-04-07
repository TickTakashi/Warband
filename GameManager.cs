using UnityEngine;
using System.Collections;
using UEL;
using System;

public class GameManager : MonoBehaviour {
  public Player[] players;
  public int current_player = 0;

  public int current_tp = 0;

  public event EventHandler<BeginTurnEventArgs> BeginTurnEvent;
  public event EventHandler<SpendEventArgs> SpendEvent;

  public void Start() {
    BeginTurn();
  }

  public void BeginTurn() {
    current_tp = Current().MaxTacticalPoints();
    BeginTurnEventArgs etea = new BeginTurnEventArgs();
    etea.player = current_player;
    BeginTurnEvent(this, etea);
  }

  public bool CanSpend(int to_spend) {
    return current_tp >= to_spend;
  }

  public void Spend(int to_spend) {
    current_tp -= to_spend;
    SpendEventArgs sea = new SpendEventArgs();
    sea.spent = to_spend;
    SpendEvent(this, sea);
  }

  public Player Current() {
    return players[current_player];
  }

  public void EndTurn() {
    current_player = (current_player + 1) % players.Length;
    BeginTurn();
  }
}

public class BeginTurnEventArgs : EventArgs {
  public int player;
}

public class SpendEventArgs : EventArgs {
  public int spent;
}