using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class WarriorAction : UIElement {
  public Warrior owner;
  public bool available;

  public abstract void DisplayRange();
  public abstract int Cost();
  public delegate void Action(Location l);

  public void Awake() {
    Grid.game.BeginTurnEvent += new EventHandler<BeginTurnEventArgs>(OnTurnBegin);
  }

  public void OnTurnBegin(object o, BeginTurnEventArgs e) {
    if (e.player == owner.owner) {
      available = true;
    }
  }

  public void TileDisplay(Color col, IEnumerable<Location> locations, Action action) {
    owner.DisplayUI(false);
    foreach (Location l in locations) {
      Tile target = Grid.board.GetTile(l);
      if (target != null) {
        TileButton select_button = target.UISelectButton();
        children.Add(select_button);
        select_button.SetColour(col);
        Location tmp = l;
        select_button.SetFunction(() => {
          available = false;
          Spend();
          action(tmp);
        });
      }
    }
  }

  public override void Visible(bool is_visible) {
    if (!is_visible) {
      ChildrenVisible(is_visible);
      children.Clear();
    }
    canvas_group.alpha = is_visible ? 1f : 0f;
    canvas_group.blocksRaycasts = is_visible;
    canvas_group.interactable = is_visible && Interactable();
  }

  public virtual bool Interactable() {
    return available && HasTP();
  }

  public override void ChildrenVisible(bool is_visible) {
    foreach (UIElement child in children) {
      child.gameObject.SetActive(is_visible);
    }
  }

  private bool HasTP() {
    return Grid.game.CanSpend(Cost());
  }

  private void Spend() {
    Grid.game.Spend(Cost());
  }
}
