using UnityEngine;

public class FinishLine : MonoBehaviour {

    [Header("Settings")]
    [SerializeField]
    private Transform PlayerOne;
    [SerializeField]
    private Transform PlayerTwo;

    private void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material.mainTextureScale =  new Vector2(transform.localScale.x, transform.localScale.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == PlayerOne.name)
        {
            Debug.Log("Player One wins!");
        }
        else if(other.name == PlayerTwo.name)
        {
            Debug.Log("Player Two wins!");
        }
    }
}