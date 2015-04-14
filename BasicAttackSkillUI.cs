using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UEL;

public class BasicAttackSkillUI : WarriorAction {

  public BasicAttack attack;

  public ShapeIcon shape_icon;
  public Text skill_name;
  public Text range;
  public Text damage;
  public Text cost;
  public Button btn;

  public void Start() {
    this.shape_icon.Init(attack.GetShape());
    this.skill_name.text = attack.skill_name;
    
    if (attack.min_range == attack.max_range)
      this.range.text = "" + attack.min_range;
    else
      this.range.text = "" + attack.min_range + "-" + attack.max_range;
    
    this.damage.text = "" + attack.power;
    this.cost.text = "" + attack.cost + "TP";
  }

  public override void DisplayRange() {
    TileDisplay(Color.red, attack.GetRange(), delegate(Location l) {
      attack.StartCoroutine(attack.Use(l));
    });
  }

  public override int Cost() {
    return attack.cost;
  }

  public override bool Interactable() {
    return base.Interactable() && !owner.casted;
  }
}
