using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //-----------------------------------------------------------------
    //Singleton
    //-----------------------------------------------------------------
    public static SoundManager Instance = null;
    //-----------------------------------------------------------------
    //AudioSources für Soundeffekte und Musik und Traktoren
    //-----------------------------------------------------------------
    [Header("Sources")]
    [SerializeField]
    private AudioSource m_SFXSource;
    [SerializeField]
    private AudioSource m_MusicSource;
    [SerializeField]
    private AudioSource m_TractorSource;
    [SerializeField]
    private AudioSource m_BeamSource;
    //-----------------------------------------------------------------
    //Pitch-Range von 5% für interessante Soundeffekte
    //-----------------------------------------------------------------
    [Header("Settings")]
    [SerializeField]
    private float m_LowPitchRange = .95f;
    [SerializeField]
    private float m_HighPitchRange = 1.05f;

    /// <summary>
    /// Wenn das Script aktiviert wird
    /// </summary>
    private void Awake()
    {
        //-----------------------------------------------------------------
        //Erstelle eine Instanz falls nicht vorhanden
        //-----------------------------------------------------------------
        if (Instance == null)
            Instance = this;
        //-----------------------------------------------------------------
        //..Oder lösche falls diese Instanz nicht die Instanz ist
        //-----------------------------------------------------------------
        else if (Instance != this)
            Destroy(gameObject);
    }
    
    /// <summary>
    /// Startet Beam-Loop
    /// </summary>
    public void PlayLooping()
    {
        m_BeamSource.Play();
    }

    /// <summary>
    /// Stoppt Beam-Loop
    /// </summary>
    public void StopLooping()
    {
        m_BeamSource.Stop();
    }

    /// <summary>
    /// Spielt einen Clip ab
    /// </summary>
    /// <param name="pi_Clip">Zu spielender Clip</param>
    public void PlaySingle(AudioClip pi_Clip)
    {
        //-----------------------------------------------------------------
        //Setzte Clip und spiele ab
        //-----------------------------------------------------------------
        m_SFXSource.clip = pi_Clip;       
        m_SFXSource.Play();
    }

    /// <summary>
    /// Spiele Clip mit random Pitch ab
    /// </summary>
    /// <param name="pi_Clip">Clip</param>
    public void RandomizeSFX(AudioClip pi_Clip)
    {
        //-----------------------------------------------------------------
        //Bestimme zufälligen Pitch
        //-----------------------------------------------------------------
        float l_RandomPitch = Random.Range(m_LowPitchRange, m_HighPitchRange);
        //-----------------------------------------------------------------
        //Setzte Pitch und Clip, dann spiele ab
        //-----------------------------------------------------------------
        m_SFXSource.pitch = l_RandomPitch;
        m_SFXSource.clip = pi_Clip;
        m_SFXSource.Play();
    }
}