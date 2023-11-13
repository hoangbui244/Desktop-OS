using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.lockedroom.io.module.pc
{
    [CreateAssetMenu(fileName = "New Note Library", menuName = "DreamOS/New Note Library")]
    public class NotepadLibrary : ScriptableObject
    {
        // Library Content
        public List<NoteItem> notes = new List<NoteItem>();

        [System.Serializable]
        public class NoteItem
        {
            public string noteTitle = "Note Title";
            [TextArea] public string noteContent = "Note Description";
            [HideInInspector] public bool isDeleted = false;
            [HideInInspector] public bool isCustom = false;
            [HideInInspector] public bool isModContent = false;
            [HideInInspector] public bool modHelper = false;
        }
    }
}