using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UEL;

public class AIPlayer : Player {

  public void Start() {
    foreach (Warrior w in MyWarriors()){
      WarriorAI wai = w.GetComponent<WarriorAI>();
      if (wai == null) {
        Debug.Log("" + w.name + " didn't have AI. Adding PassiveAI now.");
        w.gameObject.AddComponent<PassiveAI>();
      }
    }
  }
  
  // TODO: AI Players should make choices automatically based on some
  // adversarial search strategy.
  public override IEnumerator TakeTurn() {
    // TODO: Implement basic AI.
    List<Warrior> all = MyWarriors();
    while (!captain.IsDead() && Grid.game.current_tp > 0 && all.Count > 0) {
      yield return new WaitForSeconds(1f);
      // DUMB AI: Moves random units.
      // SMARTER AI: Moves units that can actually attack enemies.
      // SMART AI: Moves units to strategic locations, and tries to attack important enemies.
      // Choose a Unit
      int rand = Random.Range(0, all.Count);
      Warrior w = all[rand];
      all.RemoveAt(rand);

      // Move the camera to look at the unit.
      Vector3 initial = Grid.camera.transform.position;
      yield return StartCoroutine(Gradual(0.5f, (f) => {
        Grid.camera.transform.position = UELMethods.Smootherstep(initial, w.transform.position, f); 
      }));

      // Act with the unit
      WarriorAI wai = w.GetComponent<WarriorAI>();
      yield return StartCoroutine(wai.Act());
    }
  }
}
