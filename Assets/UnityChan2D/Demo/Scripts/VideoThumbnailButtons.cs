using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EveryplayMiniJSON;

public class VideoThumbnailButtons : MonoBehaviour {    
    private Texture2D leaderBoardContainerTexture;
    private Texture2D[] thumbnailTexture = new Texture2D[3];
    private string[] thumbnailUrl = new string[3];
    private Dictionary<string, object>[] featuredVideo = new Dictionary<string, object>[3];
    private bool[] isImageReady = new bool[3];
    private Rect[] thumbnailAtScreen = new Rect[3];
    private GUIStyle[] thumbnailButtonStyle = new GUIStyle[3];
    
    private bool isSearchDone = false;
    private WWW myWww;
    
    private const float atlasWidth = 960F;
    private const float atlasHeight = 640F;
    
    private const int thumbnailWidth = 98;
    private const int thumbnailHeight = 56;
    
    private Rect atScreenRect;
    private Rect uvTexCoord;
    private Rect uvTexRect;
    
    private string searchUrl = "/games/current/videos?client_id=3af827ab042ffaa9655b27e1c271339d597efe32&offset=0&limit=3&order=popularity";
    
    
    void DrawContainer() {
        GUI.DrawTextureWithTexCoords (atScreenRect,
                                      leaderBoardContainerTexture,
                                      uvTexRect, true);
    }
    
    void DrawButtons() {
        for (int i = 0; i < 3; i++) {
            if (GUI.Button (thumbnailAtScreen[i], "", thumbnailButtonStyle[i])) {
                if (isImageReady[i]) {
                    Debug.Log ("DATA is now loaded");
                    Everyplay.PlayVideoWithDictionary(featuredVideo[i]);
                } else {
                    Debug.Log ("DATA is not loaded yet");
                }
            }
        }
    }

    void OnGUI() {
        if(Event.current.type.Equals(EventType.Repaint))
        {
            DrawContainer();
        }
        DrawButtons();
    }
    
 
    void SearchRequest() {
        Everyplay.MakeRequest("get",
                              searchUrl,
                              null, 
                              delegate(string data) 
                              {
                                  List<System.Object> videos = EveryplayMiniJSON.Json.Deserialize(data) as List<System.Object>;
                                  
                                  int i = 0;
                                  foreach (Dictionary<string, object> video in videos) {
                                      if (video.ContainsKey ("thumbnail_url")) {
                                          thumbnailUrl[i] = (string) video["thumbnail_url"];
                                          featuredVideo[i] = video;                                          
                                          
                                          isSearchDone = true;
                                          
                                          if (i > 2) {
                                              break;
                                          }
                                          i++;
                                      }
                                  }
                              }, delegate(string error) {
                                  // act accordingly
                              });
    }
    
    void SetThumbnailButtonStyle() {
        for (int i = 0; i < 3; i++) {
            thumbnailButtonStyle[i] = new GUIStyle();
            thumbnailButtonStyle[i].fixedWidth = 0;
            thumbnailButtonStyle[i].fixedHeight = 0;
            thumbnailButtonStyle[i].stretchWidth = true;
            thumbnailButtonStyle[i].stretchHeight = true;
            thumbnailButtonStyle[i].normal.background = thumbnailTexture[i];
        }
    }
    
    void SetThumbnailAtScreen()	{
        thumbnailAtScreen[0] = new Rect((Screen.width / 2) - (320 / 2) + 170, // offset = 170
                                        (Screen.height / 2) - (360 / 2) + 122 + (70 - thumbnailHeight) / 2, 
                                        thumbnailWidth, 
                                        thumbnailHeight);
		thumbnailAtScreen[1] = new Rect((Screen.width / 2) - (320 / 2) + 170,
		                                (Screen.height / 2) - (360 / 2) + 194 + (70 - thumbnailHeight) / 2,
                                        thumbnailWidth,
                                        thumbnailHeight);
		thumbnailAtScreen[2] = new Rect((Screen.width / 2) - (320 / 2) + 170,
		                                (Screen.height / 2) - (360 / 2) + 264 + (70 - thumbnailHeight) / 2,
                                        thumbnailWidth,
                                        thumbnailHeight);
    }
    
    IEnumerator Start () {
        SearchRequest ();
        yield return new WaitForSeconds(3);
	
        if (isSearchDone) {
            for (int i = 0; i < 3; i++) {
                isImageReady[i] = false;
                myWww = new WWW(thumbnailUrl[i]);
                thumbnailTexture[i] = new Texture2D(4, 4, TextureFormat.DXT1, false);
                yield return myWww;
		
                if (myWww.isDone) {
                    myWww.LoadImageIntoTexture(thumbnailTexture[i]);
                    thumbnailButtonStyle[i].normal.background = thumbnailTexture[i];
                    
                    isImageReady[i] = true;
                } else {
					thumbnailTexture[i] = (Texture2D)Resources.Load("thumbnail-unable-to-load-replay", typeof(Texture2D));
                }
            }
        }
    }

	void SetContainerParams() {
		atScreenRect = new Rect(Screen.width / 2 - 160, Screen.height / 2 - 180, 320, 360);
		uvTexCoord = new Rect((float) 640F / atlasWidth, 
		                      (float) (640 - 360) / atlasHeight,
		                      (float) 320 / atlasWidth,
	                          (float) 360 / atlasHeight);
		
		uvTexRect = new Rect (uvTexCoord.x, uvTexCoord.y, 
	                          uvTexCoord.width, uvTexCoord.height);
	}

	void Awake() {
		leaderBoardContainerTexture = (Texture2D)Resources.Load("everyplay-leader-board-container", typeof(Texture2D));

 
		for (int i = 0; i < 3; i++) {
            thumbnailTexture[i] = (Texture2D)Resources.Load("thumbnail-loading-replay", typeof(Texture2D));
        }

		SetContainerParams();
        SetThumbnailAtScreen();
        SetThumbnailButtonStyle();
    }
    
    // Update is called once per frame
    void Update () {
    }
}
