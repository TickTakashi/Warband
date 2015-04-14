using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TileButton : UIElement {
  public Button btn;
  public Button.ButtonClickedEvent onClick { get { return btn.onClick; } }

  internal void SetColour(Color colour) {
    btn.image.color = colour;
  }

  public void SetFunction(UnityEngine.Events.UnityAction action) {
    btn.onClick.RemoveAllListeners();
    btn.onClick.AddListener(action);
  }
}
