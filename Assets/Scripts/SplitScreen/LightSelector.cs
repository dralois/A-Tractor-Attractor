using System.Collections.Generic;
using UnityEngine;

public class LightSelector : MonoBehaviour {

    [SerializeField]
    List<Light> playerSpecificLights;
    [SerializeField]
    LayerMask showLightsInLayer;

    void OnPreCull()
    {
        //--------------------------------------
        //Aktiviere Lichter
        //--------------------------------------
        foreach (Light light in playerSpecificLights)
        {
            if (showLightsInLayer == (showLightsInLayer | (1 << light.gameObject.layer)))
            {
                light.enabled = true;
            }
        }
    }

    void OnPostRender()
    {
        //--------------------------------------
        //Deaktiviere Lichter
        //--------------------------------------
        foreach (Light light in playerSpecificLights)
        {
            light.enabled = false;
        }
    }
}