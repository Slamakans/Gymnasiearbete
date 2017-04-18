/**
 * Put this script in a controller-like object, or perhaps your main camera.
 * Hold ctrl to move in increments of `snapValue`
 */

using UnityEngine;

[ExecuteInEditMode]
public class GridSnapScript : MonoBehaviour
{
    public float snapValue = 4;
    public float depth = 0;

    void Update()
    {
        /*float snapInverse = 1 / snapValue;

        float x, y, z;

        // if snapValue = 16, x = 1.45 -> snapInverse = 0.0625 -> x*0.0625 => 0.090625 -> round 0.090625 => 0 -> 0/0.0625 => 0
        // so 1.45 to nearest .5 is 1.5
        x = Mathf.Round(Mathf.Round(transform.position.x * snapInverse) / snapInverse);
        y = Mathf.Round(Mathf.Round(transform.position.y * snapInverse) / snapInverse);
        z = depth;  // depth from camera

        //if (x == 0 && y == 0) return;
        Debug.Log("x: " + x + ", y: " + y);

        transform.position = new Vector3(x, y, z);*/
    }
}