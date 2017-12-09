using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{

    public Text textObject;

    private int counter;
    private float refTime;
    private float alpha;

    public static bool CountdownActive = false;

    void Start()
    {
        Reset();
    }

    void Update()
    {       
        if (counter > 0 && refTime >= 1.0f)
        {
            refTime = 0;
            counter--;
            if (counter == 0)
            {
                textObject.fontSize = 80;
                textObject.text = "GO!";
                StartCoroutine(evade());
                // Start the race
                CountdownActive = false;
            }
            else
            {
                textObject.text = "" + counter;
            }
        }
        else
        {
            refTime += Time.deltaTime;
        }
    }

    public void Reset()
    {
        alpha = 1.0f;
        counter = 3;
        refTime = 0;
        textObject.text = "" + counter;
        CountdownActive = true;
    }

    private IEnumerator evade()
    {
        while (alpha > 0)
        {
            alpha -= 0.01f;
            if (alpha < 0)
            {
                alpha = 0;
            }
            textObject.color = new Color(textObject.color.r, textObject.color.g, textObject.color.b, alpha);
            yield return new WaitForSeconds(0.01f);
        }
        textObject.enabled = false;
    }
}