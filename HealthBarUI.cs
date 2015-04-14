using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UEL;
using System;

public class HealthBarUI : UELBehaviour {
  public Image bar_mask;
  public Image underlay;
  public Image bar;
  public Entity entity;
  private Rect initial_dimensions;

  public void Awake() {
    initial_dimensions = bar_mask.rectTransform.rect;
    entity.HealthChangeEvent += new EventHandler<HealthChangeEventArgs>(OnHealthChange);
    bar.color = GameManager.player_colors[entity.owner];
    SetValue();
  }

  private void OnHealthChange(object sender, HealthChangeEventArgs e) {
    SetValue();
  }

  public void SetValue() {
    float percentage = (float)entity.health / (float)entity.max_health;
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
