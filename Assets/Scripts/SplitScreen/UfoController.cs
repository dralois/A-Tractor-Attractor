using UnityEngine;
using UnityEngine.UI;

public class UfoController : MonoBehaviour
{
    //--------------------------------------
    //Sonstiges
    //--------------------------------------
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
    private Transform ufoContainer;
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
    //--------------------------------------
    //Frustum
    //--------------------------------------
    private float FrustrumHeight;
    private Vector3 FrustrumScale;
    //--------------------------------------
    //MPS
    //--------------------------------------
    private int mashesPerSecond;
    private int mashCounter;
    private float mashInterval;
    private float lastMashUpdate;
    private bool firePressed;

    [SerializeField]
    private AudioClip teleportClip;
    private bool wasPulling= false;
    private Screen currScreen;

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
        //--------------------------------------
        //Bestimme Kamera
        //--------------------------------------
        selectCamera();
        //--------------------------------------
        //Bestimme Frustum
        //--------------------------------------
        FrustrumHeight = 2.0f * Vector3.Distance(currentTargetCam.transform.position, ufo.position) * Mathf.Tan(currentTargetCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        FrustrumScale = ufo.localScale * 5.0f;
        //--------------------------------------
        //Initialisiere Variablen
        //--------------------------------------
        mashCounter = 0;
        mashesPerSecond = 0;
        mashInterval = 1.0f;
        lastMashUpdate = 0;
        firePressed = false;
    }

    /// <summary>
    /// Gibt an ob der Traktorstrahl grade aktiv ist
    /// </summary>
    public bool isPulling()
    {
        return (Input.GetAxis("Fire_P" + playerIndex) > 0);
    }

    /// <summary>
    /// Richtung der Ufosteuerung
    /// </summary>
    public Vector2 getDirection()
    {
        return new Vector2(getCurrentScreen() == Screen.LEFT ? transform.position.x < (canvasInfo.CanvasWidth / 4.0f) ? -1.0f : 1.0f : transform.position.x < (3.0f* canvasInfo.CanvasWidth / 4.0f) ? -1.0f : 1.0f,
                           transform.position.y < (canvasInfo.CanvasHeight / 2.0f) ? -1.0f : 1.0f);
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
    /// Mashes pro Sekunde
    /// </summary>
    public int getMashesPerSecond()
    {
        return mashesPerSecond;
    }

    /// <summary>
    /// Player Index
    /// </summary>
    public int getPlayerIndex()
    {
        return playerIndex;
    }

    /// <summary>
    /// Updated Position
    /// </summary>
    private void FixedUpdate()
    {
        selectCamera();
        updateMoveRestriction();
        update2DPosition();
        clamp2DPosition();
        updateUfoPosition();
        updateMash();
    }

    /// <summary>
    /// Updated Skalierung und Sound
    /// </summary>
    private void Update()
    {
        float newScale = (2.0f * Vector3.Distance(currentTargetCam.transform.position, ufo.position) * Mathf.Tan(currentTargetCam.fieldOfView * 0.5f * Mathf.Deg2Rad));
        //ufo.localScale = FrustrumScale / (FrustrumHeight / newScale);
        if (isPulling())
        {
            if (!wasPulling)
            {
                SoundManager.Instance.PlayLooping();
                wasPulling = true;
            }
        }
        else if(wasPulling)
        {
            SoundManager.Instance.StopLooping();
            wasPulling = false;
        }
    }

    /// <summary>
    /// Aktualisiert Ufo-Position
    /// </summary>
    void updateUfoPosition()
    {
        ufoContainer.position = getWorldPosition();
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
            if(!CountDown.CountdownActive)
                if (this.getCurrentScreen() != currScreen)
                    SoundManager.Instance.RandomizeSFX(teleportClip);
            currScreen = Screen.LEFT;
        }
        else
        {
            currentTargetCam = playerCam2;
            int layer = LayerMask.NameToLayer(layerPlayer2);
            ufo.gameObject.layer = layer;
            beam.gameObject.layer = layer;
            if (!CountDown.CountdownActive)
                if (this.getCurrentScreen() != currScreen)
                    SoundManager.Instance.RandomizeSFX(teleportClip);
            currScreen = Screen.RIGHT;
        }

    }

    /// <summary>
    /// Bestimmt ob das Ufo die Seite wechseln kann oder nicht und updated die UI
    /// </summary>
    void updateMoveRestriction()
    {
        canSwitchSides = !(this.isPulling());
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
        ufoContainer.transform.rotation = Quaternion.Slerp(ufoContainer.transform.rotation,
                                                           Quaternion.Euler(maxRotation * y , 0, -maxRotation * x),
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
            float minX = getCurrentScreen() == Screen.LEFT ? (0 + this.width / 2.0f) : (canvasInfo.CanvasWidth / 2.0f + this.width / 2.0f);
            float maxX = getCurrentScreen() == Screen.LEFT ? (canvasInfo.CanvasWidth / 2.0f - this.width / 2.0f) : (canvasInfo.CanvasWidth - this.width / 2.0f);
            clampdPos.x = Mathf.Clamp(clampdPos.x, minX, maxX);
        }
        else
        {
            clampdPos.x = Mathf.Clamp(clampdPos.x, 0 + this.width / 2.0f, canvasInfo.CanvasWidth - this.width / 2.0f);
        }
        this.transform.position = clampdPos;
    }

    /// <summary>
    /// Berechne MashPerSecond
    /// </summary>
    void updateMash()
    {
        //--------------------------------------
        //Update Mashes
        //--------------------------------------
        if (Input.GetAxis("Fire_P" + playerIndex) > 0 && !firePressed)
        {
            mashCounter++;
            firePressed = true;
        }
        else if (Input.GetAxis("Fire_P" + playerIndex) == 0)
        {
            firePressed = false;
        }
        //--------------------------------------
        //Delta
        //--------------------------------------
        lastMashUpdate += Time.deltaTime;
        //--------------------------------------
        //Update MPS
        //--------------------------------------
        if (lastMashUpdate > mashInterval)
        {
            float mashes = mashCounter;
            mashesPerSecond = (int)(mashes / mashInterval);
            lastMashUpdate = 0;
            mashCounter = 0;
        }
    }
}