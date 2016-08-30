using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;
    public float TargetOrthographicSize = 10f;

    public float InitialDelay = 1.2f;

    // Update is called once per frame
    void Update()
    {
        if (InitialDelay > 0)
        {
            InitialDelay -= Time.deltaTime;
            return;
        }
        var curSize = GetComponent<Camera>().orthographicSize;
        if (Mathf.Abs(curSize - TargetOrthographicSize) < 0.2f) GetComponent<Camera>().orthographicSize = TargetOrthographicSize;
        else GetComponent<Camera>().orthographicSize = Mathf.Lerp(curSize, TargetOrthographicSize, 0.0175f);
        if (target)
        {
            Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
            Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
    }
}