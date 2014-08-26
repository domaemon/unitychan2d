using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LiveCommentaryButtons : MonoBehaviour
{
    private const float atlasWidth = 960F;
    private const float atlasHeight = 640F;
    private Texture2D offButtonTexture, audioButtonTexture, videoButtonTexture;
    
    public string liveFaceCamMode = "off";
    
    private List<SubsetButton> subsetButtonsList = new List<SubsetButton>();
    private SubsetButton offButton, audioButton, videoButton;
    
    public class SubsetButton
    {
        private Rect atScreenRect = new Rect(0, 0, 396, 190);
        private static Rect uvTexCoord = new Rect((float) 564 / atlasWidth, 
                                                  (float) (640 - 190) / atlasHeight,
                                                  (float) 396 / atlasWidth,
                                                  (float) 190 / atlasHeight);
        
        private Rect uvTexRect = new Rect (uvTexCoord.x, uvTexCoord.y, 
                                           uvTexCoord.width, uvTexCoord.height);
        
        private Texture2D buttonTexture;
        private Rect clickableRect;
        public string buttonMode;
        
        public SubsetButton(string buttonMode, Texture2D buttonTexture, Rect clickableRect)
        {
            this.buttonMode = buttonMode;
            this.buttonTexture = buttonTexture;
            this.clickableRect = clickableRect;
        }

        public void DrawButton()
        {
            GUI.DrawTextureWithTexCoords (this.atScreenRect,
                                          this.buttonTexture,
                                          uvTexRect, true);
        }
        
		private Vector2 mousePosition = new Vector2(0, 0);

#if UNITY_EDITOR
        public bool IsClickedButton()
        {
			mousePosition.x = Input.mousePosition.x;
			mousePosition.y = Screen.height - Input.mousePosition.y;

            if (this.clickableRect.Contains(mousePosition))
            {
                return true;
            } else {
                return false;
            }
        }
#else
        public bool IsClickedButton(Touch touch)
        {
			mousePosition.x = touch.position.x;
			mousePosition.y = Screen.height - touch.position.y;

            if (this.clickableRect.Contains(mousePosition))
            {
                return true;
            } else {
                return false;
            }
        }
#endif
    }
    
    private void DrawButtons()
    {
        foreach(SubsetButton subsetButton in subsetButtonsList)
        {
            if (liveFaceCamMode.Equals (subsetButton.buttonMode))
            {
                subsetButton.DrawButton();
            }
        }
    }
    
    void OnGUI()
    {
        if(Event.current.type.Equals(EventType.Repaint))
        {
            DrawButtons();
        }
    }

    void Awake()
    {
        offButtonTexture = (Texture2D)Resources.Load("everyplay-live-facecam-off", typeof(Texture2D));
        audioButtonTexture = (Texture2D)Resources.Load("everyplay-live-facecam-audio", typeof(Texture2D));
        videoButtonTexture = (Texture2D)Resources.Load("everyplay-live-facecam-video", typeof(Texture2D));
        
        offButton = new SubsetButton("off", offButtonTexture, new Rect(50, 80, 96, 60));
        audioButton = new SubsetButton("audio", audioButtonTexture, new Rect(146, 80, 96, 60));
        videoButton = new SubsetButton("video", videoButtonTexture, new Rect(242, 80, 96, 60));
        
        PlayerPrefs.SetString("Live FaceCam Mode", liveFaceCamMode);
        
        subsetButtonsList.Add(offButton);
        subsetButtonsList.Add(audioButton);
        subsetButtonsList.Add(videoButton);
    }

    void Update()
    {
#if UNITY_EDITOR
        if(Input.GetMouseButtonDown(0))
        {
            foreach(SubsetButton subsetButton in subsetButtonsList)
            {
                if(subsetButton.IsClickedButton ())
                {
                    liveFaceCamMode = subsetButton.buttonMode;
                    PlayerPrefs.SetString("Live FaceCam Mode", liveFaceCamMode);
                }
            }
        }
#else
        foreach(Touch touch in Input.touches)
        {
            if(touch.phase == TouchPhase.Began)
            {
                foreach(SubsetButton subsetButton in subsetButtonsList)
                {
                    if(subsetButton.IsClickedButton (touch))
                    {
                        liveFaceCamMode = subsetButton.buttonMode;
                        PlayerPrefs.SetString("Live FaceCam Mode", liveFaceCamMode);
                    }
                }
            }
        }
#endif
    }
}
