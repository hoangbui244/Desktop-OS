using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Linq;
using UnityEngine.Tilemaps;
using System;

namespace com.lockedroom.io.module.pc {

    // Music album
    [System.Serializable]
    public class AudioData {
        public string musicTitle;
        public string artistTitle;
        public string imgPath;
        public string musicPath;
    }

    // Video album
    [System.Serializable]
    public class VideoData {
        public string videoTitle;
        public string videoDescription;
        public string thumbnailPath;
        public string videoPath;
        public bool playFromURL;
        public string videoURL;
    }

    // Photo album
    [System.Serializable]
    public class PhotoData {
        public string photoTitle;
        public string photoDescription;
        public string photoPath;
    }
    [System.Serializable]
    public class ChatImageData {
        public string chatImageTitle;
        public string chatImageDescription;
        public string chatImagePath;
    }

    // UserData
    [System.Serializable]
    public class UserData {
        public string firstName;
        public string lastName;
        public string password;
        public string secQuestion;
        public string secAnswer;
        public int ppIndex;
        public int userCreated;
        public string notepadDataPath;
        public string messDataPath;
        public string photoDataPath;
        public string videoDataPath;
        public string audioDataPath;
        public string chatImagePath;
        public int wallpaperIndex;
        public List<PhotoData> photoAlbum;
        public List<VideoData> videoAlbum;
        public List<AudioData> audioAlbum;
        public List<ChatImageData> chatImageData;

    }

    public class UserManager : MonoBehaviour
    {
        // Resources
        public BootManager bootManager;
        public Animator setupScreen;
        public Animator lockScreen;
        public Animator desktopScreen;
        public TMP_InputField lockScreenPassword;
        public BlurManager lockScreenBlur;
        public ProfilePictureLibrary ppLibrary;
        public GameObject ppItem;
        public Transform ppParent;
        public JsonManager JsonManager;

        // Path
        public string notepadDataPath;
        public string messDataPath;   
        public string photoDataPath;
        public string videoDataPath;
        public string audioDataPath;
        public string chatImageDataPath;
        [HideInInspector] public string notepadDataFullPath;
        [HideInInspector] public string messDataFullPath;
        [HideInInspector] public string photoDataFullPath;
        [HideInInspector] public string videoDataFullPath;
        [HideInInspector] public string audioDataFullPath;
        [HideInInspector] public string chatImageFullPath;

        // Content
        [Range(1, 20)] public int minNameCharacter = 1;
        [Range(1, 20)] public int maxNameCharacter = 14;

        [Range(1, 20)] public int minPasswordCharacter = 4;
        [Range(1, 20)] public int maxPasswordCharacter = 16;

        // Events
        public UnityEvent onLogin;
        public UnityEvent onLock;
        public UnityEvent onWrongPassword;

        [HideInInspector] public string systemUsername = "Admin";
        [HideInInspector] public string systemLastname = "";
        [HideInInspector] public string systemPassword = "1234";
        [HideInInspector] public string systemSecurityQuestion = "Answer: DreamOS";
        [HideInInspector] public string systemSecurityAnswer = "DreamOS";

        // Settings
        public bool disableUserCreating = false;
        public bool saveProfilePicture = true;
        public bool deletePrefsAtStart = false;
        public int ppIndex;

        // Multi Instance Support
        public bool allowMultiInstance;
        public string machineID = "DreamOS";

        // User variables
        public string firstName;
        public string lastName;
        [HideInInspector] public string password;
        [HideInInspector] public string secQuestion;
        [HideInInspector] public string secAnswer;
        [HideInInspector] public Sprite profilePicture;

        [HideInInspector] public bool hasPassword;
        [HideInInspector] public bool nameOK;
        [HideInInspector] public bool lastNameOK;
        [HideInInspector] public bool passwordOK;
        [HideInInspector] public bool passwordRetypeOK;
        [HideInInspector] public int userCreated;
        [HideInInspector] public int wallpaperIndex;

        [HideInInspector] public bool isLockScreenOpen = false;

        [HideInInspector] public List<GetUserInfo> guiList = new List<GetUserInfo>();
        [HideInInspector] public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPEG", ".JPE", ".BMP", ".GIF", ".PNG" };
        [HideInInspector] public static readonly List<string> VideoExtensions = new List<string> { ".MP4", ".AVI", ".MKV", ".WMV" };
        [HideInInspector] public static readonly List<string> AudioExtensions = new List<string> { ".MP3", ".WAV", ".FLAC", ".AAC" };
        public List<PhotoData> photoAlbum = new List<PhotoData>();
        public List<VideoData> videoAlbum = new List<VideoData>();
        public List<AudioData> audioAlbum = new List<AudioData>();
        public List<ChatImageData> chatImageAlbum = new List<ChatImageData>();

        //Desktop scene
        //public void Awake() {
        //    InitializeUserManager();
        //    InitializeProfilePictures();
        //    InitializeSavePath();
        //}

        // Multi instances scene
        public void Initialize() {
            InitializeUserManager();
            InitializeProfilePictures();
            InitializeSavePath();
        }

        public void InitializeSavePath() {
            // Desktop scene
            //string subpath = "Data/User";

            // Multi instances scene
            string mainPath = JsonManager.mainPath;
            string firstPath = "Instance" + JsonManager.index.ToString();
            string secondPath = JsonManager.secondPath;
            string noteFileName = "Notepad";
            string messFileName = "Message";
            string photoFolderName = "Photo";
            string videoFolderName = "Video";
            string audioFolderName = "Audio";
            string chatImageFolderName = "ChatImage";
            string fileExtension = ".data";
#if UNITY_EDITOR
            // Desktop scene
            //notepadDataPath = Application.dataPath + "/" + subpath + "/" + noteFileName + fileExtension;
            //messDataPath = Application.dataPath + "/" + subpath + "/" + messFileName + fileExtension;
            //photoDataPath = Application.dataPath + "/" + subpath + "/" + photoFileName;

            // Multi instances scene
            notepadDataPath = mainPath + "/" + firstPath + "/" + secondPath + "/" + noteFileName + fileExtension;
            notepadDataFullPath = Application.dataPath + "/" + notepadDataPath;
            messDataPath = mainPath + "/" + firstPath + "/" + secondPath + "/" + messFileName + fileExtension;
            messDataFullPath = Application.dataPath + "/" + messDataPath;
            photoDataPath = mainPath + "/" + firstPath + "/" + secondPath + "/" + photoFolderName;
            photoDataFullPath = Application.dataPath + "/" + photoDataPath;
            videoDataPath = mainPath + "/" + firstPath + "/" + secondPath + "/" + videoFolderName;
            videoDataFullPath = Application.dataPath + "/" + videoDataPath;
            audioDataPath = mainPath + "/" + firstPath + "/" + secondPath + "/" + audioFolderName;
            audioDataFullPath = Application.dataPath + "/" + audioDataPath;
            chatImageDataPath = mainPath + "/" + firstPath + "/" + secondPath + "/" + chatImageFolderName;
            chatImageFullPath = Application.dataPath + "/" + chatImageDataPath;

#else
            string appPath = Application.persistentDataPath;
            notepadDataPath = Path.Combine(appPath, mainPath, firstPath, secondPath, noteFileName + fileExtension);
            messDataPath = Path.Combine(appPath, mainPath, firstPath, secondPath, messFileName + fileExtension);
            photoDataPath = Path.Combine(appPath, mainPath, firstPath, secondPath, photoFolderName);
            videoDataPath = Path.Combine(appPath, mainPath, firstPath, secondPath, videoFolderName);
#endif
            if (!File.Exists(notepadDataFullPath)) {
                FileInfo dataFile = new FileInfo(notepadDataFullPath);
                dataFile.Directory.Create();
            }
            if (!File.Exists(messDataFullPath)) {
                FileInfo dataFile = new FileInfo(messDataFullPath);
                dataFile.Directory.Create();
            }
            if (!File.Exists(photoDataFullPath)) {
                Directory.CreateDirectory(photoDataFullPath);
            }
            if (Directory.Exists(photoDataFullPath)) {
                var files = from file in Directory.EnumerateFiles(photoDataFullPath) select file;
                foreach (var file in files) {
                    FileInfo fileInfo = new FileInfo(file);
                    if (ImageExtensions.Contains(fileInfo.Extension.ToUpperInvariant())) {
                        // TODO: check if image
                        PhotoData newPhoto = new PhotoData();
                        newPhoto.photoTitle = Path.GetFileNameWithoutExtension(fileInfo.Name);
                        newPhoto.photoDescription = "";
                        string fullPath1 = Path.GetFullPath(Application.dataPath);
                        string fullPath2 = Path.GetFullPath(fileInfo.FullName);
                        if (fullPath2.StartsWith(fullPath1, StringComparison.CurrentCultureIgnoreCase)) {
                            string result = fullPath2.Substring(fullPath1.Length).TrimStart(Path.DirectorySeparatorChar);
                            newPhoto.photoPath = result;
                        }
                        var matchedList = photoAlbum.Where(item => item.photoTitle == newPhoto.photoTitle && item.photoPath == newPhoto.photoPath).ToList();
                        if (matchedList.Count == 0) {
                            photoAlbum.Add(newPhoto);
                        }
                    }
                }
                JsonManager.SaveUserData();
            }

            if (!File.Exists(chatImageFullPath)) {
                Directory.CreateDirectory(chatImageFullPath);
            }
            if (Directory.Exists(chatImageFullPath)) {
                var files = from file in Directory.EnumerateFiles(chatImageFullPath) select file;
                foreach (var file in files) {
                    FileInfo fileInfo = new FileInfo(file);
                    if (ImageExtensions.Contains(fileInfo.Extension.ToUpperInvariant())) {
                        // TODO: check if image
                        ChatImageData newChatImage = new ChatImageData();
                        newChatImage.chatImageTitle = Path.GetFileNameWithoutExtension(fileInfo.Name);
                        newChatImage.chatImageDescription = "";
                        string fullPath1 = Path.GetFullPath(Application.dataPath);
                        string fullPath2 = Path.GetFullPath(fileInfo.FullName);
                        if (fullPath2.StartsWith(fullPath1, StringComparison.CurrentCultureIgnoreCase)) {
                            string result = fullPath2.Substring(fullPath1.Length).TrimStart(Path.DirectorySeparatorChar);
                            newChatImage.chatImagePath = result;
                        }
                        var matchedList = photoAlbum.Where(item => item.photoTitle == newChatImage.chatImageTitle && item.photoPath == newChatImage.chatImagePath).ToList();
                        if (matchedList.Count == 0) {
                            chatImageAlbum.Add(newChatImage);
                        }
                    }
                }
                JsonManager.SaveUserData();
            }


            if (!File.Exists(videoDataFullPath)) {
                Directory.CreateDirectory(videoDataFullPath);
            }
            if (Directory.Exists(videoDataFullPath)) {
                var files = from file in Directory.EnumerateFiles(videoDataFullPath) select file;
                foreach (var file in files) {
                    FileInfo fileInfo = new FileInfo(file);
                    if (VideoExtensions.Contains(fileInfo.Extension.ToUpperInvariant())) {
                        VideoData newVideo = new VideoData();
                        // TODO: check if video
                        newVideo.videoTitle = Path.GetFileNameWithoutExtension(fileInfo.Name);
                        newVideo.videoDescription = "";
                        string fullPath1 = Path.GetFullPath(Application.dataPath);
                        string fullPath2 = Path.GetFullPath(fileInfo.FullName);
                        if (fullPath2.StartsWith(fullPath1, StringComparison.CurrentCultureIgnoreCase)) {
                            string result = fullPath2.Substring(fullPath1.Length).TrimStart(Path.DirectorySeparatorChar);
                            newVideo.videoPath = result;
                        }
                        newVideo.thumbnailPath = videoDataFullPath + "/" + "Rolls Thumbnail.png";
                        newVideo.playFromURL = false;
                        newVideo.videoURL = "";
                        var matchedList = videoAlbum.Where(item => item.videoTitle == newVideo.videoTitle && item.videoPath == newVideo.videoPath).ToList();
                        if (matchedList.Count == 0) {
                            videoAlbum.Add(newVideo);
                        }
                    }
                }
                JsonManager.SaveUserData();
            }
            if (!File.Exists(audioDataFullPath)) {
                Directory.CreateDirectory(audioDataFullPath);
            }
            if (Directory.Exists(audioDataFullPath)) {
                var files = from file in Directory.EnumerateFiles(audioDataFullPath) select file;
                foreach (var file in files) {
                    FileInfo fileInfo = new FileInfo(file);
                    if (AudioExtensions.Contains(fileInfo.Extension.ToUpperInvariant())) {
                        AudioData newAudio = new AudioData();
                        // TODO: check if audio
                        newAudio.musicTitle = Path.GetFileNameWithoutExtension(fileInfo.Name);
                        newAudio.artistTitle = "";
                        string fullPath1 = Path.GetFullPath(Application.dataPath);
                        string fullPath2 = Path.GetFullPath(fileInfo.FullName);
                        if (fullPath2.StartsWith(fullPath1, StringComparison.CurrentCultureIgnoreCase)) {
                            string result = fullPath2.Substring(fullPath1.Length).TrimStart(Path.DirectorySeparatorChar);
                            newAudio.musicPath = result;
                        }
                        newAudio.imgPath = audioDataFullPath + "/" + "Example Music.png";
                        var matchedList = audioAlbum.Where(item => item.musicTitle == newAudio.musicTitle && item.musicPath == newAudio.musicPath).ToList();
                        if (matchedList.Count == 0) {
                            audioAlbum.Add(newAudio);
                        }
                    }
                }
                JsonManager.SaveUserData();
            }
        }

        public void InitializeUserManager()
        {
            // Find Boot manager in the scene
            if (bootManager == null)
                bootManager = (BootManager)GameObject.FindObjectsOfType(typeof(BootManager))[0];

            if (disableUserCreating == false)
            {
                userCreated = 0;
                JsonManager.CheckUserData();
                profilePicture = ppLibrary.pictures[ppIndex].pictureSprite;

                // If password is null, change boolean
                if (password == "") { hasPassword = false; }
                else { hasPassword = true; }

                // If user is not created, show Setup screen
                if (userCreated == 0)
                {
                    bootManager.enabled = false;
                    bootManager.bootAnimator.gameObject.SetActive(false);
                    setupScreen.gameObject.SetActive(true);
                    setupScreen.Play("Panel In");
                }
                else { BootSystem(); }
            }

            else
            {
                // If password is null, change boolean
                if (systemPassword == "") { hasPassword = false; }
                else { hasPassword = true; }

                // Setting up the user details
                firstName = systemUsername;
                lastName = systemLastname;
                password = systemPassword;
                profilePicture = ppLibrary.pictures[ppIndex].pictureSprite;

                BootSystem();
            }
        }

        public UserData GetUserData() {
            UserData userData = new UserData {
                firstName = this.firstName,
                lastName = this.lastName,
                password = this.password,
                secQuestion = this.secQuestion,
                secAnswer = this.secAnswer,
                ppIndex = this.ppIndex,
                userCreated = this.userCreated,
                notepadDataPath = this.notepadDataPath,
                messDataPath = this.messDataPath,
                photoDataPath = this.photoDataPath,
                videoDataPath = this.videoDataPath,
                audioDataPath = this.audioDataPath,
                wallpaperIndex = this.wallpaperIndex,
                photoAlbum = this.photoAlbum,
                videoAlbum = this.videoAlbum,
                audioAlbum = this.audioAlbum,
            };

            return userData;
        }

        public void InitializeProfilePictures()
        {
            if (ppParent == null || ppItem == null)
                return;

            foreach (Transform child in ppParent) { Destroy(child.gameObject); }
            for (int i = 0; i < ppLibrary.pictures.Count; ++i)
            {
                GameObject go = Instantiate(ppItem, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(ppParent, false);
                go.name = ppLibrary.pictures[i].pictureID;

                Image prevImage = go.transform.Find("Image Mask/Image").GetComponent<Image>();
                prevImage.sprite = ppLibrary.pictures[i].pictureSprite;

                Button wpButton = go.GetComponent<Button>();
                wpButton.onClick.AddListener(delegate 
                { 
                    ChangeProfilePicture(go.transform.GetSiblingIndex()); 
                    UpdateUserInfoUI();

                    try { wpButton.gameObject.GetComponentInParent<ModalWindowManager>().CloseWindow(); }
                    catch { Debug.Log("Cannot close the window due to missing modal window manager."); }
                });
            }

            GetAllUserInfoComps();
            UpdateUserInfoUI();
        }

        public void AddPhotoToAlbum(string title, string description, string fileName) {
            PhotoData newPhoto = new PhotoData {
                photoTitle = title,
                photoDescription = description,
                photoPath = photoDataPath + "/" + fileName
            };
            photoAlbum.Add(newPhoto);
            JsonManager.SaveUserData();
        }

        public void RemovePhotoFromAlbum(int photoIndex) {
            if (photoIndex >= 0 && photoIndex < photoAlbum.Count) {
                photoAlbum.RemoveAt(photoIndex);
                JsonManager.SaveUserData();
            }
        }

        public void ChangeFirstName(string textVar)
        {
            firstName = textVar;
            if (disableUserCreating == false) { 
                JsonManager.SaveUserData();
            }
        }

        public void ChangeFirstNameTMP(TMP_InputField tmpVar)
        {
            firstName = tmpVar.text;
            if (disableUserCreating == false) {
                JsonManager.SaveUserData();
            }
        }

        public void ChangeLastName(string textVar)
        {
            lastName = textVar;
            if (disableUserCreating == false) {
                JsonManager.SaveUserData();
            }
        }

        public void ChangeLastNameTMP(TMP_InputField tmpVar)
        {
            lastName = tmpVar.text;
            if (disableUserCreating == false) {
                JsonManager.SaveUserData();
            }
        }

        public void ChangePassword(string textVar)
        {
            password = textVar;
            if (disableUserCreating == false) {
                Debug.Log(password);
                JsonManager.SaveUserData();
            }
        }

        public void ChangePasswordTMP(TMP_InputField tmpVar)
        {
            password = tmpVar.text;
            if (disableUserCreating == false) {
                JsonManager.SaveUserData();
            }
        }

        public void ChangeSecurityQuestion(string textVar) {
            JsonManager.SaveUserData();
        }
        public void ChangeSecurityQuestionTMP(TMP_InputField tmpVar) {
            JsonManager.SaveUserData();
        }
        public void ChangeSecurityAnswer(string textVar) {
            JsonManager.SaveUserData();
        }
        public void ChangeSecurityAnswerTMP(TMP_InputField tmpVar) {
            JsonManager.SaveUserData();
        }

        public void ChangeProfilePicture(int pictureIndex)
        {
            ppIndex = pictureIndex;
            profilePicture = ppLibrary.pictures[ppIndex].pictureSprite;
            if (saveProfilePicture == true) {
                JsonManager.SaveUserData();
            }
        }

        public void UpdateUserInfoUI()
        {
            for (int i = 0; i < guiList.Count; ++i)
                guiList[i].GetInformation();
        }

        public void GetAllUserInfoComps()
        {
            guiList.Clear();
            GetUserInfo[] list = FindObjectsOfType(typeof(GetUserInfo)) as GetUserInfo[];
            foreach (GetUserInfo obj in list) { guiList.Add(obj); }
        }

        public void CreateUser()
        {
            userCreated = 1;
            JsonManager.SaveUserData();

            if (password == "") { hasPassword = false; }
            else { hasPassword = true; }
         
            UpdateUserInfoUI();
        }

        public void BootSystem()
        {
            bootManager.enabled = true;
            bootManager.bootAnimator.gameObject.SetActive(true);
            setupScreen.gameObject.SetActive(false);
            bootManager.bootAnimator.Play("Boot Start");
        }

        public void StartOS()
        {
            if (hasPassword == true)
            {
                lockScreenPassword.gameObject.SetActive(false);
                lockScreen.Play("Skip Login");
            }

            else
            {
                lockScreenPassword.gameObject.SetActive(true);
                lockScreen.Play("Lock Screen In");
            }
        }

        public void LockOS()
        {
            if (lockScreenBlur != null) { lockScreenBlur.BlurOutAnim(); }
            lockScreen.gameObject.SetActive(true);
            lockScreen.Play("Lock Screen In");
            desktopScreen.Play("Desktop Out");
            onLock.Invoke();
        }

        public void LockScreenOpenClose()
        {
            if (isLockScreenOpen == true)
            {
                if (lockScreenBlur != null) { lockScreenBlur.BlurOutAnim(); }
                lockScreen.Play("Lock Screen Out");
            }
            else { lockScreen.Play("Lock Screen In"); }
        }

        public void LockScreenAnimate()
        {
            if (hasPassword == true)
            {
                if (lockScreenBlur != null) { lockScreenBlur.BlurInAnim(); }
                lockScreen.Play("Lock Screen Password In");
            }

            else
            {
                if (lockScreenBlur != null) { lockScreenBlur.BlurOutAnim(); }
                lockScreen.Play("Lock Screen Out");
                desktopScreen.Play("Desktop In");
                onLogin.Invoke();
            }
        }

        public void Login()
        {
            if (lockScreenPassword.text == password)
            {
                lockScreen.Play("Lock Screen Password Out");
                desktopScreen.Play("Desktop In");
                onLogin.Invoke();
                StartCoroutine("DisableLockScreenHelper");
            }
            else if (lockScreenPassword.text != password) { onWrongPassword.Invoke(); }
        }

        IEnumerator DisableLockScreenHelper()
        {
            yield return new WaitForSeconds(1f);
            lockScreen.gameObject.SetActive(false);
        }
    }
}