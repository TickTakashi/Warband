using UnityEngine;
using System.Collections;
using UEL;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : UELBehaviour {
  public Player[] players;
  public int current_player = 0;
  public CanvasGroup next_turn_button;
  public CanvasGroup tp_display;
  public CanvasGroup turn_panel;

  public int current_tp = 0;

  public event EventHandler<BeginTurnEventArgs> BeginTurnEvent;
  public event EventHandler<SpendEventArgs> SpendEvent;

  public static Color[] player_colors = new Color[] {
    Color.cyan,
    Color.yellow,
    Color.magenta,
  };

  public void Start() {
    StartCoroutine(BeginTurn());
  }

  public IEnumerator BeginTurn() {
    Grid.camera.can_control = false;
    current_tp = Current().MaxTacticalPoints();
    Current().StartTurn();
    yield return StartCoroutine(AnimateTurnStart());

    Grid.camera.can_control = true;
    yield return StartCoroutine(Current().TakeTurn());

    current_player = (current_player + 1) % players.Length;
    yield return StartCoroutine(BeginTurn());
  }

  public IEnumerator AnimateTurnStart() {
    // Step 1: Fade out the UI
    float initial_alpha = tp_display.alpha;
    yield return StartCoroutine(Gradual(1f, (float f) => {
      tp_display.alpha = UELMethods.Smootherstep(initial_alpha, 0f, f);
      next_turn_button.alpha = UELMethods.Smootherstep(initial_alpha, 0f, f);
    }));
    
    // Step 2: Update the UI While its invisible
    BeginTurnEventArgs etea = new BeginTurnEventArgs();
    etea.player = current_player;
    BeginTurnEvent(this, etea);
    
    Text t = turn_panel.GetComponentInChildren<Text>();
    t.text = "Team <color=" + CurrentName() + ">" + CurrentName() + "</color>";

    // Step 3: Move the camera to the current players Captain Warrior.
    Vector3 orig_pos = Grid.camera.transform.position;
    Vector3 capt_pos = Current().captain.transform.position;
    Transform cam_trans = Grid.camera.transform;
    yield return StartCoroutine(Gradual(1f, (float f) => {
      cam_trans.position = UELMethods.Smootherstep(orig_pos, capt_pos, f);
    }));

    // Step 4: Show the Team Banner.
    yield return StartCoroutine(Gradual(0.5f, (float f) => {
      turn_panel.alpha = UELMethods.Smootherstep(0f, 1f, f);
    }));

    yield return new WaitForSeconds(1f);

    yield return StartCoroutine(Gradual(0.5f, (float f) => {
      turn_panel.alpha = UELMethods.Smootherstep(1f, 0f, f);
    }));

    // Step 5: Fade In the UI.
    yield return StartCoroutine(Gradual(1f, (float f) => {
      tp_display.alpha = UELMethods.Smootherstep(0f, 1f, f);
      next_turn_button.alpha = UELMethods.Smootherstep(0f, 1f, f);
    }));
  }

  public string CurrentName() {
    switch (current_player) {
      case 0:
        return "Cyan";
      case 1:
        return "Yellow";
      case 2:
        return "Magenta";
      default:
        throw new UnityException("GameManager - CurrentName - Player index not supported");
    } 
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
    Current().EndTurn();
  }
}

public class BeginTurnEventArgs : EventArgs {
  public int player;
}

public class SpendEventArgs : EventArgs {
  public int spent;
}