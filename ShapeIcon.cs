using UnityEngine;
using System.Collections;

public class ShapeIcon : MonoBehaviour {

  public Transform square;
  public float square_size = .20f;
  public float square_spacing = .05f;

	// Use this for initialization
	public void Init (Location[] hits) {
    //float square_size = this.square_size * 1080 /
    RectTransform trans = transform.GetComponent<RectTransform>();
    foreach (Location l in hits) {
      RectTransform s = (Instantiate(square) as Transform).GetComponent<RectTransform>();
      s.SetParent(trans, false);
      s.sizeDelta = new Vector2(square_size, square_size);
      s.anchoredPosition = new Vector2(l.x * (square_size + square_spacing),
                                       l.y * (square_size + square_spacing));
    }
	}
}
