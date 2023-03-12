using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Michsky.DreamOS
{
    public class WindowPanelManager : MonoBehaviour
    {
        // List
        public List<PanelItem> panels = new List<PanelItem>();

        // Settings
        public bool editMode;
        public int currentPanelIndex = 0;
        private int currentButtonIndex = 0;
        private int newPanelIndex;

        private GameObject currentPanel;
        private GameObject nextPanel;
        private GameObject currentButton;
        private GameObject nextButton;

        private Animator currentPanelAnimator;
        private Animator nextPanelAnimator;
        private Animator currentButtonAnimator;
        private Animator nextButtonAnimator;

        private string panelFadeIn = "Panel In";
        private string panelFadeOut = "Panel Out";
        private string panelFadeOutHelper = "Panel Out Helper";
        private string buttonFadeIn = "Closed to Open";
        private string buttonFadeOut = "Open to Closed";

        bool firstTime = true;

        [System.Serializable]
        public class PanelItem
        {
            public string panelName;
            public GameObject panelObject;
            public GameObject buttonObject;
        }

        void OnEnable()
        {
            if (firstTime == false && currentPanel != null)
            {
                currentPanelAnimator = currentPanel.GetComponent<Animator>();
                currentPanelAnimator.Play(panelFadeOutHelper);
            }

            if (firstTime == false && currentButton != null)
            {
                currentButtonAnimator = currentButton.GetComponent<Animator>();
                currentButtonAnimator.Play(buttonFadeOut);
            }

            OpenFirstTab();
        }

        void Start()
        {
            if (panels[currentPanelIndex].buttonObject != null)
            {
                currentButton = panels[currentPanelIndex].buttonObject;
                currentButtonAnimator = currentButton.GetComponent<Animator>();
                currentButtonAnimator.Play(buttonFadeIn);
            }

            currentPanel = panels[currentPanelIndex].panelObject;
            currentPanelAnimator = currentPanel.GetComponent<Animator>();
            currentPanelAnimator.Play(panelFadeIn);

            firstTime = false;
        }

        public void OpenFirstTab()
        {
            currentPanel = panels[0].panelObject;
            currentPanelAnimator = currentPanel.GetComponent<Animator>();
            currentPanelAnimator.Play(panelFadeIn);
            currentPanelIndex = 0;

            if (panels[0].buttonObject != null)
            {
                currentButton = panels[0].buttonObject;
                currentButtonAnimator = currentButton.GetComponent<Animator>();
                currentButtonAnimator.Play(buttonFadeIn);
            }

            currentButtonIndex = 0;
        }

        public void OpenPanel(string newPanel)
        {
            for (int i = 0; i < panels.Count; i++)
            {
                if (panels[i].panelName == newPanel)
                    newPanelIndex = i;
            }

            if (newPanelIndex != currentPanelIndex)
            {
                currentPanel = panels[currentPanelIndex].panelObject;
                currentPanelIndex = newPanelIndex;
                nextPanel = panels[currentPanelIndex].panelObject;

                currentPanelAnimator = currentPanel.GetComponent<Animator>();
                nextPanelAnimator = nextPanel.GetComponent<Animator>();

                currentPanelAnimator.Play(panelFadeOut);
                nextPanelAnimator.Play(panelFadeIn);


                if (panels[currentButtonIndex].buttonObject != null)
                {
                    currentButton = panels[currentButtonIndex].buttonObject;
                    currentButtonAnimator = currentButton.GetComponent<Animator>();
                    currentButtonAnimator.Play(buttonFadeOut);
                }

                currentButtonIndex = newPanelIndex;

                if (panels[currentButtonIndex].buttonObject != null)
                {
                    nextButton = panels[currentButtonIndex].buttonObject;
                    nextButtonAnimator = nextButton.GetComponent<Animator>();
                    nextButtonAnimator.Play(buttonFadeIn);
                }
            }
        }

        public void NextPage()
        {
            if (currentPanelIndex <= panels.Count - 2)
            {
                currentPanel = panels[currentPanelIndex].panelObject;
                currentButton = panels[currentButtonIndex].buttonObject;
                nextButton = panels[currentButtonIndex + 1].buttonObject;

                currentPanelAnimator = currentPanel.GetComponent<Animator>();
                currentButtonAnimator = currentButton.GetComponent<Animator>();

                currentButtonAnimator.Play(buttonFadeOut);
                currentPanelAnimator.Play(panelFadeOut);

                currentPanelIndex += 1;
                currentButtonIndex += 1;
                nextPanel = panels[currentPanelIndex].panelObject;

                nextPanelAnimator = nextPanel.GetComponent<Animator>();
                nextButtonAnimator = nextButton.GetComponent<Animator>();
                nextPanelAnimator.Play(panelFadeIn);
                nextButtonAnimator.Play(buttonFadeIn);
            }
        }

        public void PrevPage()
        {
            if (currentPanelIndex >= 1)
            {
                currentPanel = panels[currentPanelIndex].panelObject;
                currentButton = panels[currentButtonIndex].buttonObject;
                nextButton = panels[currentButtonIndex - 1].buttonObject;

                currentPanelAnimator = currentPanel.GetComponent<Animator>();
                currentButtonAnimator = currentButton.GetComponent<Animator>();

                currentButtonAnimator.Play(buttonFadeOut);
                currentPanelAnimator.Play(panelFadeOut);

                currentPanelIndex -= 1;
                currentButtonIndex -= 1;
                nextPanel = panels[currentPanelIndex].panelObject;

                nextPanelAnimator = nextPanel.GetComponent<Animator>();
                nextButtonAnimator = nextButton.GetComponent<Animator>();
                nextPanelAnimator.Play(panelFadeIn);
                nextButtonAnimator.Play(buttonFadeIn);
            }
        }
    }
}