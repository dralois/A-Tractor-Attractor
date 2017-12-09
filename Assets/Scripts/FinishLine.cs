using UnityEngine;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour {

    [Header("Settings")]
    [SerializeField]
    private Transform PlayerOne;
    [SerializeField]
    private string PlayerOneName;
    [SerializeField]
    private Transform PlayerTwo;
    [SerializeField]
    private string PlayerTwoName;
    [SerializeField]
    private Text Winner;
    [SerializeField]
    private AudioClip WinSound;


    private int p1RoundCounter;
    private int p2RoundCounter;
    [SerializeField]
    private Text p1TurnGui;
    [SerializeField]
    private Text p2TurnGui;

    bool finished;


    private void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material.mainTextureScale =  new Vector2(transform.localScale.x, transform.localScale.z);
        p1RoundCounter = 1;
        p2RoundCounter = 1;
        finished = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (finished)
            return;
        if(other.name == PlayerOne.name)
        {
            if (other.GetComponent<Rigidbody>().position.x < this.transform.position.x)
            {
                if (p1RoundCounter == 3 && !finished)
                {
                    Winner.text = PlayerOneName.ToUpper() + " WINS!";
                    SoundManager.Instance.PlaySingle(WinSound);
                    finished = true;
                }
                else
                {
                    p1RoundCounter++;
                }
            }
            else
            {
                p1RoundCounter--;
            }

            p1TurnGui.text = p1RoundCounter + "/3";
        }

        else if(other.name == PlayerTwo.name)
        {
            if (other.GetComponent<Rigidbody>().position.x < this.transform.position.x)
            {
                if (p2RoundCounter == 3 && !finished)
                {
                    Winner.text = PlayerTwoName.ToUpper() + " WINS!";
                    SoundManager.Instance.PlaySingle(WinSound);
                    finished = true;
                }
                else
                {
                    p2RoundCounter++;
                }
            }
            else
            {
                p2RoundCounter--;
            }

            p2TurnGui.text = p2RoundCounter + "/3";
        }
    }
}