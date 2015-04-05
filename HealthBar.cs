using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UEL;

public class HealthBar : UELBehaviour, IObserver {

  public Image bar;
  public Image underlay;

  private Entity e;
  private Rect initial_dimensions;
  private float offset = 0.75f;

  public void Awake() {
    initial_dimensions = bar.rectTransform.rect;
  }

  public void SetEntity(Entity e) {
    this.e = e;
    e.Attach(this);
  }

  public void SetValue(float percentage) {
    if (percentage > 1 || percentage < 0) {
      throw new UnityException("HealthBar - SetValue - Health overflow!");
    }

    bar.rectTransform.sizeDelta = new Vector2(
      initial_dimensions.width * percentage,
      initial_dimensions.width);

    bar.rectTransform.anchoredPosition = new Vector2(
      initial_dimensions.width * percentage / 2f, 0f);
  }


  public void Notify() {
    Vector3 position = e.transform.position + new Vector3(
      Grid.board.tile_size * offset / 2f,
      0,
      Grid.board.tile_size * offset / 2f);

    Vector2 viewpoint_point = Camera.main.WorldToViewportPoint(position);


    float percentage = (float)e.health / (float)e.max_health;
    SetValue(percentage);

    RectTransform rt = GetComponent<RectTransform>();
    rt.anchorMin = viewpoint_point;
    rt.anchorMax = viewpoint_point; 
  }

}
