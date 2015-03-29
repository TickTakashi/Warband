using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelector : MonoBehaviour {

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
      Debug.Log("Casting Ray!");
      Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
      Debug.DrawLine(r.origin, r.origin + 10f * r.direction, Color.cyan);
      RaycastHit hit_info;
      if (Physics.Raycast(r, out hit_info, entity_layer.value)) {
        Warrior s = hit_info.collider.GetComponent<Warrior>();
        if (s != null) {
          selected = s;
          UpdateSelection();
        }
      } else {
        ClearSelection();
      }
    }
	}

  void UpdateSelection() {
    ClearSelection();
    paths = Board.WarriorMoves(selected);
    Debug.Log("There are " + paths.Keys.Count + " destinations.");
    foreach (Location l in paths.Keys) {
      Transform marker = Instantiate(marker_prefab) as Transform;
      marker.position = Board.Position(l);
      markers.Add(marker);
    }
  }

  void ClearSelection() {
    for (int i = markers.Count - 1; i >= 0; i--) {
      Destroy(markers[i].gameObject);
    }
    markers.Clear();
  }
}
