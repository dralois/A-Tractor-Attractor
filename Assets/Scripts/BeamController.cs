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
        this.lr.positionCount = 12;
	}
	
	// Update is called once per frame
	void Update () {
        this.lr.SetPosition(11, ufo.position);        

        if (playerCursor.getCurrentScreen() == Screen.LEFT)
        {
            for(int i = 0; i < 10; i++)
            {
                this.lr.SetPosition(i+1, (vehicle1.position * (10-i)/11.0f + ufo.position* (i+1)/11.0f));
            }
            this.lr.SetPosition(0, vehicle1.position);
            this.transform.LookAt(vehicle1);
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                this.lr.SetPosition(i + 1, (vehicle2.position * (10- i) / 11.0f + ufo.position * (i+1) / 11.0f));
            }
            this.lr.SetPosition(0, vehicle2.position);
            this.transform.LookAt(vehicle2);
        }

        if (playerCursor.isPulling())
        {
            Gradient l_Next = lr.colorGradient;
            GradientAlphaKey[] l_All = l_Next.alphaKeys;

            l_All[1].time = (l_All[1].time + Time.deltaTime) % 1.0f;            
            l_Next.alphaKeys = l_All;

            this.lr.colorGradient = l_Next;
        }
    }
}