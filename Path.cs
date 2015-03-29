using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Path : IComparable<Path> {
  public int cost = 0;
  public List<Location> path;

  public Path(List<Location> path, int cost) {
    AddCost(cost);
    this.path = new List<Location>(path);
  }

  public Path(Path p, Location next, int next_cost)
    : this(p.path, p.cost) {
    path.Add(next);
    AddCost(next_cost);
  }

  private void AddCost(int next_cost) {
    if (next_cost == 0 || cost == int.MaxValue)
      cost = int.MaxValue;
    else
      cost += next_cost;
  }

  public Location End() {
    return path[path.Count - 1];
  }

  public Location Penultimate() {
    return path.Count > 1 ? path[path.Count - 2] : End();
  }

  public static bool operator <(Path a, Path b) {
    return a.cost < b.cost || (a.cost == b.cost &&
      a.path.Count < b.path.Count);
  }

  public static bool operator >(Path a, Path b) {
    return a.cost > b.cost || (a.cost == b.cost &&
      a.path.Count > b.path.Count);
  }

  public int CompareTo(Path other) {
    if (other == null)
      return 1;
    else if (this < other) {
      return -1;
    } else if (this.path.Count == other.path.Count) {
      return 0;
    } else {
      return 1;
    }
  }
}