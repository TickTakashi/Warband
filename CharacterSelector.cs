using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UEL;
using System;

public class CharacterSelector : MonoBehaviour {
  /*public LayerMask entity_layer;
  public Warrior selected;
  public Transform marker_prefab;
  private Dictionary<Location, Path> paths;
  private List<Transform> markers;

  public event EventHandler<CharacterSelectedEventArgs> CharacterSelectedEvent;

  void Start() {
    markers = new List<Transform>();
    Grid.game.BeginTurnEvent += new EventHandler<BeginTurnEventArgs>(OnTurnBegin);
  }

	// Update is called once per frame
	void Update () {
    if (Input.GetButtonDown("Fire1")) {
      Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
      Debug.DrawLine(r.origin, r.origin + 10f * r.direction, Color.cyan);
      RaycastHit hit;
      if (Physics.Raycast(r, out hit, Mathf.Infinity, entity_layer)) {
        Warrior s = hit.collider.GetComponent<Warrior>();
        if (s != null) {
          UpdateSelection(s);
        }

        if (hit.collider.gameObject.tag == "Selector") {
          MoveUnit(selected, Grid.board.CalculateLocation(hit.collider.transform.position));
          ClearSelection();
        }
      } else {
        Debug.Log("Didn't hit anything.");
        ClearSelection();
      }
    }

    if (Input.GetButtonDown("Fire2")) {
      ClearSelection();
    }
	}

  void MoveUnit(Warrior selected, Location destination) {
    // TODO: Check if its our turn.
    // TODO: Check if we are allowed to move right now
    int move_cost = 1;
    if (Grid.game.CanSpend(move_cost)) {
      selected.FollowPath(paths[destination]);
      Grid.game.Spend(move_cost);
    }
  }

  void UpdateSelection(Warrior sel) {
    ClearSelection();
    this.selected = sel;

    if (sel.player == Grid.game.Current()) {
      Grid.ui.DisplayUIFor(sel, true);
    }

    CharacterSelectedEventArgs csea = new CharacterSelectedEventArgs();
    csea.character = sel;
    CharacterSelectedEvent(this, csea);
  }

  public void DisplayMovement() {
    ClearSelection();
    paths = Grid.board.WarriorMoves(selected);
    foreach (Location l in paths.Keys) {
      Transform marker = Instantiate(marker_prefab) as Transform;
      marker.position = Grid.board.CalculatePosition(l);
      markers.Add(marker);
    }
  }

  // TODO: Make the move buttons UI objects and remove this crap.
  void ClearSelection() {
    Deselect();
    for (int i = markers.Count - 1; i >= 0; i--) {
      Destroy(markers[i].gameObject);
    }
    markers.Clear();
  }

  void Deselect() {
    if (this.selected != null) {
      Grid.ui.DisplayUIFor(this.selected, false);
    }

    this.selected = null;
  }

  public void OnTurnBegin(object o, BeginTurnEventArgs e) {
    ClearSelection();
  }*/
}

public class CharacterSelectedEventArgs : EventArgs {
  public Entity character;
}