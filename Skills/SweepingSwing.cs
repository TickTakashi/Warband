using UnityEngine;
using System.Collections;

public class SweepingSwing : BasicAttack {

  public override Location[] GetShape() {
    return new Location[] { new Location(0, 0), new Location(-1, 1),
                            new Location(0, 1), new Location(1, 1) };
  }
}
