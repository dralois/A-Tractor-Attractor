using UnityEngine;


public class BeamController : MonoBehaviour {

    private LineRenderer lr;

    [SerializeField]
    private UfoController playerCursor;
    [SerializeField]
    private Transform ufo;
    [SerializeField]
    private Transform vehicle1;
    [SerializeField]
    private Transform vehicle2;

    // Use this for initialization
    void Start () {
        this.lr = this.GetComponent<LineRenderer>();
        this.lr.positionCount = 2;
	}
	
	// Update is called once per frame
	void Update () {
        this.lr.SetPosition(0, ufo.position);

        if (playerCursor.getCurrentScreen() == Screen.LEFT)
        {
            this.lr.SetPosition(1, vehicle1.position);
            this.transform.LookAt(vehicle1);
        }
        else
        {
            this.lr.SetPosition(1, vehicle2.position);
            this.transform.LookAt(vehicle2);
        }
    }
}
