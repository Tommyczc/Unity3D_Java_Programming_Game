using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Michsky.DreamOS
{
    [RequireComponent(typeof(Animator))]
    public class TaskBarButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler
    {
        [Header("Resources")]
        public Animator buttonAnimator;
        [HideInInspector] public WindowManager windowManager;

        [Header("Settings")]
        public string buttonTitle = "App Window";
        public bool defaultPinState = true;

        [Header("Context Menu Pin")]
        public ContextMenuContent contextMenuContent;
        public Sprite pinIcon;
        public string pinLabel;
        public Sprite unpinIcon;
        public string unpinLabel;

        [Header("Events")]
        public UnityEvent onClickEvents;

        [HideInInspector] public bool isPinned;
        [HideInInspector] public int buttonID;

        bool firstTime = true;
        bool isDragging = false;

        void Start()
        {
            if (buttonAnimator == null) { buttonAnimator = gameObject.GetComponent<Animator>(); }

            if (defaultPinState == true && !PlayerPrefs.HasKey(buttonTitle + "TaskbarShortcut"))
                PlayerPrefs.SetString(buttonTitle + "TaskbarShortcut", "true");

            if (PlayerPrefs.GetString(buttonTitle + "TaskbarShortcut") == "true") { isPinned = true; }
            else { isPinned = false; }

            if (isPinned == true) { buttonAnimator.Play("Draw"); }
            else { buttonAnimator.Play("Hide"); }

            firstTime = false;

            if (contextMenuContent != null)
            {
                contextMenuContent.ClearMenu();

                if (isPinned == false) { AddPinEvent(pinLabel, pinIcon); }
                else { AddPinEvent(unpinLabel, unpinIcon); }
            }
        }

        void OnEnable()
        {
            if (firstTime == false && isPinned == true) { buttonAnimator.Play("Draw"); }
            else if (firstTime == false && isPinned == false) { buttonAnimator.Play("Hide"); }

            StartCoroutine("DisableAnimator");
        }

        public void OnBeginDrag(PointerEventData data) { isDragging = true; }
        public void OnEndDrag(PointerEventData data) { isDragging = false; }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right || isDragging == true)
                return;

            if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Active to Highlighted")
                && windowManager.transform.GetSiblingIndex() != windowManager.transform.parent.childCount - 1)
            {
                windowManager.FocusToWindow();
                buttonAnimator.Play("Inactive to Active");
            }

            else if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Active to Highlighted")
                || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Closed to Active")
                || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Inactive to Active")
                || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted to Active")
                && windowManager.transform.GetSiblingIndex() == windowManager.transform.parent.childCount - 1)
            {
                windowManager.MinimizeWindow();
                buttonAnimator.Play("Active to Inactive");              
            }

            else if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Inactive to Highlighted")
                || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted to Inactive")
                || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Active to Inactive"))
            {
                onClickEvents.Invoke();
                windowManager.FocusToWindow();
                buttonAnimator.Play("Highlighted to Active");
            }

            else if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hide"))
            {
                onClickEvents.Invoke();
                windowManager.FocusToWindow();
                buttonAnimator.Play("Hide to Active");
            }

            else
            {
                onClickEvents.Invoke();
                windowManager.FocusToWindow();
                buttonAnimator.Play("Closed to Active");
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            StopCoroutine("DisableAnimator");
            buttonAnimator.enabled = true;

            if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted to Closed")
                || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Active to Closed"))
                buttonAnimator.Play("Closed to Highlighted");

            else if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted to Active")
                || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Closed to Active")
               || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Inactive to Active")
                     || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hide to Active"))
                buttonAnimator.Play("Active to Highlighted");

            else if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Active to Inactive")
                || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted to Inactive"))
                buttonAnimator.Play("Inactive to Highlighted");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Closed to Highlighted"))
                buttonAnimator.Play("Highlighted to Closed");
            else if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Active to Highlighted"))
                buttonAnimator.Play("Highlighted to Active");
            else if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Inactive to Highlighted"))
                buttonAnimator.Play("Highlighted to Inactive");

            StartCoroutine("DisableAnimator");
        }

        public void SetOpen()
        {
            StopCoroutine("DisableAnimator");
            buttonAnimator.enabled = true;

            if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Inactive to Highlighted")
                || buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Active to Highlighted"))
                buttonAnimator.Play("Highlighted to Active");
            else if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hide"))
                buttonAnimator.Play("Hide to Active");
            else
                buttonAnimator.Play("Closed to Active");

            StartCoroutine("DisableAnimator");
        }

        public void SetClosed()
        {
            StopCoroutine("DisableAnimator");
            buttonAnimator.enabled = true;

            if (PlayerPrefs.GetString(buttonTitle + "TaskbarShortcut") == "true") { buttonAnimator.Play("Active to Closed"); }
            else { buttonAnimator.Play("Hide"); }

            StartCoroutine("DisableAnimator");
        }

        public void SetMinimized()
        {
            StopCoroutine("DisableAnimator");
            buttonAnimator.enabled = true;
            buttonAnimator.Play("Active to Inactive");
            StartCoroutine("DisableAnimator");
        }

        public void PinTaskBarButton()
        {
            if (isPinned == false)
            {
                PlayerPrefs.SetString(buttonTitle + "TaskbarShortcut", "true");
                isPinned = true;
            }

            else
            {
                if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("Highlighted to Closed"))
                    buttonAnimator.Play("Hide");

                PlayerPrefs.SetString(buttonTitle + "TaskbarShortcut", "false");
                isPinned = false;
            }

            if (contextMenuContent != null)
            {
                contextMenuContent.ClearMenu();

                if (isPinned == false) { AddPinEvent(pinLabel, pinIcon); }
                else { AddPinEvent(unpinLabel, unpinIcon); }
            }
        }

        void AddPinEvent(string label, Sprite icon)
        {
            ContextMenuContent.ContextItem item = new ContextMenuContent.ContextItem();
            item.itemText = label;
            item.itemIcon = icon;
            item.onClickEvents = new UnityEvent();
            item.onClickEvents.AddListener(PinTaskBarButton);
            contextMenuContent.contexItems.Add(item);
        }

        void AddLaunchEvent(string label, Sprite icon)
        {
            ContextMenuContent.ContextItem item = new ContextMenuContent.ContextItem();
            item.itemText = label;
            item.itemIcon = icon;
            item.onClickEvents = new UnityEvent();
            item.onClickEvents.AddListener(windowManager.OpenWindow);
            contextMenuContent.contexItems.Add(item);
        }

        IEnumerator DisableAnimator()
        {
            yield return new WaitForSeconds(0.5f);
            buttonAnimator.enabled = false;
        }
    }
}