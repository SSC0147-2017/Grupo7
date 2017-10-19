public class ScreenShake : Shake {
    new void Start() {
        Target = SpaceshipsUtils.FindLevel(gameObject).Camera.transform.parent;
        base.Start();
    }
}