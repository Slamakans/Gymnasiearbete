using UnityEngine;
using System.Collections;

public class StoneificationsUI : MonoBehaviour {
    public Transform stoneImage;

    private Player player;
    private int curStones = 0;
    private ArrayList images = new ArrayList();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        curStones = player.GetStoneifications();
        if(curStones < images.Count)
        {
            var stone = images[images.Count - 1] as RectTransform;
            Destroy(stone.gameObject);
            images.RemoveAt(images.Count - 1);
        }
        else if(curStones > images.Count)
        {
            var stone = Instantiate(stoneImage, transform) as RectTransform;
            stone.anchoredPosition = GetPos();
            images.Add(stone);
        }
    }

    Vector3 GetPos()
    {
        return new Vector3(200f + (images.Count * 50f), -50f, 0);
    }
}