using UnityEngine;
using System.Collections;

public class RandomizeAnimationSpeed : StateMachineBehaviour {

  public float delta = 0.1f;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    float b = 1f;
    animator.speed = Random.Range(b - delta, b + delta);
  }
}
