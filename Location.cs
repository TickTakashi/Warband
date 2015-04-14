using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Location {
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
    return Range(0, range);
  }

  internal IEnumerable<Location> Range(int min, int max) {
    for (int i = -max; i <= max; i++) {
      for (int j = -(max - Mathf.Abs(i)); j <= (max - Mathf.Abs(i)); j++) {
        if (Mathf.Abs(i) + Mathf.Abs(j) >= min) {
          yield return new Location(x + i, y + j);
        }
      }
    } 
  }

  public override bool Equals(object obj) {
    if (obj == null) {
      return false;
    }

    Location p = obj as Location;
    if ((System.Object)p == null) {
      return false;
    }

    return (x == p.x) && (y == p.y);
  }

  public override int GetHashCode() {
    int hash = 17;
    hash = hash * 47 + x;
    hash = hash * 47 + y;
    return hash;
  }
}
