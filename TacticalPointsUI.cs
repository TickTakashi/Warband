using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UEL;
using System;

public class TacticalPointsUI : UELBehaviour {

  public Text tactical_points;

  void Start() {
    Grid.game.SpendEvent += new EventHandler<SpendEventArgs>(OnSpend);
    Grid.game.BeginTurnEvent += new EventHandler<BeginTurnEventArgs>(OnBeginTurn);
    SetTPText();
  }

	public void OnSpend(object o, SpendEventArgs e) {
    SetTPText();
	}

  public void OnBeginTurn(object o, BeginTurnEventArgs e) {
    SetTPText();
  }

  public void SetTPText() {
    tactical_points.text = "" + Grid.game.current_tp;
  }
}
