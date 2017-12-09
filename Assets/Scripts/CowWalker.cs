using UnityEngine;

public class CowWalker : MonoBehaviour {

    [SerializeField]
    private float speed;
    private Rigidbody rigid;
    private bool rotating = false;

	// Use this for initialization
	void Start () {
        this.rigid = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        bool walk = Random.Range(0, 100) < 40;
        if (walk)
        {
            this.rigid.AddForce(this.transform.forward * speed);
        }


        rotating = (Random.Range(0, 100) < 5.0f) ^ rotating;
        if(rotating)
        {
            this.transform.rotation *= Quaternion.AngleAxis(0.1f, Vector3.up);
        }
    }
}
