using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Board : MonoBehaviour {

  private static int tile_size = 2;
  private static Player[] players;
  private static Dictionary<Location, Tile> board = new Dictionary<Location, Tile>();
  private static Dictionary<Entity, Location> entities = new Dictionary<Entity, Location>();

  public static Vector3 CalculatePosition(Location l) {
    return board[l].transform.position;
  }

  public static Location CalculateLocation(Vector3 position) {
    return new Location(position.x / tile_size, position.z / tile_size);
  }

  public static Location GetLocation(Entity e) {
    return entities[e];
  }

  public static Tile GetTile(Location l) {
    return board.ContainsKey(l) ? board[l] : null;
  }

  public static void InitTile(Tile t) {
    Location l = CalculateLocation(t.transform.position);
    if (board.ContainsKey(l))
      throw new UnityException("Board - InitTile - Tile Initialized Twice!");

    board[l] = t;
  }

  public static void InitEntity(Entity e) {
    Location l = CalculateLocation(e.transform.position);
    if (entities.ContainsKey(e))
      throw new UnityException("Board - InitEntity - Entity Initialized Twice!");
    entities[e] = l;
    if (!board.ContainsKey(l))
      throw new UnityException("Board - InitEntity - No Tile To Stand On!");
    board[l].occupant = e;
  }

  public static void MoveEntity(Entity e, Location l) {
    board[entities[e]].occupant = null;
    entities.Remove(e);
    if (board[l].occupant != null) {
      throw new UnityException("Board - MoveEntity - Destination Not Empty!");
    }
    board[l].occupant = e;
    entities[e] = l;
  }

  public static Dictionary<Location, Path> WarriorMoves(Warrior warrior) {
    return Routes(entities[warrior], warrior.GetSpeed());
  }


  static string PrintBoard() {
    /* Debug board print. */
    string s = "";
    foreach (Location l in board.Keys) {
      s += l.ToString();
    }
    return s;
  }
  // Returns an array of paths for all locations reachable from a in budget.
  // while cheapest path is less than budget:
  //   For each neighbour of cheapest path
  //     new path = path including neighbor
  //     if new cheaper or equal to budget &&
  //       (if new path is cheaper than path in destinations dict ||
  //       dest doesnt contain it)
  //         replace destionations dict entry with new path
  //         if new cheaper than budget
  //           add current path to list of paths
  //   remove cheapest path from paths
  public static Dictionary<Location, Path> Routes(Location a, int budget) {
    // Hashmap to store destinations and the current best path to them
    Dictionary<Location, Path> dest = new Dictionary<Location, Path>();
    // List of current paths. (Starts with a)
    List<Path> paths = new List<Path>();
    foreach (Location n in a.Neighbors()) {
      Path path_n = new Path(new List<Location>() { n }, LocationCost(n));
      if (path_n.cost <= budget) {
        paths.Add(path_n);
        dest[n] = path_n;
      }
    }

    paths.Sort();

    while (paths.Count > 0 && paths[0].cost < budget) {
      Path cheapest_path = paths[0];
      foreach (Location neighbor in cheapest_path.End().Neighbors()){
        if (neighbor != cheapest_path.Penultimate()) {
          int ncost = LocationCost(neighbor);
          Path npath = new Path(cheapest_path, neighbor, ncost);
          if (npath.cost <= budget) {
            if (!dest.ContainsKey(neighbor) || npath < dest[neighbor]) {
              dest[neighbor] = npath;

              if (npath.cost < budget) {
                int index = paths.BinarySearch(npath);
                if (index < 0) {
                  index = ~index;
                }
                paths.Insert(index, npath);
              }
            }
          }
        }
      }
      paths.RemoveAt(0);
    }
    return dest;
  }

  public static int LocationCost(Location l) {
    return board.ContainsKey(l) ? board[l].Cost() : 0;
  }

  public static string PrintPaths(List<Path> paths) {
    string s = "";
    foreach(Path p in paths) {
      s += p.End();
    }

    return s;
  }

}
