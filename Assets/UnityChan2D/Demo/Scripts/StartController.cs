using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class StartController : MonoBehaviour
{
    [SceneName]
    public string nextLevel;

    [SerializeField]
    private KeyCode enter = KeyCode.X;
    public Joystick touchJoystick;
    
    private void CheckForRecordingPermission(bool granted)
    {
        if (granted)
        {
            Debug.Log("Microphone access was granted");
        }
        else
        {
            Debug.Log("Microphone access was DENIED");
        }
    }

    void Awake()
    {
        Everyplay.FaceCamRecordingPermission += CheckForRecordingPermission;
        Everyplay.FaceCamRequestRecordingPermission();
        LiveCommentaryButtons liveCommentaryButtons = gameObject.AddComponent<LiveCommentaryButtons>();
    }

    void OnDestroy()
    {
        Everyplay.FaceCamRecordingPermission -= CheckForRecordingPermission;
    }

    void Update()
    {
        if (Input.GetKeyDown(enter))
        {
            StartCoroutine(LoadStage());
        }
        
        if (touchJoystick.IsFingerDown())
        {
            StartCoroutine(LoadStage());
        }
    }

    private IEnumerator LoadStage()
    {
        foreach (AudioSource audioS in FindObjectsOfType<AudioSource>())
        {
            audioS.volume = 0.2f;
        }

        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = 1;

        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length + 0.5f);
        Application.LoadLevel(nextLevel);
    }
}
