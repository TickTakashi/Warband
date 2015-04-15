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
    routines.Add(BeginTurn());
    StartCoroutine(RunGame());
  }

  public List<IEnumerator> routines = new List<IEnumerator>();

  public static int DRAW = -3;
  public static int UNDECIDED = -1;
  public int GetWinner() {
    List<int> winners = new List<int>();

    for (int i = 0; i < players.Length; i++)
      if (!players[i].captain.IsDead())
        winners.Add(i);
    
    if (winners.Count == 0) {
      return DRAW;
    }

    if (winners.Count == 1) {
      return winners[0];
    }

    return UNDECIDED;
  }

  public IEnumerator RunGame() {
    yield return StartCoroutine(FadeToBlack(false));
    int winner = GetWinner();
    while (winner == UNDECIDED) {
      if (routines.Count == 0) {
        current_player = (current_player + 1) % players.Length;
        routines.Add(BeginTurn());
      }
      IEnumerator r = routines[0];
      routines.RemoveAt(0);
      yield return StartCoroutine(r);
      winner = GetWinner();
    }

    yield return StartCoroutine(FadeUI(false));
    if (winner == DRAW) {
      yield return StartCoroutine(UIBannerMessage("DRAW"));
    } else {
      yield return StartCoroutine(UIBannerMessage(TeamString(winner) +
        " VICTORY!"));
    }
    yield return StartCoroutine(FadeToBlack(true));
  }

  public CanvasGroup black_tex;

  public IEnumerator FadeToBlack(bool to_black) {
    float initial_alpha = black_tex.alpha;
    float final_alpha = to_black ? 1f : 0f;
    yield return StartCoroutine(Gradual(2f, (float f) => {
      black_tex.alpha = UELMethods.Smootherstep(initial_alpha, final_alpha, f);
    })); 
  }

  public IEnumerator BeginTurn() {
    current_tp = Current().MaxTacticalPoints();
    yield return StartCoroutine(UIBannerMessage(TeamString(current_player)));
    StartTurn();
    routines.Add(Current().TakeTurn());
  }

  public IEnumerator FadeUI(bool in_out) {
    float initial_alpha = tp_display.alpha;
    float final_alpha = in_out ? 1f : 0f;
    yield return StartCoroutine(Gradual(0.5f, (float f) => {
      tp_display.alpha = UELMethods.Smootherstep(initial_alpha, final_alpha, f);
      next_turn_button.alpha = UELMethods.Smootherstep(initial_alpha, final_alpha, f);
    }));
  }

  public IEnumerator UIBannerMessage(string message, float display_time = 1f) {
    // Step 2: Update the UI While its invisible
    BeginTurnEventArgs etea = new BeginTurnEventArgs();
    etea.player = current_player;
    BeginTurnEvent(this, etea);

    Text t = turn_panel.GetComponentInChildren<Text>();
    t.text = message;

    // Step 3: Move the camera to the current players Captain Warrior.
    Vector3 orig_pos = Grid.camera.transform.position;
    Vector3 capt_pos = Current().captain.transform.position;
    Transform cam_trans = Grid.camera.transform;
    yield return StartCoroutine(Gradual(0.5f, (float f) => {
      cam_trans.position = UELMethods.Smootherstep(orig_pos, capt_pos, f);
    }));

    // Step 4: Show the Team Banner.
    yield return StartCoroutine(Gradual(0.25f, (float f) => {
      turn_panel.alpha = UELMethods.Smootherstep(0f, 1f, f);
    }));

    yield return new WaitForSeconds(display_time);

    yield return StartCoroutine(Gradual(0.25f, (float f) => {
      turn_panel.alpha = UELMethods.Smootherstep(1f, 0f, f);
    }));
  }

  public string TeamString(int team) {
    return "Team <color=" + CurrentName(team) + ">" + CurrentName(team) + "</color>";
  }

  public string CurrentName(int player) {
    switch (player) {
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

  public void StartTurn() {
    Current().StartTurn();
    next_turn_button.interactable = true;
  }

  public void EndTurn() {
    if (Current().turn) {
      Current().EndTurn();
      next_turn_button.interactable = false;
    }
  }
}

public class BeginTurnEventArgs : EventArgs {
  public int player;
}

public class SpendEventArgs : EventArgs {
  public int spent;
}