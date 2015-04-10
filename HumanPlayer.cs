using UnityEngine;
using System.Collections;

public class HumanPlayer : Player {

  private bool turn = true;
  private Warrior selected;

  // TODO: Human players should control their units via normal input.
  public override IEnumerator TakeTurn() {
    while (Grid.game.current_tp > 0 && turn) {
      yield return StartCoroutine(WaitForSelection());
      yield return StartCoroutine(WaitForCommandOrDeselection());
    }

    turn = true;
  }

  public IEnumerator WaitForSelection() {
    Debug.Log("Waiting for Selection.");
    while (true) {
      if (Input.GetButtonDown("Fire1")) {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawLine(r.origin, r.origin + 10f * r.direction, Color.cyan);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit, Mathf.Infinity, Grid.board.entity_layer)) {
          Warrior s = hit.collider.GetComponent<Warrior>();
          if (s != null) {
            UpdateSelection(s);
            yield return null;
            break;
          }
        } else {
          Debug.Log("Raycast didn't hit anything.");
        }
      } 
      yield return null;
    }
  }

  public IEnumerator WaitForCommandOrDeselection() {
    // TODO: Wait for button press OR deselection by clicking somewhere else.
    Debug.Log("Character Selected, Waiting for command...");
    while (true) {
      if (Input.GetButtonDown("Fire1") && !UnityEngine.EventSystems.
        EventSystem.current.IsPointerOverGameObject()) {
          ClearSelection();
          Debug.Log("Clicked off: Deselecting...");
        break;
      }
      yield return null;
    }
  }

  public void ClickEndTurn() {
    turn = false;
  }

  void UpdateSelection(Warrior sel) {
    ClearSelection();
    this.selected = sel;

    if (sel.player == Grid.game.Current()) {
      Grid.ui.DisplayUIFor(sel, true);
    }
  }

  void ClearSelection() {
    if (this.selected != null)
      Grid.ui.DisplayUIFor(this.selected, false);
    
    this.selected = null;
  }
}
