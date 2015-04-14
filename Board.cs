using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UEL;

public class Board : UELBehaviour {

  public int tile_size = 2;
  public LayerMask entity_layer;
  
  private Dictionary<Location, Tile> board = new Dictionary<Location, Tile>();

  public Dictionary<Warrior, Location> warriors = new Dictionary<Warrior,Location>();
  public Dictionary<Totem, Location> totems = new Dictionary<Totem,Location>();


  public Vector3 CalculatePosition(Location l) {
    return board[l].transform.position;
  }

  public Location CalculateLocation(Vector3 position) {
    return new Location(position.x / tile_size, position.z / tile_size);
  }




  public Location GetLocation(Entity e) {
    Warrior w = (Warrior)e;
    if (w != null)
      return GetLocation(w);
    else
      return GetLocation((Totem)e);
  }

  public Location GetLocation(Warrior w) {
    return warriors[w];
  }

  public Location GetLocation(Totem t) {
    return totems[t];
  }

  public Tile GetTile(Location l) {
    return board.ContainsKey(l) ? board[l] : null;
  }

  public Tile GetTile(Vector3 pos) {
    Location tile_loc = CalculateLocation(pos);
    return GetTile(tile_loc);
  }
  public void InitTile(Tile t) {
    Location l = CalculateLocation(t.transform.position);
    if (board.ContainsKey(l))
      throw new UnityException("Board - InitTile - Tile Initialized Twice!");

    board[l] = t;
  }

  public void RemoveEntity(Entity e) {
    Location l = CalculateLocation(e.transform.position);
    Warrior w = e as Warrior;
    Totem t = e as Totem;

    if (!(w != null && warriors.ContainsKey(w) || t != null && totems.ContainsKey(t)))
      throw new UnityException("Board - RemoveEntity - Entity not present!");

    if (t != null)
      totems.Remove(t);
    else
      warriors.Remove(w);
 
    if (!board.ContainsKey(l))
      throw new UnityException("Board - RemoveEntity - No Tile To Stand On!");

    board[l].occupant = null;
  }

  public void InitEntity(Entity e) {
    Location l = CalculateLocation(e.transform.position);
    Warrior w = e as Warrior;
    Totem t = e as Totem;

    if (w != null && warriors.ContainsKey(w) || t != null && totems.ContainsKey(t))
      throw new UnityException("Board - InitEntity - Entity Initialized Twice!");

    if (t != null)
      totems[t] = l;
    else
      warriors[w] = l;

    if (!board.ContainsKey(l))
      throw new UnityException("Board - InitEntity - No Tile To Stand On!");
    board[l].occupant = e;
  }

  public void MoveEntity(Warrior w, Location l) {
    board[warriors[w]].occupant = null;
    warriors.Remove(w);
    if (board[l].occupant != null) {
      throw new UnityException("Board - MoveEntity - Destination Not Empty!");
    }
    board[l].occupant = w;
    warriors[w] = l;
  }


  public Dictionary<Location, Path> WarriorMoves(Warrior warrior) {
    return Routes(warrior.location, warrior.GetSpeed());
  }


  string PrintBoard() {
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
  public Dictionary<Location, Path> Routes(Location a, int budget) {
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
                if (index < 0) 
                  index = ~index;
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

  public int LocationCost(Location l) {
    return board.ContainsKey(l) ? board[l].Cost() : 0;
  }

  public string PrintPaths(List<Path> paths) {
    string s = "";
    foreach(Path p in paths)
      s += p.End();
    return s;
  }
}
