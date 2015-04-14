using UnityEngine;
using System.Collections;
using UEL;
using System;

public abstract class Entity : UELBehaviour {
  public int health;
  public int max_health;
  public int owner;
  public EntityCanvas ui;
  public Player player { get { return Grid.game.players[owner]; } }
  public Location location { get { return Grid.board.GetLocation(this); } }

  public Transform portrait_cam;

  public event EventHandler<HealthChangeEventArgs> HealthChangeEvent;
  public event EventHandler<PositionChangeEventArgs> PositionChangeEvent;

  public void Awake() {
    Grid.board.InitEntity(this);

    if (max_health < health) {
      Debug.LogWarning("Entity - Awake - max_health less than health!");
      max_health = health;
    }

    Grid.game.BeginTurnEvent += new EventHandler<BeginTurnEventArgs>(OnTurnBegin);
  }

  public abstract void OnTurnBegin(object o, BeginTurnEventArgs e);

  public virtual void Damage(int value) {
    if (value > health)
      value = health;
    health -= value;
    HealthChangeEventArgs hcea = new HealthChangeEventArgs();
    hcea.delta = -value;
    if (HealthChangeEvent != null) {
      HealthChangeEvent(this, hcea);
    }
    // TODO: Add Hurt Animations.
    if (health == 0)
      Die();
  }

  public abstract void Die();


  public virtual void PositionChange(PositionChangeEventArgs e) {
    PositionChangeEvent(this, e);
  }
}

public class HealthChangeEventArgs : EventArgs {
  public int delta;
}

public class PositionChangeEventArgs : EventArgs {
  public bool moving = false;
}
