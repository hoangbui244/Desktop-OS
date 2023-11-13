﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace com.lockedroom.io.module.pc {
    public class MultiInstanceManager : MonoBehaviour {
        // Resources
        public Camera playerCamera;

        // Settings
        public bool manageProjectors = true;

        // Number of instance
        public int instanceNum;

        // instancePrefab
        public GameObject instancePrefab;

        // SpawnPos
        public Transform[] spawnPos;

        // Instance List
        public List<InstanceItem> instances = new List<InstanceItem>();

        [System.Serializable]
        public class InstanceItem {
            // Core Resoucres
            public WorldSpaceManager worldSpaceManager;
            public Canvas instanceCanvas;

            // External Resources
            public UserManager userManager;
            public SettingsManager settingsManager;
            public ReminderManager reminderManager;
            public NetworkManager networkManager;
            public WallpaperManager wallpaperManager;
            public WidgetManager widgetManager;
            public WebBrowserManager webBrowserManager;
            public List<MusicDataDisplay> musicDataItems = new List<MusicDataDisplay>();
            public List<SwitchManager> switches = new List<SwitchManager>();
            public List<GetUserInfo> userInfoItems = new List<GetUserInfo>();
            public List<VideoDataDisplay> videoDataItems = new List<VideoDataDisplay>();

            public RenderTexture instanceRT;

            // Editor Only
#if UNITY_EDITOR
            [HideInInspector] public bool isExpanded;
#endif
        }

        void Awake() {
            CreateInstances();
            for (int i = 0; i < instances.Count; i++) {
                AutoWizard(i);
                if (manageProjectors == true) {
                    instances[i].worldSpaceManager.onExit.AddListener(delegate {
                        for (int x = 0; x < instances.Count; x++) {
                            instances[x].worldSpaceManager.projectorCam.enabled = true;
                        }
                    });

                    instances[i].worldSpaceManager.onEnterEnd.AddListener(delegate {
                        for (int x = 0; x < instances.Count; x++) {
                            instances[x].worldSpaceManager.projectorCam.enabled = false;
                        }
                    });

                    instances[i].instanceCanvas.GetComponentInChildren<EventSystem>().gameObject.SetActive(false);
                }
            }

            EventSystem baseES = new GameObject().AddComponent<EventSystem>();
            baseES.gameObject.name = "[DreamOS Event System]";
            baseES.gameObject.AddComponent<InputSystemChecker>();
        }

        void CreateInstances() {
            for (int i = 0; i < instanceNum; i++) {
                GameObject newInstance;
                if (i < spawnPos.Length) {
                    newInstance = Instantiate(instancePrefab, spawnPos[i].position, transform.rotation);
                }
                else {
                    newInstance = Instantiate(instancePrefab, transform.position, transform.rotation);
                }
                InstanceItem newItem = new InstanceItem();
                newItem.worldSpaceManager = newInstance.GetComponentInChildren<WorldSpaceManager>();
                newItem.instanceCanvas = newInstance.GetComponentInChildren<Canvas>();
                instances.Add(newItem);
            }
        }

        public void AutoWizard(int instanceIndex) {
            // We need to find the resources to initialize the instance
            AutoFindResources(instanceIndex);

            // Destroy all speech recognition comps as it's meant for single-instance purposes
#if UNITY_EDITOR
            SpeechRecognition[] srComps = GameObject.FindObjectsOfType<SpeechRecognition>();

            foreach (SpeechRecognition tempComp in srComps) {
                tempComp.gameObject.SetActive(false);
                DestroyImmediate(tempComp);
            }

            Undo.RecordObject(this, "Destroyed DreamOS speech recognition");
            //UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
        }

        void AutoFindResources(int index) {
            // User data
            instances[index].userManager = instances[index].instanceCanvas.GetComponentInChildren<UserManager>();
            instances[index].userManager.JsonManager.index = index;
            instances[index].userManager.JsonManager.CheckUserData();
            instances[index].userManager.Initialize();
            SetMachineID(index);

            // Settings data
            instances[index].settingsManager = instances[index].instanceCanvas.GetComponentInChildren<SettingsManager>();
            if (instances[index].settingsManager != null) {
                instances[index].settingsManager.userManager = instances[index].userManager;
                instances[index].settingsManager.saveKey = instances[index].userManager.machineID;
            }

            // Reminder data
            instances[index].reminderManager = instances[index].instanceCanvas.GetComponentInChildren<ReminderManager>();
            if (instances[index].reminderManager != null) {
                instances[index].reminderManager.userManager = instances[index].userManager;
            }

            // Network data
            instances[index].networkManager = instances[index].instanceCanvas.GetComponentInChildren<NetworkManager>();
            if (instances[index].networkManager != null) {
                instances[index].networkManager.userManager = instances[index].userManager;
            }

            // Wallpaper data
            instances[index].wallpaperManager = instances[index].instanceCanvas.GetComponentInChildren<WallpaperManager>();
            if (instances[index].wallpaperManager != null) {
                instances[index].wallpaperManager.wallpaperIndex = instances[index].userManager.wallpaperIndex;
                instances[index].wallpaperManager.desktopSource.sprite = instances[index].wallpaperManager.wallpaperLibrary.wallpapers[instances[index].userManager.wallpaperIndex].wallpaperSprite;
                instances[index].wallpaperManager.lockScreenSource.sprite = instances[index].wallpaperManager.desktopSource.sprite;
                instances[index].wallpaperManager.selectedWallpaper.sprite = instances[index].wallpaperManager.desktopSource.sprite;
            }

            // Notepad data
            NotepadStoring notepadStoring = instances[index].instanceCanvas.GetComponentInChildren<NotepadStoring>();
            if (notepadStoring != null) {
                notepadStoring.fullPath = instances[index].userManager.notepadDataPath;
                notepadStoring.ReadNoteData();
            }

            // Message data
            MessageStoring messageStoring = instances[index].instanceCanvas.GetComponentInChildren<MessageStoring>();
            if (messageStoring != null) {
                messageStoring.fullPath = instances[index].userManager.messDataPath;
                messageStoring.ReadMessageData();
            }

            // PhotoLibrary data
            PhotoGalleryManager photoGalleryManager = instances[index].instanceCanvas.GetComponentInChildren<PhotoGalleryManager>();
            if (photoGalleryManager != null) {
                photoGalleryManager.userManager = instances[index].userManager;
                photoGalleryManager.InitializePhotos();
            }

            // Video data
            VideoPlayerManager videoPlayerManager = instances[index].instanceCanvas.GetComponentInChildren<VideoPlayerManager>();
            if (videoPlayerManager != null) {
                videoPlayerManager.userManager = instances[index].userManager;
                videoPlayerManager.InitializeVideoPlayer();
            }

            // Audio data
            MusicPlayerManager musicPlayerManager = instances[index].instanceCanvas.GetComponentInChildren<MusicPlayerManager>();
            if (musicPlayerManager != null) {
                musicPlayerManager.userManager = instances[index].userManager;
                musicPlayerManager.InitializePlayer();
            }

            // Widget data
            instances[index].widgetManager = instances[index].instanceCanvas.GetComponentInChildren<WidgetManager>();
            if (instances[index].widgetManager != null) { instances[index].widgetManager.userManager = instances[index].userManager; }

            // WebBrowser data
            instances[index].webBrowserManager = instances[index].instanceCanvas.GetComponentInChildren<WebBrowserManager>();
            if (instances[index].webBrowserManager != null) { instances[index].webBrowserManager.userManager = instances[index].userManager; }

            // Fetch music data display
            instances[index].musicDataItems.Clear();
            foreach (MusicDataDisplay newComp in instances[index].instanceCanvas.GetComponentsInChildren<MusicDataDisplay>()) {
                instances[index].musicDataItems.Add(newComp);
                newComp.userManager = instances[index].userManager;
            }

            // Fetch user info
            instances[index].userInfoItems.Clear();
            foreach (GetUserInfo newComp in instances[index].instanceCanvas.GetComponentsInChildren<GetUserInfo>()) {
                instances[index].userInfoItems.Add(newComp);
                newComp.userManager = instances[index].userManager;
            }

            // Fetch switch
            instances[index].switches.Clear();
            foreach (SwitchManager newComp in instances[index].instanceCanvas.GetComponentsInChildren<SwitchManager>()) {
                instances[index].switches.Add(newComp);
                newComp.userManager = instances[index].userManager;
            }

            // Fetch video data display
            instances[index].videoDataItems.Clear();
            foreach (VideoDataDisplay newComp in instances[index].instanceCanvas.GetComponentsInChildren<VideoDataDisplay>()) {
                instances[index].videoDataItems.Add(newComp);
                newComp.userManager = instances[index].userManager;
            }

            // Set camera
            if (playerCamera == null) { playerCamera = Camera.main; }
            for (int i = 0; i < instances.Count; i++) {
                if (playerCamera != null) { instances[i].worldSpaceManager.mainCamera = playerCamera; }
                else { Debug.LogWarning("<b>[Multi Instance Manager]</b> No main camera found and player camera is missing."); break; }
            }

#if UNITY_EDITOR
            if (EditorApplication.isPlaying)
                return;

            Undo.RecordObject(this, "Initialized DreamOS instances");
            EditorUtility.SetDirty(this);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
        }

        public void SetMachineID(int index) {
            instances[index].userManager.allowMultiInstance = true;
            instances[index].userManager.machineID = "DreamOS #" + index;
            instances[index].worldSpaceManager.transform.parent.name = "DreamOS Instance #" + index;
        }
    }
}