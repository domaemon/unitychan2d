using UnityEngine;

[RequireComponent(typeof(UnityChan2DController), typeof(AudioSource))]
public class Demo : MonoBehaviour
{
    [SerializeField]
    private AudioClip damageVoice;

    [SerializeField]
    private AudioClip[] jumpVoices;

    [SerializeField]
    private AudioClip clearVoice;

    [SerializeField]
    private AudioClip timeOverVoice;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        string liveFaceCamMode = PlayerPrefs.GetString("Live FaceCam Mode");
        
        if (Application.loadedLevelName == "World 1-1")
        {
            if (Everyplay.IsRecordingSupported())
            {
                switch (liveFaceCamMode) {
                case "off":
                    break;
                case "audio":
                    if (Everyplay.FaceCamIsRecordingPermissionGranted())
                    {
                        Everyplay.FaceCamSetAudioOnly(true);
                        Everyplay.FaceCamStartSession();
                    }
                    break;
                case "video":
                    if (Everyplay.FaceCamIsRecordingPermissionGranted())
                    {
                        Everyplay.FaceCamSetAudioOnly(false);
                        Everyplay.FaceCamStartSession();
                    }
                    break;
                default:
                    break;
                }
                Everyplay.StartRecording();
            }
        }
        else if (Application.loadedLevelName == "Start")
        {
            if (Everyplay.IsRecording())
            {
                if (Everyplay.FaceCamIsSessionRunning())
                {
                    Everyplay.SetMetadata("live_commentary", 1);
                    Everyplay.FaceCamStopSession();
                }
                Everyplay.StopRecording();
                Everyplay.ShowSharingModal();
            }
        }
    }

    void OnDamage()
    {
        PlayVoice(damageVoice);
    }

    void Jump()
    {
        int i = Random.Range(0, jumpVoices.Length);
        PlayVoice(jumpVoices[i]);
    }

    void Clear()
    {
        PlayVoice(clearVoice);
    }

    void TimeOver()
    {
        PlayVoice(timeOverVoice);
    }

    void PlayVoice(AudioClip voice)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(voice);
    }
}