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

  public static int Distance(Location a, Location b) {
    return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
  }

  public static bool operator ==(Location a, Location b) {
    return a.x == b.x && a.y == b.y;
  }

  public static bool operator !=(Location a, Location b) {
    return !(a.x == b.x && a.y == b.y); 
  }

  public static Location operator +(Location a, Location b) {
    Location added = new Location(a.x + b.x, a.y + b.y);
    return added;
  }

  public override string ToString() {
    return "(" + x.ToString() + ", " + y.ToString() + ")";
  }

  internal IEnumerable<Location> Range(int range) {
    for (int i = -range; i <= range; i++) {
      for (int j = -(range - Mathf.Abs(i)); j <= (range - Mathf.Abs(i)); j++) {
        yield return new Location(x + i, y + j);
      }
    } 
  }
}
