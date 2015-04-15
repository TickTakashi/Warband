using UnityEngine;
using System.Collections;

public class HumanPlayer : Player {
  private Warrior selected;

  // TODO: Human players should control their units via normal input.
  public override IEnumerator TakeTurn() {
    yield return StartCoroutine(Grid.game.FadeUI(true));
    Grid.camera.can_control = true;
    while (turn && !captain.IsDead()) {
      yield return StartCoroutine(WaitForSelection());
      yield return StartCoroutine(WaitForCommandOrDeselection());
    }
    Grid.camera.can_control = false;
    yield return StartCoroutine(Grid.game.FadeUI(false));
  }

  public IEnumerator WaitForSelection() {
    while (turn && !captain.IsDead()) {
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
    while (turn && !captain.IsDead()) {
      if (Input.GetButtonDown("Fire1") && !UnityEngine.EventSystems.
        EventSystem.current.IsPointerOverGameObject()) {
          ClearSelection();
        break;
      }
      yield return null;
    }
  }


  void UpdateSelection(Warrior sel) {
    if (sel != selected) {
      ClearSelection();
      selected = sel;
      if (selected != null && selected.owner == Grid.game.current_player)
        selected.DisplayUI(true);
    }
  }

  void ClearSelection() {
    if (selected != null)
      selected.DisplayUI(false);
    
    selected = null;
  }
}
