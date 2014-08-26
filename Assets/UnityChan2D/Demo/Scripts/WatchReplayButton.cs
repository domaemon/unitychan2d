using UnityEngine;
using System.Collections;

public class WatchReplayButton : MonoBehaviour {
    private Texture2D watchReplayTexture;
    private const int watchImageWidth = 136;
    private const int watchImageHeight = 136;
    
    private Rect watchReplayAtScreen = new Rect (Screen.width - (watchImageWidth + 32), 
                                                 32, 
                                                 watchImageWidth, 
                                                 watchImageHeight);
    
    void OnGUI()
    {
        //GUI.skin.GetStyle("toggle style one");
        if (GUI.Button(watchReplayAtScreen, watchReplayTexture, GUIStyle.none))
        {
            Everyplay.ShowWithPath("/feed/game");
        }
    }
    
    // Use this for initialization
    void Start () {
        watchReplayTexture = (Texture2D)Resources.Load("watch-replay-button-example", typeof(Texture2D));
    }
    
    // Update is called once per frame
    void Update () {
    }
}
