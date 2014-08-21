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
	public Texture2D shareReplayTexture;
    
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

    void OnGUI()
    {
        LiveCommentaryButtons liveCommentaryButtons = gameObject.AddComponent<LiveCommentaryButtons>();
        if (!liveCommentaryButtons)
        {
            Debug.Log ("Live Commentary Buttons could not be created.");
        }
        
        if (GUI.Button(new Rect(Screen.width - 168, 32, 136, 136), shareReplayTexture, GUIStyle.none))
        {
            Everyplay.ShowWithPath("/feed/game");
        }
    }

    void Awake()
    {
        shareReplayTexture = (Texture2D)Resources.Load("watch-replay-button-example", typeof(Texture2D));
        Everyplay.FaceCamRecordingPermission += CheckForRecordingPermission;
        Everyplay.FaceCamRequestRecordingPermission();
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
