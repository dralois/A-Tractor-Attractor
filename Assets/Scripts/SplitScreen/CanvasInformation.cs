using UnityEngine;

public class CanvasInformation : MonoBehaviour {

    public float CanvasWidth { get; private set; }
    public float CanvasHeight { get; private set; }
    
    void Start ()
    {
        //--------------------------------------
        //Speichere Canvas-Dimensionen
        //--------------------------------------
        RectTransform objectRectTransform = gameObject.GetComponent<RectTransform>();
        CanvasWidth = objectRectTransform.rect.width;
        CanvasHeight = objectRectTransform.rect.height;
    }
}