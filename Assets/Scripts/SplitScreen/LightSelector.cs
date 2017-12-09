using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSelector : MonoBehaviour {

    [SerializeField]
    List<Light> playerSpecificLights;
    [SerializeField]
    LayerMask showLigtsInLayer;

    void OnPreCull()
    {
        foreach (Light light in playerSpecificLights)
        {
            if (showLigtsInLayer == (showLigtsInLayer | (1 << light.gameObject.layer)))
            {
                light.enabled = true;
            }
        }
    }

    void OnPostRender()
    {
        foreach (Light light in playerSpecificLights)
        {
            light.enabled = false;
        }
    }
}
