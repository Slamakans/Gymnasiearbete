using System;
using System.Collections;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    public float TargetOrthographicSize = 10f;

    public float InitialDelay = 1.2f;
    private float timePassed = 0;

    private GameObject levelImage;
    private Player player;

    public bool Frozen = true;
    public bool Confine = true;
    private bool panning = false;

    public float leftBound, rightBound, bottomBound, topBound;

    public Transform[] waypoints = new Transform[] { };

    void Start()
    {
        levelImage = GameObject.FindGameObjectWithTag("LevelImage");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        /*
        // set the desired aspect ratio (the values in this example are
        // hard-coded for 4:3, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = 4f / 3f;

        // determine the game window's current aspect ratio
        float windowaspect = (float) Screen.width / Screen.height;

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
        }*/

        StartCoroutine(PanWaypointsAndUnfreeze());
    }

    void FixedUpdate()
    {
        if (!Frozen)
        {
            if (!panning) SmoothFollowPlayer();
            ConfineToBounds(levelImage.GetComponent<SpriteRenderer>().bounds);
        }
    }

    void LateUpdate()
    {
        // Panning is done after FixedUpdate's ConfineToBounds call, so we need to do it again here
        if (panning)
        {
            ConfineToBounds(levelImage.GetComponent<SpriteRenderer>().bounds);
        }
    }

    private IEnumerator PanWaypointsAndUnfreeze()
    {
        panning = true;
        while (Frozen)
        {
            if (InitialDelay - timePassed > 0)
            {
                timePassed += Time.deltaTime;
                // ConfineToBounds(levelImage.GetComponent<SpriteRenderer>().bounds);
                player.rb2d.constraints = RigidbodyConstraints2D.FreezePositionX;
            }
            else
            {
                timePassed = InitialDelay;
                Frozen = false;
            }
            yield return new WaitForFixedUpdate();
        }

        if (waypoints.Length == 0)
        {
            panning = false;
        }
        else
        {
            foreach (Transform t in waypoints)
            {
                bool done = false;
                StartCoroutine(PanToTransform(t, trans => done = true));
                yield return new WaitUntil(() => done);
                panning = true;
            }

            yield return new WaitForSeconds(0.1f);
            panning = false;
        }

        // yield return new WaitUntil(() => );
        player.GetComponent<Animator>().SetTrigger("spawn");
        yield return new WaitWhile(() => player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Spawn"));
        player.rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private bool ConfineToBounds(Bounds bounds)
    {
        if (!Confine) return false;
        // Debug.Log("Image height: " + bounds.size.y);
        // X fucked
        // Y fine
        float vertExtent = Camera.main.orthographicSize;
        float horzExtent = vertExtent * (Screen.width / (float)Screen.height);
        // Debug.Log(Screen.width + "        " + (float)Screen.height);

        leftBound = bounds.min.x + horzExtent;//(horzExtent / 8f) * 5f;// * 0.75f;
        rightBound = bounds.max.x - horzExtent;//(horzExtent / 8f) * 5f;// * 0.75f;
        bottomBound = bounds.min.y + vertExtent;
        topBound = bounds.max.y - vertExtent;

        // Debug.Log(minX + "   " + maxX + "   " + minY + "   " + maxY);

        //Debug.Log();
        Vector3 p = new Vector3(target.position.x, target.position.y, transform.position.z);
        p.x = Mathf.Clamp(p.x, leftBound, rightBound);
        p.y = Mathf.Clamp(p.y, bottomBound, topBound);
        // transform.position = p;

        bool hadToConfine = p.x != transform.position.x || p.y != transform.position.y;
        if (hadToConfine) transform.position = p;
        Game.HadToConfineCamera = hadToConfine;
        return hadToConfine;
    }

    private void SmoothFollowPlayer()
    {
        SmoothFollowTransform(target);
    }

    private void SmoothFollowTransform(Transform t)
    {
        SmoothFollowTransformWithDamp(t, dampTime);
    }

    private void SmoothFollowTransformWithDamp(Transform t, float damping)
    {
        Camera cam = GetComponent<Camera>();
        var curSize = cam.orthographicSize;
        if (Mathf.Abs(curSize - TargetOrthographicSize) < 0.2f) cam.orthographicSize = TargetOrthographicSize;
        else cam.orthographicSize = Mathf.Lerp(curSize, TargetOrthographicSize, 0.0175f);

        if (t)
        {
            Vector3 point = cam.WorldToViewportPoint(t.position);
            Vector3 delta = t.position - cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); // (new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, damping);
        }
    }

    public IEnumerator PanToTransform(Transform t, Func<Transform, bool> callback)
    {
        panning = true;
        while (Mathf.Abs(transform.position.x - t.position.x) > 0.15f)
        {
            SmoothFollowTransformWithDamp(t, Mathf.Max((transform.position - t.position).magnitude / 60f, 0.15f));
            yield return new WaitForFixedUpdate();
        }

        if (callback != null)
        {
            callback(t);
        }
        panning = false;
    }
}