﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace com.lockedroom.io.module.pc
{
    [CreateAssetMenu(fileName = "New Photo Library", menuName = "DreamOS/New Photo Library")]
    public class PhotoGalleryLibrary : ScriptableObject
    {
        // Library Content
        public List<PictureItem> pictures = new List<PictureItem>();

        [System.Serializable]
        public class PictureItem
        {
            public string pictureTitle = "Picture Title";
            public string pictureDescription = "Picture Description";
            public Sprite pictureSprite;
            [HideInInspector] public bool isModContent = false;
            [HideInInspector] public bool modHelper = false;
        }
    }
}