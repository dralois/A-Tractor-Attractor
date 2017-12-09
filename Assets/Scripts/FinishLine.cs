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

    private void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material.mainTextureScale =  new Vector2(transform.localScale.x, transform.localScale.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == PlayerOne.name)
        {
            Winner.text = PlayerOneName.ToUpper() + " WINS!";
        }
        else if(other.name == PlayerTwo.name)
        {
            Winner.text = PlayerTwoName.ToUpper() + " WINS!";
        }
    }
}