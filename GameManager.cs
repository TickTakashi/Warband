using UnityEngine;
using System.Collections;
using UEL;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
  public Player[] players;
  public int current_player = 0;

  public int current_tp = 0;

  public event EventHandler<BeginTurnEventArgs> BeginTurnEvent;
  public event EventHandler<SpendEventArgs> SpendEvent;

  public static Color[] player_colors = new Color[] {
    Color.green,
    Color.red,
    Color.cyan,
    Color.yellow,
    Color.blue
  };

  public void Start() {
    StartCoroutine(BeginTurn());
  }

  public IEnumerator BeginTurn() {
    current_tp = Current().MaxTacticalPoints();
    BeginTurnEventArgs etea = new BeginTurnEventArgs();
    etea.player = current_player;
    BeginTurnEvent(this, etea);
    yield return StartCoroutine(Current().TakeTurn());
    current_player = (current_player + 1) % players.Length;
    yield return StartCoroutine(BeginTurn());
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
}

public class BeginTurnEventArgs : EventArgs {
  public int player;
}

public class SpendEventArgs : EventArgs {
  public int spent;
}