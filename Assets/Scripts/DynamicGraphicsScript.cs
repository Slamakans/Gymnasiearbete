using UnityEngine;


[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class DynamicGraphicsScript : MonoBehaviour {
    public Sprite LightSprite;
    public Sprite DarkSprite;
    public Sprite GrassSprite;
    public bool Growth = false;
    public bool SideGrowth = false;
    private SpriteRenderer grassRenderer;
    private SpriteRenderer sideGrassRendererLeft;
    private SpriteRenderer sideGrassRendererRight;

    public void Awake()
    {
    }
    
    void Update () {
        if (!LightSprite || !DarkSprite) return;
        if (Growth)
        {
            if (!grassRenderer) grassRenderer = transform.Find("GrassRenderer") ? transform.Find("GrassRenderer").GetComponent<SpriteRenderer>() : null;
            if (!sideGrassRendererLeft) sideGrassRendererLeft = transform.Find("SideGrassRendererLeft") ? transform.Find("SideGrassRendererLeft").GetComponent<SpriteRenderer>() : null;
            if (!sideGrassRendererRight) sideGrassRendererRight = transform.Find("SideGrassRendererRight") ? transform.Find("SideGrassRendererRight").GetComponent<SpriteRenderer>() : null;
        }

        SpriteRenderer mainRenderer = GetComponent<SpriteRenderer>();

        Collider2D topCollider = Physics2D.Raycast(transform.position, Vector2.up, mainRenderer.sprite.bounds.size.y / 1.8f).collider;
        Collider2D leftCollider = null;
        Collider2D rightCollider = null;

        if (sideGrassRendererLeft) leftCollider = Physics2D.Raycast(transform.position, Vector2.left, mainRenderer.sprite.bounds.size.x / 1.8f).collider;
        if (sideGrassRendererRight) rightCollider = Physics2D.Raycast(transform.position, Vector2.right, mainRenderer.sprite.bounds.size.x / 1.8f).collider;

        bool left = leftCollider && leftCollider.CompareTag("Platform");
        bool right = rightCollider && rightCollider.CompareTag("Platform");

        // Debug.DrawLine(transform.position, (Vector2) transform.position + (new Vector2(mainRenderer.sprite.bounds.size.x / 1.8f, 0)), Color.blue, 0.02f);

        Sprite newMain = topCollider && topCollider.CompareTag("Platform") ? DarkSprite : LightSprite;

        if (mainRenderer.sprite != newMain)
        {
            mainRenderer.sprite = newMain;
        }

        if (Growth)
        {
            if (grassRenderer) grassRenderer.sprite = newMain == LightSprite ? GrassSprite : null;

            if (SideGrowth)
            {
                if (!left && !sideGrassRendererLeft.sprite) sideGrassRendererLeft.sprite = GrassSprite;
                else if (left && sideGrassRendererLeft.sprite) sideGrassRendererLeft.sprite = null;

                if (!right && !sideGrassRendererRight.sprite) sideGrassRendererRight.sprite = GrassSprite;
                else if (right && sideGrassRendererRight.sprite) sideGrassRendererRight.sprite = null;
            }
            else
            {
                if (sideGrassRendererLeft && sideGrassRendererLeft.sprite)
                {
                    sideGrassRendererLeft.sprite = null;
                }

                if (sideGrassRendererRight && sideGrassRendererRight.sprite)
                {
                    sideGrassRendererRight.sprite = null;
                }
            }
        }
    }
}
