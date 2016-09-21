using UnityEngine;
using UnityEngine.UI;

public class StoneificationsUI_Number : MonoBehaviour
{
    private Sprite[] numbers;
    private Player player;
    private int cur, next;
    private int delta = 0;
    private int index = 1;
    private static float counterStart = 0.05f;
    private float counter = counterStart;

    void Start()
    {
        numbers = Resources.LoadAll<Sprite>("Sprites/123...");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        cur = 0;

        Debug.Log(numbers.Length);
    }

	void Update () {
        next = player.GetStoneifications();
        if (delta == 0) delta = next - cur;
        if (delta <= 0 && index <= 0) delta = 0;
       // Debug.Log("Cur: " + cur + "   Next: " + next + "    Delta: " + delta + "   Index: " + index);

        if(delta < 0)
        {
            counter -= Time.deltaTime;

            if(counter <= 0)
            {
                counter = counterStart;
                index--;
                if (index % 2 == 1)
                {
                    delta++;
                }
            }
        }
        else if(delta > 0)
        {
            counter -= Time.deltaTime;

            if(counter <= 0)
            {
                counter = counterStart;
                if(index % 2 == 1)
                {
                    index--;
                }
                else
                {
                    index += 3;
                    delta--;
                }
            }
        }

        GetComponent<Image>().sprite = numbers[index];

        cur = player.GetStoneifications();
	}
}
