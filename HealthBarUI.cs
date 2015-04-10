using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UEL;
using System;

public class HealthBarUI : FollowEntity {
  public Image bar_mask;
  public Image underlay;
  public Image bar;

  private Rect initial_dimensions;

  public void Awake() {
    initial_dimensions = bar_mask.rectTransform.rect;
  }

  public override void SetEntity(Entity e) {
    base.SetEntity(e);
    e.HealthChangeEvent += new EventHandler<HealthChangeEventArgs>(OnHealthChange);
    bar.color = GameManager.player_colors[e.owner];
    SetValue();
  }

  private void OnHealthChange(object sender, HealthChangeEventArgs e) {
    MaintainPosition();
  }

  public void SetValue() {
    float percentage = (float)e.health / (float)e.max_health;
    if (percentage > 1 || percentage < 0) {
      throw new UnityException("HealthBar - SetValue - Health overflow!");
    }

    bar_mask.rectTransform.sizeDelta = new Vector2(
      initial_dimensions.width * percentage,
      initial_dimensions.height);

    bar_mask.rectTransform.anchoredPosition = new Vector2(
      initial_dimensions.width * percentage / 2f, 0f);
  }
}
