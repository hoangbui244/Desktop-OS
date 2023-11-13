using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using System.IO;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace com.lockedroom.io.module.pc {
    [RequireComponent(typeof(VideoPlayer))]
    [RequireComponent(typeof(AudioSource))]
    public class VideoPlayerManager : MonoBehaviour {
        // Resources
        public VideoPlayerLibrary libraryAsset;
        [SerializeField] private Transform videoLibraryParent;
        public GameObject user;
        public UserManager userManager;
        public GameObject videoLibraryButton;
        public GameObject videoPlayerWindow;
        public Animator videoControlsAnimator;
        public Animator miniPlayerAnimator;
        [HideInInspector] public VideoPlayer videoComponent;
        [HideInInspector] public VideoPlayerManager videoManager;

        // Settings
        public bool sortListByName = true;
        [Range(1, 15)] public float controlsOutTime = 2.5f;
        [Range(1, 60)] public float seekTime = 10;
        public string videoPanelName = "Now Playing";
        [HideInInspector] public bool modSupport;

        // Hidden variables
        [HideInInspector] public int currentClipIndex;
        [HideInInspector] public int secondsPassed;
        [HideInInspector] public int minutesPassed;
        [HideInInspector] public int totalSeconds;
        [HideInInspector] public int totalMinutes;
        [HideInInspector] public bool loop;

        [HideInInspector] public bool isDone;
        [HideInInspector] public bool miniPlayerEnabled = false;
        [HideInInspector] public WindowPanelManager wManager;
        bool updateInputPos;
        bool controlHelper;
        int indexHelper;
        Vector3 lastMousePos;

        public bool IsPlaying {
            get {
                return videoComponent.isPlaying;
            }
        }
        public bool IsLooping {
            get {
                return videoComponent.isLooping;
            }
        }
        public bool IsPrepared {
            get {
                return videoComponent.isPrepared;
            }
        }
        public bool IsDone {
            get {
                return isDone;
            }
        }
        public double Time {
            get {
                return videoComponent.time;
            }
        }
        public ulong Duration {
            get {
                return (ulong)(videoComponent.frameCount / videoComponent.frameRate);
            }
        }
        public double NTime {
            get {
                return Time / Duration;
            }
        }

        void Awake() {
            userManager = user.GetComponentInChildren<UserManager>();
            videoComponent = gameObject.GetComponent<VideoPlayer>();
            videoManager = gameObject.GetComponent<VideoPlayerManager>();
            wManager = videoPlayerWindow.GetComponent<WindowPanelManager>();
            if (modSupport == true)
                return;

            //InitializeVideoPlayer();
        }

        private static int SortByName(VideoData o1, VideoData o2) {
            return o1.videoTitle.CompareTo(o2.videoTitle);
        }

        public void InitializeVideoPlayer() {
            PrepareVideos();
            if (miniPlayerAnimator != null) { 
                miniPlayerAnimator.gameObject.SetActive(false); 
            }
        }

        void PrepareVideos() {
            videoComponent.SetTargetAudioSource(0, gameObject.GetComponent<AudioSource>());

            if (sortListByName == true) { userManager.videoAlbum.Sort(SortByName); }
            foreach (Transform child in videoLibraryParent) { Destroy(child.gameObject); }
            for (int i = 0; i < userManager.videoAlbum.Count; ++i) {
                // Checking for mods

                //if (libraryAsset.videos[i].isModContent == true && libraryAsset.videos[i].modHelper == true)
                //{
                //    libraryAsset.videos.RemoveAt(i);
                //    i--; continue;
                //}

                //if (libraryAsset.videos[i].excludeFromLibrary == false)
                //{
                // Spawn standard notification to the requested parent
                GameObject go = Instantiate(videoLibraryButton, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(videoLibraryParent, false);
                go.gameObject.name = userManager.videoAlbum[i].videoTitle;

                // Set cover image
                Transform coverGO = go.transform.Find("Cover Mask/Cover/Image").GetComponent<Transform>();

                // Load the texture from the file path
                string thumbnailPath = userManager.videoAlbum[i].thumbnailPath;

                // Create a Sprite using the loaded texture
                Texture2D texture = LoadTextureFromFile(thumbnailPath);

                if (texture != null) {
                    // Create a Sprite using the loaded texture
                    Sprite loadedSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                    coverGO.GetComponent<Image>().sprite = loadedSprite;
                }

                // Video from file path
                string videoPath = Application.dataPath + "/" + userManager.videoAlbum[i].videoPath;

                // Set ID tags
                TextMeshProUGUI videoTitle = go.transform.Find("Video Title").GetComponent<TextMeshProUGUI>();
                videoTitle.text = userManager.videoAlbum[i].videoTitle;
                TextMeshProUGUI descText = go.transform.Find("Video Description").GetComponent<TextMeshProUGUI>();
                descText.text = userManager.videoAlbum[i].videoDescription;
                TextMeshProUGUI durationText = go.transform.Find("Duration Area/Duration").GetComponent<TextMeshProUGUI>();

                //if (userManager.videoAlbum[i].playFromURL == false)
                //    durationText.text = (((int)libraryAsset.videos[i].videoClip.length / 60) % 60) + ":" + ((int)libraryAsset.videos[i].videoClip.length % 60).ToString("D2");
                //else
                //    durationText.text = "URL";

                // Add button events
                Button itemButton = go.GetComponent<Button>();
                itemButton.onClick.AddListener(delegate {
                    if (userManager.videoAlbum[go.transform.GetSiblingIndex()].playFromURL == false) {
                        videoComponent.source = VideoSource.VideoClip;
                        videoComponent.Stop();
                        //currentClipIndex = go.transform.GetSiblingIndex();
                        videoComponent.url = videoPath;
                        videoComponent.time = 0;
                        videoComponent.Play();
                    }

                    else {
                        videoComponent.source = VideoSource.Url;
                        indexHelper = go.transform.GetSiblingIndex();
                        PlayVideoURL(userManager.videoAlbum[go.transform.GetSiblingIndex()].videoURL);
                    }

                    DisableMiniPlayer();
                    wManager.OpenPanel(videoPanelName);
                });
                //}

                //else if (libraryAsset.videos[i].excludeFromLibrary == true)
                //{
                //    libraryAsset.videos.RemoveAt(i);
                //    i--; continue;
                //}
            }

            videoComponent.Play();
            videoComponent.Pause();
        }

        private Texture2D LoadTextureFromFile(string filePath) {
            Texture2D texture = null;
            byte[] fileData;

            if (File.Exists(filePath)) {
                fileData = File.ReadAllBytes(filePath);
                texture = new Texture2D(2, 2);
                texture.LoadImage(fileData);
            }

            return texture;
        }

        void Update() {
            if (videoManager.IsPrepared) {
                totalMinutes = (int)videoManager.Duration / 60;
                totalSeconds = (int)videoManager.Duration - totalMinutes * 60;
                minutesPassed = (int)videoManager.Time / 60;
                secondsPassed = (int)videoManager.Time - minutesPassed * 60;
            }

#if ENABLE_LEGACY_INPUT_MANAGER
            if (updateInputPos == true) {
                if (lastMousePos == Input.mousePosition
                    && videoControlsAnimator.GetCurrentAnimatorStateInfo(0).IsName("In")) {
                    StartCoroutine("ControlsFadeOutStart");
                }

                else if (lastMousePos != Input.mousePosition
                    && !videoControlsAnimator.GetCurrentAnimatorStateInfo(0).IsName("In")
                    && controlHelper == true) {
                    StopCoroutine("ControlsFadeOutStart");
                    Cursor.visible = true;
                    videoControlsAnimator.CrossFade("In", 0.2f);
                    controlHelper = false;
                }

                else if (lastMousePos != Input.mousePosition)
                    StopCoroutine("ControlsFadeOutStart");

                lastMousePos = Input.mousePosition;
            }
#elif ENABLE_INPUT_SYSTEM
            if (updateInputPos == true)
            {
                if (lastMousePos == new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0)
                    && videoControlsAnimator.GetCurrentAnimatorStateInfo(0).IsName("In"))
                {
                    StartCoroutine("ControlsFadeOutStart");
                }

                else if (lastMousePos != new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0)
                    && !videoControlsAnimator.GetCurrentAnimatorStateInfo(0).IsName("In")
                    && controlHelper == true)
                {
                    StopCoroutine("ControlsFadeOutStart");
                    Cursor.visible = true;
                    videoControlsAnimator.CrossFade("In", 0.2f);
                    controlHelper = false;
                }

                else if (lastMousePos != new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0))
                    StopCoroutine("ControlsFadeOutStart");

                lastMousePos = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0);
            }
#endif
        }

        public void PlayCustomVideo(int videoIndex) {
            videoComponent.Stop();
            currentClipIndex = videoIndex;
            videoComponent.clip = libraryAsset.videos[videoIndex].videoClip;
            videoComponent.time = 0;
            videoComponent.Play();
        }

        public void PlayVideoURL(string url) {
            videoComponent.Stop();
            currentClipIndex = indexHelper;
            videoComponent.url = userManager.videoAlbum[indexHelper].videoURL;
            videoComponent.time = 0;
            videoComponent.Play();
        }

        public void PlayVideoClip(VideoClip video, string title) {
            //VideoPlayerLibrary.VideoItem item = new VideoPlayerLibrary.VideoItem();
            //item.videoClip = video;
            //item.videoTitle = title;
            //item.excludeFromLibrary = true;
            //libraryAsset.videos.Add(item);
            //videoComponent.source = VideoSource.VideoClip;

            //for (int i = 0; i < libraryAsset.videos.Count; ++i)
            //{
            //    if (libraryAsset.videos[i].videoTitle == title)
            //    {
            //        currentClipIndex = i;
            //        break;
            //    }
            //}

            //PlayCustomVideo(currentClipIndex);
        }

        public void Play() {
            videoComponent.Play();
        }
        public void Pause() {
            videoComponent.Pause(); StopCoroutine("ControlsFadeOutStart");
        }
        public void SeekForward() {
            videoComponent.time += seekTime;
        }
        public void SeekBackward() {
            videoComponent.time -= seekTime;
        }

        public void IncreasePlaybackSpeed() {
            if (!videoComponent.canSetPlaybackSpeed)
                return;

            videoComponent.playbackSpeed += 1;
            videoComponent.playbackSpeed = Mathf.Clamp(videoComponent.playbackSpeed, 0, 10);
        }

        public void DecreasePlaybackSpeed() {
            if (!videoComponent.canSetPlaybackSpeed)
                return;

            videoComponent.playbackSpeed -= 1;
            videoComponent.playbackSpeed = Mathf.Clamp(videoComponent.playbackSpeed, 0, 10);
        }

        public void DisableControls() {
            StopCoroutine("ControlsFadeOutStart");
            updateInputPos = false;
            videoControlsAnimator.CrossFade("Out", 0.2f);
            Cursor.visible = true;
        }

        public void EnableInputUpdating() {
            updateInputPos = true;
        }
        public void DisableInputUpdating() {
            updateInputPos = false; Cursor.visible = true;
        }
        public void ControlOutOfBounds() {
            Cursor.visible = true;
        }

        public void EnableMiniPlayer() {
            if (miniPlayerAnimator != null && videoComponent.isPlaying == true) {
                StopCoroutine("MiniPlayerOut");
                miniPlayerEnabled = true;
                miniPlayerAnimator.gameObject.SetActive(true);
                miniPlayerAnimator.Play("In");
            }
        }

        public void DisableMiniPlayer() {
            if (miniPlayerAnimator != null && miniPlayerAnimator.gameObject.activeSelf == true) {
                miniPlayerAnimator.Play("Out");
                miniPlayerEnabled = false;
                StartCoroutine("MiniPlayerOut");
            }
        }

        IEnumerator ControlsFadeOutStart() {
            yield return new WaitForSeconds(controlsOutTime);
            Cursor.visible = false;
            videoControlsAnimator.CrossFade("Out", 0.2f);
            controlHelper = true;
            StopCoroutine("ControlsFadeOutStart");
        }

        IEnumerator MiniPlayerOut() {
            yield return new WaitForSeconds(0.5f);
            miniPlayerAnimator.gameObject.SetActive(false);
        }
    }
}