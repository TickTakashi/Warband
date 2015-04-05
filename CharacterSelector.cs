using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UEL;

public class CharacterSelector : UELBehaviour {

  public LayerMask entity_layer;
  public Warrior selected;
  public Transform marker_prefab;
  private Dictionary<Location, Path> paths;
  private List<Transform> markers;

  void Start() {
    markers = new List<Transform>();
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
          selected = s;
          UpdateSelection();
        }

        if (hit.collider.gameObject.tag == "Selector") {
          MoveUnit(selected, Grid.board.CalculateLocation(hit.collider.transform.position));
          ClearSelection();
        }
      } else {
        ClearSelection();
      }
    }
	}

  void MoveUnit(Warrior selected, Location destination) {
    // TODO: Check if its our turn.
    // TODO: Check if we are allowed to move right now.
    selected.FollowPath(paths[destination]);
  }

  void UpdateSelection() {
    ClearSelection();
    paths = Grid.board.WarriorMoves(selected);
    foreach (Location l in paths.Keys) {
      Transform marker = Instantiate(marker_prefab) as Transform;
      marker.position = Grid.board.CalculatePosition(l);
      markers.Add(marker);
    }

    NotifyAll();
  }

  void ClearSelection() {
    for (int i = markers.Count - 1; i >= 0; i--) {
      Destroy(markers[i].gameObject);
    }
    markers.Clear();
  }
}
