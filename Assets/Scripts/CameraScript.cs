using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    public float TargetOrthographicSize = 10f;

    public float InitialDelay = 1.2f;
    private float timePassed = 0;

    private GameObject levelImage;

    void Start()
    {
        levelImage = GameObject.FindGameObjectWithTag("LevelImage");

        // set the desired aspect ratio (the values in this example are
        // hard-coded for 4:3, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = 4f / 3f;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect r = camera.rect;

            r.width = 1.0f;
            r.height = scaleheight;
            r.x = 0;
            r.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = r;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect r = camera.rect;

            r.width = scalewidth;
            r.height = 1.0f;
            r.x = (1.0f - scalewidth) / 2.0f;
            r.y = 0;

            camera.rect = r;
        }
    }

    void LateUpdate()
    {
        if (InitialDelay - timePassed > 0)
        {
            timePassed += Time.deltaTime;
            ConfineToBounds(levelImage.GetComponent<SpriteRenderer>().bounds);
            return;
        }
        else timePassed = InitialDelay;

        if (ConfineToBounds(levelImage.GetComponent<SpriteRenderer>().bounds))
        {
            SmoothFollowPlayer();
        }
    }

    private bool ConfineToBounds(Bounds bounds)
    {
        Debug.Log("Image height: " + bounds.size.y);
        float vertExtent = Camera.main.orthographicSize;
        float horzExtent = vertExtent * Screen.width / Screen.height;

        float minY = vertExtent - (bounds.size.y / 2f) + bounds.extents.y / 2;
        float maxY = (bounds.size.y / 2f) - vertExtent + bounds.extents.y / 2;
        float minX = horzExtent - (bounds.size.x / 2f) + bounds.extents.x / 2;
        float maxX = (bounds.size.x / 2f) - horzExtent + bounds.extents.x / 2;
        Debug.Log("minY: " + minY);
        Debug.Log("maxY: " + maxY);
        Debug.Log("minX: " + minX);
        Debug.Log("maxX: " + maxX);
        Vector3 p = transform.position;
        p.x = Mathf.Clamp(p.x, minX, maxX);
        p.y = Mathf.Clamp(p.y, minY, maxY);
        bool hadToConfine = p.x != transform.position.x || p.y != transform.position.y;
        transform.position = p;
        return hadToConfine;
    }

    private void SmoothFollowPlayer()
    {
        var curSize = GetComponent<Camera>().orthographicSize;
        if (Mathf.Abs(curSize - TargetOrthographicSize) < 0.2f) GetComponent<Camera>().orthographicSize = TargetOrthographicSize;
        else GetComponent<Camera>().orthographicSize = Mathf.Lerp(curSize, TargetOrthographicSize, 0.0175f);
        if (target)
        {
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
            Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); // (new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }
}