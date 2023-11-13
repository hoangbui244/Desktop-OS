using System.IO;
using UnityEngine;

namespace com.lockedroom.io.module.pc {
    public class JsonManager : MonoBehaviour {
        public UserManager userManager;
        public string userDataPath;
        public string userFullDataPath;
        public int index;

        // Desktop scene
        public string subpath = "Data/User";

        // Multi instances scene
        public string mainPath = "Data";
        public string secondPath = "User";
        
        public void CheckUserData() {
            string firstPath = "Instance" + index.ToString();
            string userFileName = "User";
            string fileExtension = ".json";
#if UNITY_EDITOR
            // Desktop scene
            //userDataPath = Application.dataPath + "/" + subpath + "/" + userFileName + fileExtension;

            // Multi instances scene
            userDataPath = mainPath + "/" + firstPath + "/" + secondPath + "/" + userFileName + fileExtension;
            userFullDataPath = Application.dataPath + "/" + userDataPath;
#else
            string appPath = Application.persistentDataPath;
            userFullDataPath = Path.Combine(appPath, mainPath, firstPath, secondPath, userFileName + fileExtension);
#endif
            if (!File.Exists(userFullDataPath)) {
                FileInfo dataFile = new FileInfo(userFullDataPath);
                dataFile.Directory.Create();
            }
            else {
                LoadUserData();
            }
        }

        public void LoadUserData() {
            string jsonData = File.ReadAllText(userFullDataPath);
            UserData loadData = JsonUtility.FromJson<UserData>(jsonData);
            userManager.firstName = loadData.firstName;
            userManager.lastName = loadData.lastName;
            userManager.password = loadData.password;
            userManager.secQuestion = loadData.secQuestion;
            userManager.secAnswer = loadData.secAnswer;
            userManager.ppIndex = loadData.ppIndex;
            userManager.userCreated = loadData.userCreated;
            userManager.notepadDataPath = loadData.notepadDataPath;
            userManager.wallpaperIndex = loadData.wallpaperIndex;
            userManager.messDataPath = loadData.messDataPath;
            userManager.photoDataPath = loadData.photoDataPath;
            userManager.videoDataPath = loadData.videoDataPath;
            userManager.audioDataPath = loadData.audioDataPath;
            userManager.photoAlbum = loadData.photoAlbum;
            userManager.videoAlbum = loadData.videoAlbum;
            userManager.audioAlbum = loadData.audioAlbum;
        }

        public void SaveUserData() {
            UserData userData = userManager.GetUserData();

            string jsonData = JsonUtility.ToJson(userData);
            File.WriteAllText(userFullDataPath, jsonData);
        }

        public void DeleteUserData() {
            if (File.Exists(userFullDataPath)) {
                File.Delete(userFullDataPath);
            }
        }
    }
}