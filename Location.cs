using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct Location {
  public int x;
  public int y;

  public Location(int x, int y) {
    this.x = x;
    this.y = y;
  }

  public Location(float x, float y) {
    this.x = Mathf.RoundToInt(x);
    this.y = Mathf.RoundToInt(y);
  }

  public IEnumerable<Location> Neighbors() {
    yield return new Location(x - 1, y);
    yield return new Location(x + 1, y);
    yield return new Location(x, y + 1);
    yield return new Location(x, y - 1);
  }

  public static bool operator ==(Location a, Location b) {
    return a.x == b.x && a.y == b.y;
  }

  public static bool operator !=(Location a, Location b) {
    return !(a.x == b.x && a.y == b.y); 
  }

  public override string ToString() {
    return "(" + x.ToString() + ", " + y.ToString() + ")";
  }
}
