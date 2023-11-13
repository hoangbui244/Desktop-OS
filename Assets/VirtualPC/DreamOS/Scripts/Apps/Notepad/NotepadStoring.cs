using UnityEngine;
using System.IO;

namespace com.lockedroom.io.module.pc
{
    [AddComponentMenu("DreamOS/Apps/Notepad/Notepad Storing")]
    public class NotepadStoring : MonoBehaviour
    {
        [Header("Resources")]
        public NotepadManager notepadManager;
        public string fullPath;
        int noteIndex;
        string currentTitle;
        string currentContent;

        void Awake()
        {
            if (notepadManager == null)
                notepadManager = gameObject.GetComponent<NotepadManager>();
        }

        public void UpdateData()
        {
            File.WriteAllText(fullPath, "NOTE_DATA");

            for (int i = 0; i < notepadManager.libraryAsset.notes.Count; ++i)
            {
                if (notepadManager.libraryAsset.notes[i].isDeleted) {
                    continue;
                }
                if (notepadManager.libraryAsset.notes[i].isCustom == true)
                {
                    noteIndex = i;
                    WriteNoteData(i);
                }
            }
        }

        public void CheckData() {
            //UserManager userManager = FindObjectOfType<UserManager>();
            //if (userManager != null) {
            //    fullPath = userManager.notepadDataPath;
            //}
            if (!File.Exists(fullPath)) {
                FileInfo dataFile = new FileInfo(fullPath);
                dataFile.Directory.Create();
                File.WriteAllText(fullPath, "NOTE_DATA");
            }
        }

        public void WriteNoteData(int tempIndex)
        {
            File.AppendAllText(fullPath, "\n\nNoteIndex: " + noteIndex.ToString() +
              "\n{" +
              "\n[Title] " + notepadManager.libraryAsset.notes[tempIndex].noteTitle +
              "\n[Content] " + notepadManager.libraryAsset.notes[tempIndex].noteContent +
              "\n}");
        }

        public void ReadNoteData()
        {
            CheckData();
            foreach (string option in File.ReadLines(fullPath))
            {
                if (option.Contains("NoteIndex: "))
                {
                    int tempIndex = int.Parse(option.Replace("NoteIndex: ", ""));
                    noteIndex = tempIndex;
                }

                else if (option.Contains("[Title] "))
                {
                    string tempTitle = option.Replace("[Title] ", "");
                    currentTitle = tempTitle;
                }

                else if (option.Contains("[Content] "))
                {
                    string tempContent = option.Replace("[Content] ", "");
                    currentContent = tempContent;
                    notepadManager.CreateStoredNote(currentTitle, currentContent);
                }
            }
        }
    }
}