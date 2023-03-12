using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Michsky.DreamOS
{
    public class SettingsManager : MonoBehaviour
    {
        // Resources
        public UIManager themeManager;
        public RebootManager rebootManager;
        public UserManager userManager;
        public SetupManager setupManager;
        public Image lockscreenImage;
        public Transform accentColorList;
        public Transform accentReversedColorList;
        public ItemDragContainer desktopDragger;

        // Settings
        public Sprite defaultWallpaper;
        public bool lockDesktopItems = true;

        // Debug
        Toggle toggleHelper;

        void Start()
        {
            // Set selected theme as custom and get the color data
            if (PlayerPrefs.GetString("DreamOS" + "UseCustomTheme") == "true")
            {
                ChangeAccentColor(PlayerPrefs.GetString("DreamOS" + "CustomTheme" + "AccentColor"));
                ChangeAccentReversedColor(PlayerPrefs.GetString("DreamOS" + "CustomTheme" + "AccentRevColor"));
                CheckForToggles();
            }

            if (!PlayerPrefs.HasKey("DreamOS" + "SnapDesktopItems"))
            {
                if (lockDesktopItems == false) { SnapDesktopItems(false); }
                else { SnapDesktopItems(true); }
            }

            else if (PlayerPrefs.GetString("DreamOS" + "SnapDesktopItems") == "false") { SnapDesktopItems(false); }
            else { SnapDesktopItems(true); }

            if (!PlayerPrefs.HasKey("DreamOS" + "SaveDesktopOrder")) { SaveDesktopOrder(false); }
            else if (PlayerPrefs.GetString("DreamOS" + "SaveDesktopOrder") == "false") { SaveDesktopOrder(false); }
            else { SaveDesktopOrder(true); }
        }

        public void SnapDesktopItems(bool value)
        {
            if (desktopDragger == null)
                return;

            if (value == true) 
            { 
                desktopDragger.SnappedDragMode(); 
                PlayerPrefs.SetString("DreamOS" + "SnapDesktopItems", "true"); 
            }

            else 
            { 
                desktopDragger.FreeDragMode();
                PlayerPrefs.SetString("DreamOS" + "SnapDesktopItems", "false"); 
            }
        }

        public void SaveDesktopOrder(bool value)
        {
            if (desktopDragger == null)
                return;

            if (value == true) 
            {
                for (int i = 0; i < desktopDragger.transform.childCount; ++i)
                {
                    ItemDragger tempDragger = desktopDragger.transform.GetChild(i).GetComponent<ItemDragger>();
                    tempDragger.rememberPosition = true;
                    tempDragger.UpdateObject();
                }

                PlayerPrefs.SetString("DreamOS" + "SaveDesktopOrder", "true"); 
            }

            else 
            {
                for (int i = 0; i < desktopDragger.transform.childCount; ++i)
                {
                    ItemDragger tempDragger = desktopDragger.transform.GetChild(i).GetComponent<ItemDragger>();
                    tempDragger.rememberPosition = false;
                    tempDragger.RemoveData();
                }

                PlayerPrefs.SetString("DreamOS" + "SaveDesktopOrder", "false");
            }
        }

        public void CheckForToggles()
        {
            // Invoke color toggle depending on the data
            foreach (Transform obj in accentColorList)
            {
                if (obj.name == PlayerPrefs.GetString("DreamOS" + "CustomTheme" + "AccentColor"))
                {
                    toggleHelper = obj.GetComponent<Toggle>();
                    toggleHelper.isOn = true;
                    toggleHelper.onValueChanged.Invoke(true);
                }
            }

            foreach (Transform obj in accentReversedColorList)
            {
                if (obj.name == PlayerPrefs.GetString("DreamOS" + "CustomTheme" + "AccentRevColor"))
                {
                    toggleHelper = obj.GetComponent<Toggle>();
                    toggleHelper.isOn = true;
                    toggleHelper.onValueChanged.Invoke(true);
                }
            }
        }

        public void SelectSystemTheme()
        {
            themeManager.selectedTheme = UIManager.SelectedTheme.Default;
            PlayerPrefs.SetString("DreamOS" + "UseCustomTheme", "false");
        }

        public void SelectCustomTheme()
        {
            themeManager.selectedTheme = UIManager.SelectedTheme.Custom;
            PlayerPrefs.SetString("DreamOS" + "UseCustomTheme", "true");
        }

        public void ChangeAccentColor(string colorCode)
        {
            // Change color depending on the color code
            Color colorHelper;
            ColorUtility.TryParseHtmlString("#" + colorCode, out colorHelper);
            themeManager.highlightedColorCustom = new Color(colorHelper.r, colorHelper.g, colorHelper.b, themeManager.highlightedColorCustom.a);
            PlayerPrefs.SetString("DreamOS" + "CustomTheme" + "AccentColor", colorCode);
        }

        public void ChangeAccentReversedColor(string colorCodeReversed)
        {
            // Change color depending on the color code
            Color colorHelper;
            ColorUtility.TryParseHtmlString("#" + colorCodeReversed, out colorHelper);
            themeManager.highlightedColorSecondaryCustom = new Color(colorHelper.r, colorHelper.g, colorHelper.b, themeManager.highlightedColorSecondaryCustom.a);
            PlayerPrefs.SetString("DreamOS" + "CustomTheme" + "AccentRevColor", colorCodeReversed);
        }

        public void WipeUserData()
        {
            // Delete user data
            DeleteUserData();
            rebootManager.WipeSystem();
            StartCoroutine("WaitForReboot");
        }

        public void WipeEverything()
        {
            // Delete EVERYTHING! Use with caution
            PlayerPrefs.DeleteAll();
        }

        IEnumerator WaitForReboot()
        {
            yield return new WaitForSeconds(rebootManager.waitTime);
            SelectSystemTheme();
            userManager.InitializeUserManager();
            userManager.desktopScreen.Play("Desktop Out");
            setupManager.PanelAnim(0);
            setupManager.currentBGAnimator = setupManager.steps[0].background.GetComponent<Animator>();
            setupManager.currentPanelAnimator = setupManager.steps[0].panel.GetComponent<Animator>();
            setupManager.currentBGAnimator.Play("Panel In");
            setupManager.currentPanelAnimator.Play("Panel In");
        }

        public void DeleteUserData()
        {
            // User data
            PlayerPrefs.DeleteKey("DreamOS" + "User" + "Created");
            PlayerPrefs.DeleteKey("DreamOS" + "User" + "FirstName");
            PlayerPrefs.DeleteKey("DreamOS" + "User" + "LastName");
            PlayerPrefs.DeleteKey("DreamOS" + "User" + "Password");
            PlayerPrefs.DeleteKey("DreamOS" + "User" + "SecQuestion");
            PlayerPrefs.DeleteKey("DreamOS" + "User" + "SecAnswer");
            PlayerPrefs.DeleteKey("DreamOS" + "User" + "ProfilePicture");
            PlayerPrefs.DeleteKey("DreamOS" + "User" + "DreamOS" + "UseCustomTheme");

            // Reminder data
            PlayerPrefs.DeleteKey("Reminder1Enabled");
            PlayerPrefs.DeleteKey("Reminder2Enabled");
            PlayerPrefs.DeleteKey("Reminder3Enabled");
            PlayerPrefs.DeleteKey("Reminder4Enabled");
            PlayerPrefs.DeleteKey("Reminder1Title");
            PlayerPrefs.DeleteKey("Reminder1Hour");
            PlayerPrefs.DeleteKey("Reminder1Minute");
            PlayerPrefs.DeleteKey("Reminder1Type");
            PlayerPrefs.DeleteKey("Reminder2Title");
            PlayerPrefs.DeleteKey("Reminder2Hour");
            PlayerPrefs.DeleteKey("Reminder2Minute");
            PlayerPrefs.DeleteKey("Reminder2Type");
            PlayerPrefs.DeleteKey("Reminder3Title");
            PlayerPrefs.DeleteKey("Reminder3Hour");
            PlayerPrefs.DeleteKey("Reminder3Minute");
            PlayerPrefs.DeleteKey("Reminder3Type");
            PlayerPrefs.DeleteKey("Reminder4Title");
            PlayerPrefs.DeleteKey("Reminder4Hour");
            PlayerPrefs.DeleteKey("Reminder4Minute");
            PlayerPrefs.DeleteKey("Reminder4Type");

            // Network data
            PlayerPrefs.DeleteKey("ConnectedNetworkTitle");
            PlayerPrefs.DeleteKey("CurrentNetworkIndex");
        }
    }
}