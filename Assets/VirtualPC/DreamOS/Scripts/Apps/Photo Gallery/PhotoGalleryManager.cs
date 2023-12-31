﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace com.lockedroom.io.module.pc {
    public class PhotoGalleryManager : MonoBehaviour {
        // Resources
        public PhotoGalleryLibrary libraryAsset;
        public Sprite loadedSprite;
        [SerializeField] private Transform pictureLibraryParent;
        public GameObject user;
        public UserManager userManager;
        public GameObject pictureLibraryButton;
        public GameObject photoGalleryWindow;
        public Image imageViewer;
        public TextMeshProUGUI viewerTitle;
        public TextMeshProUGUI viewerDescription;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button previousButton;
        [HideInInspector] public WindowPanelManager wManager;

        // Settings
        public bool sortListByName = true;
        public bool allowArrowNavigation = true;
        public string viewerPanelName = "Viewer";
        [HideInInspector] public bool modSupport;
        [HideInInspector] public int currentIndex;

        private static int SortByName(PhotoData o1, PhotoData o2) {
            // Compare the names and sort by A to Z
            return o1.photoTitle.CompareTo(o2.photoTitle);
        }

        void Awake() {
            userManager = user.GetComponentInChildren<UserManager>();
            // Get window manager
            wManager = photoGalleryWindow.GetComponent<WindowPanelManager>();
            if (modSupport == true)
                return;

            //InitializePhotos();
        }

        void Update() {
            if (allowArrowNavigation == false) { 
                this.enabled = false; 
                return; 
            }
            if (wManager.panels[wManager.currentPanelIndex].panelName != viewerPanelName) { 
                return; 
            }

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.LeftArrow) && currentIndex > 0) { 
                PrevAction(); 
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && currentIndex < userManager.photoAlbum.Count - 1) { 
                NextAction(); 
            }
#elif ENABLE_INPUT_SYSTEM
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame && currentIndex > 0) { PrevAction(); }
            else if (Keyboard.current.rightArrowKey.wasPressedThisFrame && currentIndex < libraryAsset.pictures.Count - 1) { NextAction(); }
#endif
        }

        public void InitializePhotos() {
            PreparePhotos();
            nextButton.onClick.AddListener(NextAction);
            previousButton.onClick.AddListener(PrevAction);
        }

        void PreparePhotos() {

            // Destroy each object in picture library parent before creating the new ones
            foreach (Transform child in pictureLibraryParent) { Destroy(child.gameObject); }

            // Sort pictures by A to Z if it's enabled
            if (sortListByName == true) { userManager.photoAlbum.Sort(SortByName); }

            // Instantiate the entire picture library as buttons
            for (int i = 0; i < userManager.photoAlbum.Count; ++i) {
                // Checking for mods

                //if (libraryAsset.pictures[i].isModContent == true && libraryAsset.pictures[i].modHelper == true
                //    || libraryAsset.pictures[i].pictureSprite == null)
                //{
                //    libraryAsset.pictures.RemoveAt(i);
                //    i--; continue;
                //}

                // Spawn picture button
                GameObject go = Instantiate(pictureLibraryButton, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                go.transform.SetParent(pictureLibraryParent, false);
                go.gameObject.name = userManager.photoAlbum[i].photoTitle;

                // Set image
                Image coverGO = go.transform.Find("Image Parent/Image").GetComponent<Image>();

                // Load the texture from the file path
                string photoPath = Application.dataPath + "/" + userManager.photoAlbum[i].photoPath;

                // Create a Sprite using the loaded texture
                Texture2D texture = LoadTextureFromFile(photoPath);

                // Create a Sprite using the loaded texture
                if (texture != null) { 
                    loadedSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                    coverGO.sprite = loadedSprite;
                }

                // Fit picture to the box depending on its width and height
                AspectRatioFitter arf = coverGO.GetComponent<AspectRatioFitter>();
                if (coverGO.sprite.texture.height > coverGO.sprite.texture.width) { arf.aspectRatio = 0.5f; }
                else { arf.aspectRatio = 1.8f; }

                // Set ID tags
                TextMeshProUGUI photoText = go.transform.Find("Highlighted/Image Title").GetComponent<TextMeshProUGUI>();
                photoText.text = userManager.photoAlbum[i].photoTitle;
                TextMeshProUGUI descText = go.transform.Find("Highlighted/Image Description").GetComponent<TextMeshProUGUI>();
                descText.text = userManager.photoAlbum[i].photoDescription;

                // Add button events
                Button itemButton = go.GetComponent<Button>();
                itemButton.onClick.AddListener(delegate {
                    //OpenCustomPicture(go.transform.GetSiblingIndex());

                    // Open picture depending on picture index from the library
                    currentIndex = go.transform.GetSiblingIndex();
                    imageViewer.sprite = coverGO.sprite;
                    viewerTitle.text = userManager.photoAlbum[currentIndex].photoTitle;
                    viewerDescription.text = userManager.photoAlbum[currentIndex].photoDescription;
                    CheckForButtonStates();
                    wManager.OpenPanel(viewerPanelName);
                });
            }
        }
        /// <summary>
        /// Loading image function
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
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

        public void OpenCustomPicture(int pictureIndex) {
            // Open picture depending on picture index from the library
            //currentIndex = pictureIndex;
            //imageViewer.sprite = userManager.photoAlbum[pictureIndex].pictureSprite;
            //viewerTitle.text = userManager.photoAlbum[pictureIndex].title;
            //viewerDescription.text = userManager.photoAlbum[pictureIndex].description;
            //CheckForButtonStates();
        }

        public void OpenCustomSprite(Sprite pictureIndex, string title, string description) {
            // Open picture depending on vars (e.g. downloaded file)
            imageViewer.sprite = pictureIndex;
            viewerTitle.text = title;
            viewerDescription.text = description;
            wManager.OpenPanel(viewerPanelName);
        }

        private void NextAction() {
            pictureLibraryParent.GetChild(currentIndex + 1).GetComponent<Button>().onClick.Invoke();
        }
        private void PrevAction() {
            pictureLibraryParent.GetChild(currentIndex - 1).GetComponent<Button>().onClick.Invoke();
        }

        // Check for the current item and change button states
        public void CheckForButtonStates() {
            if (userManager.photoAlbum.Count == 1) {
                nextButton.gameObject.SetActive(false);
                previousButton.gameObject.SetActive(false);
                return;
            }

            if (currentIndex == 0) { previousButton.gameObject.SetActive(false); }
            else { previousButton.gameObject.SetActive(true); }

            if (currentIndex == userManager.photoAlbum.Count - 1) { nextButton.gameObject.SetActive(false); }
            else { nextButton.gameObject.SetActive(true); }
        }
    }
}