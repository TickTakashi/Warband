using UnityEngine;

static class Grid {
  public static Board board;
  public static GameManager game;
  public static Canvas canvas;
  public static Canvas tile_canvas;
  public static CameraControl camera;

  // when the program launches, Grid will check that all the needed elements are in place
  // that's exactly what you do in the static constructor here:
  static Grid() {
    GameObject g;

    g = safeFind("__Board");
    board = (Board)SafeComponent(g, "Board");

    g = safeFind("__GameManager");
    game = (GameManager)SafeComponent(g, "GameManager");

    g = safeFind("__ScreenUI");
    canvas = (Canvas)SafeComponent(g, "Canvas");

    g = safeFind("__Camera");
    camera = (CameraControl)SafeComponent(g, "CameraControl");

    g = safeFind("__TileUI");
    tile_canvas = (Canvas)SafeComponent(g, "Canvas");

    // PS. annoying arcane technical note - remember that really, in c# static constructors do not run
    // until the FIRST TIME YOU USE THEM.  almost certainly in any large project like this, Grid
    // would be called zillions of times by all the Awake (etc etc) code everywhere, so it is
    // a non-issue. but if you're just testing or something, it may be confusing that (for example)
    // the wake-up alert only appears just before you happen to use Grid, rather than "when you hit play"
    // you may want to call "SayHello" from the GeneralOperations.cs, just to decisively start this script.
  }

  // when Grid wakes up, it checks everything is in place - it uses these routines to do so
  private static GameObject safeFind(string s) {
    GameObject g = GameObject.Find(s);
    if (g == null) BigProblem("The " + s + " game object is not in this scene. You're stuffed.");
    return g;
  }

  private static Component SafeComponent(GameObject g, string s) {
    Component c = g.GetComponent(s);
    if (c == null) BigProblem("The " + s + " component is not there. You're stuffed.");
    return c;
  }

  private static void BigProblem(string error) {
    for (int i = 10; i > 0; --i) Debug.LogError(" >>>>>>>>>>>> Cannot proceed... " + error);
    for (int i = 10; i > 0; --i) Debug.LogError(" !!! Is it possible you just forgot to launch from scene zero, the __preEverythingScene scene.");
    Debug.Break();
  }
}
