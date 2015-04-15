using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PassiveAI : WarriorAI {

  // Passive AI:
  //   Does nothing until an enemy unit is close enough to attack.
  //   if an enemy is close enough to attack, it attacks them.
  public override IEnumerator Act() {
    BasicAttack[] attacks = w.GetComponents<BasicAttack>();
    Debug.Log(name + " is Calculating Paths...");
    Dictionary<Location, Path> routes = Grid.board.WarriorMoves(w);

    // "Can hit A from a list of possible locations B by using skill C at location D."
    Dictionary<Location, List<Pair<Location, Pair<BasicAttack, Location>>>> can_hit_a_from_b = new Dictionary<Location, List<Pair<Location, Pair<BasicAttack, Location>>>>();
    List<Entity> targets = new List<Entity>();

    Debug.Log(name + " is Calculating places it can hit...");
    // Find all places that we can hit
    foreach (Location possible_destination in routes.Keys) {
      foreach (BasicAttack skill in attacks) {
        Debug.Log(name + " is considering " + skill.skill_name + "...");
        foreach (Pair<Location, Location> hittable in skill.Hittable(possible_destination)){
          if (!can_hit_a_from_b.ContainsKey(hittable.first))
            can_hit_a_from_b[hittable.first] = new List<Pair<Location, Pair<BasicAttack, Location>>>();
          can_hit_a_from_b[hittable.first].Add(
            new Pair<Location, Pair<BasicAttack, Location>>(possible_destination,
              new Pair<BasicAttack, Location>(skill, hittable.second)));
        }
      }
    }

    Debug.Log(name + " is Considering Enemies we can hit...");
    // Find all enemy entities that we can hit
    foreach (Location possible_target in can_hit_a_from_b.Keys) {
      Tile t = Grid.board.GetTile(possible_target);
      if (t != null && t.occupant != null && t.occupant.owner != w.owner) {
        targets.Add(t.occupant);
      } 
    }

    // If there are no valid enemy targets, end thinking.
    if (targets.Count == 0) {
      Debug.Log(name + " couldn't find anything to hit.");
      yield break;
    }

    Debug.Log(name + " is selecting the most vulnerable target...");
    // Select the most vulnerable target from the closest targets
    targets.Sort((a, b) => a.health.CompareTo(b.health));
    Entity final_target = targets[0];

    Debug.Log(name + " decided to attack " + final_target.name);
    // Get the list of actions we can perform on this target
    List<Pair<Location, Pair<BasicAttack, Location>>> actions = can_hit_a_from_b[final_target.location];

    Debug.Log(name + " is choosing the best attack to use on the target...");
    // Find the deadliest action
    actions.Sort((a, b) => b.second.first.power.CompareTo(a.second.first.power));
    Pair<Location, Pair<BasicAttack, Location>> final_action = actions[0];

    Debug.Log(name + " decided to hit " + final_target.name + " with " + final_action.second.first.skill_name);
    
    Location final_destination = final_action.first;
    Debug.Log(name + " is moving towards the target...");
    // Move to the location such that we can hit the target.
    w.FollowPath(routes[final_destination]);
    do {
      // Note: Keep the camera on the unit.
      Grid.camera.transform.position = w.transform.position;
      yield return null;
    } while (!w.ready);

    // Use the deadliest skill on the appropriate target location.
    BasicAttack final_skill = final_action.second.first;
    Location final_skill_target = final_action.second.second;

    Debug.Log(name + " is attacking with " + final_skill.skill_name + "!");
    yield return StartCoroutine(final_skill.Use(final_skill_target));
  }
}
