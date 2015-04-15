using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

  public Entity occupant;

  public int terrain_cost = 1;

  public TileButton btn_prefab;

  private TileButton uib;

  public virtual void Awake() {
    Grid.board.InitTile(this);
  }

  public virtual int Cost() {
    return IsOccupied() ? 0 : terrain_cost;
  }

  public virtual bool IsOccupied() {
    return occupant != null;
  }

  public virtual TileButton UISelectButton() {
    if (uib == null) {
      Transform ui = Grid.tile_canvas.transform;
      uib = Instantiate(btn_prefab);
      uib.rtrans.SetParent(ui, false);
      uib.rtrans.localPosition = Vector3.up * transform.position.z *
        (1f / ui.localScale.y) + Vector3.right * transform.position.x *
        (1f / ui.localScale.x);
      uib.name = Grid.board.CalculateLocation(transform.position).ToString();
    }
    
    uib.gameObject.SetActive(true);
    return uib;
  }
}
