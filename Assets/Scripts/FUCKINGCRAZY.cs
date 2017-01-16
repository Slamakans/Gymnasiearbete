using UnityEngine;
using UnityEngine.UI;

public class FUCKINGCRAZY : MonoBehaviour
{
    public bool redOn = true;
    public bool blueOn = true;
    public bool greenOn = true;
    private float counter = 0;

	void Update ()
    {
        counter += 0.05f;
        float red = Mathf.Abs(Mathf.Sin(counter % 2) * 255);
        float blue = Mathf.Abs(Mathf.Sin(counter % 2) * 255);
        float green = Mathf.Abs(Mathf.Sin(counter % 2) * 255);
        GetComponent<Image>().color = new Color(redOn ? red / 255 : 0, blueOn ? blue / 255 : 0, greenOn ? green / 255 : 0);

        var p = transform.localPosition;
        transform.localPosition = new Vector3(p.x * -1, -1 * p.y, p.z);
	}
}
