using UnityEngine;
using TMPro;

namespace com.lockedroom.io.module.pc
{
    public class ResetPassword : MonoBehaviour
    {
        [Header("Resources")]
        public UserManager userManager;
        public GameObject user;
        public TextMeshProUGUI securityQuestion;
        public TMP_InputField securityAnswer;
        public TMP_InputField newPassword;
        public TMP_InputField newPasswordRetype;
        public Animator errorMessage;
        public ModalWindowManager modalManager;

        string tempSecAnswer;

        void OnEnable()
        {
            if (userManager == null) { 
                userManager = user.GetComponent<UserManager>(); 
            }

            if (userManager.disableUserCreating == false && userManager.secQuestion != "")
            {
                securityQuestion.text = userManager.secQuestion;
                tempSecAnswer = userManager.secAnswer;
            }

            else
            {
                securityQuestion.text = userManager.systemSecurityQuestion;
                tempSecAnswer = userManager.systemSecurityAnswer;
            }
        }

        public void ChangePassword()
        {
            if (newPassword.text.Length >= userManager.minPasswordCharacter && newPassword.text.Length <= userManager.maxPasswordCharacter
                && newPassword.text == newPasswordRetype.text && securityAnswer.text == tempSecAnswer)
            {
                userManager.password = newPassword.text;
                userManager.hasPassword = true;
                modalManager.CloseWindow();
                newPassword.text = "";
                newPasswordRetype.text = "";
                securityAnswer.text = "";
                userManager.JsonManager.SaveUserData();
            }

            else { errorMessage.Play("Auto In"); }
        }
    }
}