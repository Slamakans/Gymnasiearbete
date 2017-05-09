using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RemoteControlPickupScript : MonoBehaviour
{
    public void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(PlayCutscene());
    }

    private Color orange = new Color(216, 180, 70, 255) / 255f;
    private Color pink = new Color(180, 25, 87, 255) / 255f;
    private float y_offset = 5;

    IEnumerator PlayCutscene ()
    {
        GameObject HUD = GameObject.Find("HUD");
        HUD.SetActive(false);

        GameObject player = Game.player;
        Rigidbody2D p_rb2d = player.GetComponent<Rigidbody2D>();

        Vector3 center = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        center = new Vector3(center.x, center.y, -17f);

        Debug.Log(center);

        GameObject tv_image = Instantiate(Resources.Load("Cutscene TV")) as GameObject;
        GameObject remote_image = Instantiate(Resources.Load("Cutscene Remote")) as GameObject;
        GameObject bg_image = Instantiate(Resources.Load("Cutscene BG")) as GameObject;

        bg_image.transform.localScale = new Vector3(100f, 100f, 1f);
        bg_image.transform.position = center;
        bg_image.GetComponent<SpriteRenderer>().color = orange;

        remote_image.transform.localScale = new Vector3(2f, 2f, 1f);
        remote_image.transform.position = center + new Vector3(0, 5, -1);

        tv_image.transform.localScale = new Vector3(2f, 2f, 1f);
        tv_image.transform.position = center + new Vector3(0, -5, -1);

        int bgSwitches = 0;

        int counter = 0;

        while (bgSwitches < 5)
        {
            counter++;
            p_rb2d.constraints = RigidbodyConstraints2D.FreezeAll;

            Debug.Log("counter: " + counter);
            if (counter % 17 == 0)
            {
                bgSwitches++;
                bg_image.GetComponent<SpriteRenderer>().color = bg_image.GetComponent<SpriteRenderer>().color == orange ? pink : orange;
            }

            remote_image.transform.position = remote_image.transform.position + new Vector3(0f, 0.01f);
            tv_image.transform.position = tv_image.transform.position + new Vector3(0f, -0.02f);

            remote_image.transform.localScale = remote_image.transform.localScale + new Vector3(0.002f, 0.002f);
            tv_image.transform.localScale = tv_image.transform.localScale + new Vector3(-0.004f, -0.004f);

            yield return new WaitForFixedUpdate();
        }

        HUD.SetActive(true);
        Destroy(tv_image);
        Destroy(remote_image);
        Destroy(bg_image);

        Player.HasRemote = true;
        Destroy(gameObject);
    }
}
