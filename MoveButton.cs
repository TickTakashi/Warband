using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveButton : WarriorAction {

  public override void DisplayRange() {
    Dictionary<Location, Path> routes = Grid.board.WarriorMoves(owner);
    TileDisplay(GameManager.player_colors[owner.owner], routes.Keys,
      delegate(Location l) { owner.FollowPath(routes[l]); });
  }

  public override int Cost() {
    return 1;
  }

  public override bool Interactable() {
    return base.Interactable() && !owner.moved;
  }
}
