using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField]
    private int playerIndex;
    [SerializeField]
    private float speed;
    [SerializeField]
    private LayerMask raycastLayerMask;
    [SerializeField]
    private float ufoHeight;
    [Header("Components")]
    [SerializeField]
    private Camera playerCam1;
    [SerializeField]
    private Camera playerCam2;
    [SerializeField]
    private Transform ufo;
    [Header("Requirements")]
    [SerializeField]
    private string layerPlayer1= "Ufo_Player_1";
    [SerializeField]
    private string layerPlayer2 = "Ufo_Player_2";
    [SerializeField]
    private CanvasInformation canvasInfo;
    private Plane groundPlane;

    private float width;
    private float height;

    private Camera currentTargetCam;

    private Image imgComp;
    private Color c;

    void Start()
    {

        RectTransform objectRectTransform = gameObject.GetComponent<RectTransform>();
        width = objectRectTransform.rect.width;
        height = objectRectTransform.rect.height;

        groundPlane = new Plane(Vector3.up, new Vector3(0, ufoHeight,0));

        imgComp = this.GetComponent<Image>();
        c = imgComp.color;
    }


    public Vector3 getWorldPosition()
    {
        return Vector3.zero;
    }

    public Screen getCurrentScreen()
    {
        if(this.transform.position.x  > canvasInfo.CanvasWidth / 2.0)
        {
            return Screen.RIGHT;
        }
        else
        {
            return Screen.LEFT;
        }
    }

    void Update()
    {
        update2DPosition();
        clamp2DPosition();
        selectCamera();
        updateUfoPosition();

        //TEST
        if (this.getCurrentScreen() == Screen.LEFT)
        {
            imgComp.color = c;
        }
        else
        {
            imgComp.color = new Color(c.r, c.g, c.b, 0.5f);
        }
    }



    void updateUfoPosition()
    {
        RectTransform rect = this.GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        rect.GetWorldCorners(corners);
        Vector3 center = (corners[0] + corners[1] + corners[2] + corners[3]) / 4.0f;

        Vector2 screenPos = new Vector2(this.transform.position.x , this.transform.position.y);
        Ray r = RectTransformUtility.ScreenPointToRay(currentTargetCam, screenPos);

        float rayDistance;
        if (groundPlane.Raycast(r, out rayDistance))
            ufo.position = r.GetPoint(rayDistance);
    }

    void selectCamera()
    {
        if (this.getCurrentScreen() == Screen.LEFT)
        {
            currentTargetCam = playerCam1;
            ufo.gameObject.layer = LayerMask.NameToLayer(layerPlayer1);
        }
        else
        {
            currentTargetCam = playerCam2;
            ufo.gameObject.layer = LayerMask.NameToLayer(layerPlayer2);
        }
    }

    void update2DPosition()
    {
        float x = Input.GetAxis("Horizontal_P" + playerIndex);
        float y = Input.GetAxis("Vertical_P" + playerIndex);
        Vector2 direction = new Vector2(x,y); //TODO evtl normalizen

        Vector3 offset = new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
        this.transform.position +=  offset;
    }

    void clamp2DPosition()
    {
        Vector3 clampdPos = this.transform.position;
        clampdPos.x = Mathf.Clamp(clampdPos.x, 0 + this.width / 2.0f, canvasInfo.CanvasWidth - this.width / 2.0f);
        clampdPos.y = Mathf.Clamp(clampdPos.y, 0 + this.height / 2.0f, canvasInfo.CanvasHeight- this.height / 2.0f);
        this.transform.position = clampdPos;
    }
}
