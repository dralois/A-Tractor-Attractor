using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasInformation : MonoBehaviour {

    public float CanvasWidth { get; private set; }
    public float CanvasHeight { get; private set; }

    // Use this for initialization
    void Start ()
    {
        RectTransform objectRectTransform = gameObject.GetComponent<RectTransform>();
        CanvasWidth = objectRectTransform.rect.width;
        CanvasHeight = objectRectTransform.rect.height;
        Debug.Log(CanvasWidth);
        Debug.Log(CanvasHeight);
    }
}
