using UnityEngine;
using System.Collections;

public class HumanPlayer : Player {


  private Warrior selected;

  // TODO: Human players should control their units via normal input.
  public override IEnumerator TakeTurn() {
    while (Grid.game.current_tp > 0 && turn) {
      yield return StartCoroutine(WaitForSelection());
      yield return StartCoroutine(WaitForCommandOrDeselection());
    }
  }

  public IEnumerator WaitForSelection() {
    while (turn) {
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
        }
      } 
      yield return null;
    }
  }

  public IEnumerator WaitForCommandOrDeselection() {
    // TODO: Wait for button press OR deselection by clicking somewhere else.
    while (turn) {
      if (Input.GetButtonDown("Fire1") && !UnityEngine.EventSystems.
        EventSystem.current.IsPointerOverGameObject()) {
          ClearSelection();
        break;
      }
      yield return null;
    }
  }


  void UpdateSelection(Warrior sel) {
    ClearSelection();
    selected = sel;
    if (selected != null && selected.owner == Grid.game.current_player)
      selected.DisplayUI(true);
  }

  void ClearSelection() {
    if (selected != null)
      selected.DisplayUI(false);
    
    selected = null;
  }
}
