using UnityEngine;
using UnityEngine.UI;

public class UfoController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private int playerIndex;
    [SerializeField]
    private Screen playerScreen;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float ufoHeight;
    [SerializeField]
    private float maxRotation;
    [SerializeField]
    private float rotationSpeed;
    [Header("Components")]
    [SerializeField]
    private Camera playerCam1;
    [SerializeField]
    private Camera playerCam2;
    [SerializeField]
    private Transform ufo;
    [SerializeField]
    private BeamController beam;
    [SerializeField]
    private Image border;
    [Header("Requirements")]
    [SerializeField]
    private string layerPlayer1= "Ufo_Player_1";
    [SerializeField]
    private string layerPlayer2 = "Ufo_Player_2";
    [SerializeField]
    private CanvasInformation canvasInfo;
    private Plane groundPlane;
    //--------------------------------------
    //Sprite Information for clamped movement
    //--------------------------------------
    private float width;
    private float height;
    private bool canSwitchSides;
    //--------------------------------------
    //Aktuelle Kamera
    //--------------------------------------
    private Camera currentTargetCam;

    private float FrustrumHeight;
    private Vector3 FrustrumScale;

    void Start()
    {
        //--------------------------------------
        //Bestimme Sprite-Dimensionen
        //--------------------------------------
        RectTransform objectRectTransform = gameObject.GetComponent<RectTransform>();
        width = objectRectTransform.rect.width;
        height = objectRectTransform.rect.height;
        //-----------------------------------------
        //Erstelle Ebene für 3D-Positionsbestimmung
        //-----------------------------------------
        groundPlane = new Plane(Vector3.up, new Vector3(0, ufoHeight,0));

        selectCamera();

        FrustrumHeight = 2.0f * Vector3.Distance(currentTargetCam.transform.position, ufo.position) * Mathf.Tan(currentTargetCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        FrustrumScale = ufo.localScale;
    }

    /// <summary>
    /// Gibt an ob der Traktorstrahl grade aktiv ist
    /// </summary>
    /// <returns></returns>
    public bool isPulling()
    {
        return (Input.GetAxis("Fire_P" + playerIndex) > 0);
    }

    /// <summary>
    /// Position in 3D
    /// </summary>
    public Vector3 getWorldPosition()
    {
        //--------------------------------------
        //Erstelle Ray
        //--------------------------------------
        Vector2 screenPos = new Vector2(this.transform.position.x, this.transform.position.y);
        Ray r = RectTransformUtility.ScreenPointToRay(currentTargetCam, screenPos);
        //--------------------------------------
        //Caste Ray, gebe Punkt..
        //--------------------------------------
        float rayDistance;
        if (groundPlane.Raycast(r, out rayDistance))
        {
            return r.GetPoint(rayDistance);
        }
        //--------------------------------------
        //..Oder Null
        //--------------------------------------
        return Vector3.zero;
    }

    /// <summary>
    /// Aktueller Screen
    /// </summary>
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

    /// <summary>
    /// Player Index
    /// </summary>
    public int getPlayerIndex()
    {
        return playerIndex;
    }

    private void FixedUpdate()
    {
        updateMoveRestriction();
        update2DPosition();
        clamp2DPosition();
        selectCamera();
        updateUfoPosition();
    }

    private void Update()
    {
        //float newScale = (2.0f * Vector3.Distance(currentTargetCam.transform.position, ufo.position) * Mathf.Tan(currentTargetCam.fieldOfView * 0.5f * Mathf.Deg2Rad));
        //ufo.localScale = FrustrumScale / (FrustrumHeight / newScale);
    }

    /// <summary>
    /// Aktualisiert Ufo-Position
    /// </summary>
    void updateUfoPosition()
    {
        ufo.position = getWorldPosition();
    }

    /// <summary>
    /// Bestimme benutzte Kamera
    /// </summary>
    void selectCamera()
    {
        if (this.getCurrentScreen() == Screen.LEFT)
        {
            currentTargetCam = playerCam1;
            int layer = LayerMask.NameToLayer(layerPlayer1);
            ufo.gameObject.layer = layer;
            beam.gameObject.layer = layer;
        }
        else
        {
            currentTargetCam = playerCam2;
            int layer = LayerMask.NameToLayer(layerPlayer2);
            ufo.gameObject.layer = layer;
            beam.gameObject.layer = layer;
        }
    }

    /// <summary>
    /// Bestimmt ob das Ufo die Seite wechseln kann oder nicht und updated die UI
    /// </summary>
    void updateMoveRestriction()
    {
        canSwitchSides = !(this.isPulling() && this.getCurrentScreen() == playerScreen);
        border.color = new Color(border.color.r, border.color.g, border.color.b, canSwitchSides ?  0.5f : 1.0f);
    }

    /// <summary>
    /// Aktualisiert GUI (2D)
    /// </summary>
    void update2DPosition()
    {
        //--------------------------------------
        //Hole Achsenindex (je nach Spieler)
        //--------------------------------------
        float x = Input.GetAxis("Horizontal_P" + playerIndex);
        float y = Input.GetAxis("Vertical_P" + playerIndex);
        Vector2 direction = new Vector2(x,y); //TODO evtl normalizen
        //--------------------------------------
        //Bestimme neue Position
        //--------------------------------------
        Vector3 offset = new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
        //--------------------------------------
        //Ufo Rotation
        //--------------------------------------
        ufo.transform.rotation = Quaternion.Slerp(ufo.transform.rotation,
                                                  Quaternion.Euler(maxRotation * y, 0 , -maxRotation * x),
                                                  rotationSpeed * Time.deltaTime);
        //--------------------------------------
        //Translate
        //--------------------------------------
        this.transform.position +=  offset;
    }

    /// <summary>
    /// Beschränke Positionen (Bildschirmrand)
    /// </summary>
    void clamp2DPosition()
    {
        Vector3 clampdPos = this.transform.position;
        clampdPos.y = Mathf.Clamp(clampdPos.y, 0 + this.height / 2.0f, canvasInfo.CanvasHeight - this.height / 2.0f);
        if (!canSwitchSides)
        {
            float minX = playerScreen == Screen.LEFT ? (0 + this.width / 2.0f) : (canvasInfo.CanvasWidth / 2.0f + this.width / 2.0f);
            float maxX = playerScreen == Screen.LEFT ? (canvasInfo.CanvasWidth / 2.0f - this.width / 2.0f) : (canvasInfo.CanvasWidth - this.width / 2.0f);
            clampdPos.x = Mathf.Clamp(clampdPos.x, minX, maxX);
        }
        else
        {
            clampdPos.x = Mathf.Clamp(clampdPos.x, 0 + this.width / 2.0f, canvasInfo.CanvasWidth - this.width / 2.0f);
        }
        this.transform.position = clampdPos;
    }
}