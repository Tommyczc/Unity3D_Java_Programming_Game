using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Michsky.DreamOS
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class WindowManager : MonoBehaviour, IPointerClickHandler
    {
        // Resources
        public Animator windowAnimator;
        public RectTransform windowContent;
        public RectTransform navbarRect;
        public GameObject taskBarButton;
        public WindowDragger windowDragger;
        public WindowResize windowResize;

        // Fullscreen & minimize
        public GameObject fullscreenImage;
        public GameObject normalizeImage;

        // Settings
        public bool enableBackgroundBlur = true;
        public bool enableMobileMode = false;
        public bool hasNavDrawer = true;
        public float minNavbarWidth = 75;
        public float maxNavbarWidth = 300;
        [Range(1, 25)] public float smoothness = 10;
        public DefaultState defaultNavbarState = DefaultState.Minimized;

        // Events
        public UnityEvent onEnableEvents;
        public UnityEvent onLaunchEvents;
        public UnityEvent onQuitEvents;

        public enum DefaultState { Minimized, Expanded }

        TaskBarButton tbbHelper;
        BlurManager windowBGBlur;
        RectTransform windowRect;

        float left;
        float right;
        float top;
        float bottom;
        bool isNavDrawerOpen = true;
        bool isInTransition = false;

        [HideInInspector] public bool isNormalized;
        [HideInInspector] public bool isFullscreen;
        [HideInInspector] public bool disableAtStart = true;

        void Start()
        {
            SetupWindow();
        }

        void OnEnable()
        {
            onEnableEvents.Invoke();

            if (hasNavDrawer == true)
            {
                if (navbarRect == null || windowContent == null)
                {
                    Debug.LogError("<b>[Window Manager]</b> Navbar is enabled but its resources are missing!");
                    return;
                }

                if (PlayerPrefs.GetString(gameObject.name + "NavDrawer") == "true")
                {
                    defaultNavbarState = DefaultState.Expanded;
                    isNavDrawerOpen = true;
                }

                else if (PlayerPrefs.GetString(gameObject.name + "NavDrawer") == "false"
                    || !PlayerPrefs.HasKey(gameObject.name + "NavDrawer"))
                {
                    defaultNavbarState = DefaultState.Minimized;
                    isNavDrawerOpen = false;
                }

                if (defaultNavbarState == DefaultState.Minimized)
                {
                    navbarRect.sizeDelta = new Vector2(minNavbarWidth, navbarRect.sizeDelta.y);
                    windowContent.offsetMin = new Vector2(minNavbarWidth, windowContent.offsetMin.y);
                }

                else if (defaultNavbarState == DefaultState.Expanded)
                {
                    navbarRect.sizeDelta = new Vector2(maxNavbarWidth, navbarRect.sizeDelta.y);
                    windowContent.offsetMin = new Vector2(maxNavbarWidth, windowContent.offsetMin.y);
                }
            }
        }

        void Update()
        {
            if (isInTransition == false)
                return;

            if (defaultNavbarState == DefaultState.Minimized)
            {
                navbarRect.sizeDelta = Vector2.Lerp(navbarRect.sizeDelta, new Vector2(minNavbarWidth, navbarRect.sizeDelta.y), Time.deltaTime * smoothness);
                windowContent.offsetMin = Vector2.Lerp(windowContent.offsetMin, new Vector2(minNavbarWidth, windowContent.offsetMin.y), Time.deltaTime * smoothness);

                if (navbarRect.sizeDelta.x <= minNavbarWidth + 0.1f)
                    isInTransition = false;
            }

            else if (defaultNavbarState == DefaultState.Expanded)
            {
                navbarRect.sizeDelta = Vector2.Lerp(navbarRect.sizeDelta, new Vector2(maxNavbarWidth, navbarRect.sizeDelta.y), Time.deltaTime * smoothness);
                windowContent.offsetMin = Vector2.Lerp(windowContent.offsetMin, new Vector2(maxNavbarWidth, windowContent.offsetMin.y), Time.deltaTime * smoothness);

                if (navbarRect.sizeDelta.x >= maxNavbarWidth - 0.1f)
                    isInTransition = false;
            }
        }

        public void SetupWindow()
        {
            if (windowAnimator == null)
                windowAnimator = gameObject.GetComponent<Animator>();

            if (taskBarButton != null && enableMobileMode == false)
            {
                try
                {
                    tbbHelper = taskBarButton.GetComponent<TaskBarButton>();
                    tbbHelper.windowManager = this.GetComponent<WindowManager>();
                }

                catch { Debug.Log("<b>[Window Manager]</b> No variable attached to Task Bar Button. Task Bar functions won't be working properly.", this); }
            }

            if (enableBackgroundBlur == true)
            {
                try { windowBGBlur = gameObject.GetComponent<BlurManager>(); }
                catch { Debug.Log("<b>[Window Manager]</b> No Blur Manager attached to Game Object. Background Blur won't be working.", this); }
            }

            windowRect = gameObject.GetComponent<RectTransform>();

            if (windowDragger != null)
                windowDragger.wManager = this;

            left = windowRect.offsetMin.x;
            right = -windowRect.offsetMax.x;
            top = -windowRect.offsetMax.y;
            bottom = windowRect.offsetMin.y;

            if (disableAtStart == true)
                gameObject.SetActive(false);

            if (enableMobileMode == true)
                return;

            if (fullscreenImage != null && normalizeImage != null)
            {
                fullscreenImage.SetActive(true);
                normalizeImage.SetActive(false);
            }
        }

        public void NavDrawerAnimate()
        {
            if (navbarRect == null || windowContent == null)
                return;

            if (isNavDrawerOpen == true)
            {
                PlayerPrefs.SetString(gameObject.name + "NavDrawer", "false");
                defaultNavbarState = DefaultState.Minimized;
                isNavDrawerOpen = false;
            }

            else
            {
                PlayerPrefs.SetString(gameObject.name + "NavDrawer", "true");
                defaultNavbarState = DefaultState.Expanded;
                isNavDrawerOpen = true;
            }

            isInTransition = true;
        }

        public void OpenWindow()
        {
            StopCoroutine("DisableObject");
            gameObject.SetActive(true);

            if (!windowAnimator.GetCurrentAnimatorStateInfo(0).IsName("Panel Fullscreen")
                && (!windowAnimator.GetCurrentAnimatorStateInfo(0).IsName("Panel Normalize")))
                windowAnimator.Play("Panel In");

            if (taskBarButton != null && enableMobileMode == false)
                tbbHelper.SetOpen();
            else if (taskBarButton != null && enableMobileMode == true)
                taskBarButton.SetActive(true);

            if (windowBGBlur != null)
                windowBGBlur.BlurInAnim();

            FocusToWindow();
            onLaunchEvents.Invoke();
        }

        public void CloseWindow()
        {
            if (enableMobileMode == false)
            {
                if (taskBarButton != null) { tbbHelper.SetClosed(); }
                else if (taskBarButton != null) { taskBarButton.SetActive(false); }

                if (enableBackgroundBlur == true && windowBGBlur != null)
                    windowBGBlur.BlurOutAnim();

                windowAnimator.Play("Panel Out");
            }

            else
            {
                taskBarButton.SetActive(false);

                if (windowAnimator.GetCurrentAnimatorStateInfo(0).IsName("Panel Shrink")
                    || windowAnimator.GetCurrentAnimatorStateInfo(0).IsName("Panel Shrink Helper"))
                    windowAnimator.Play("Panel Shrink Minimize");
                else
                    windowAnimator.Play("Panel Out");
            }

            StartCoroutine("DisableObject");
            onQuitEvents.Invoke();
        }

        public void MinimizeWindow()
        {
            windowAnimator.Play("Panel Minimize");

            if (taskBarButton != null) { tbbHelper.SetMinimized(); }
            if (enableBackgroundBlur == true && windowBGBlur != null) { windowBGBlur.BlurOutAnim(); }
        }

        public void FullscreenWindow()
        {
            if (isFullscreen == false)
            {
                isFullscreen = true;
                isNormalized = false;

                if (fullscreenImage != null && normalizeImage != null)
                {
                    fullscreenImage.SetActive(false);
                    normalizeImage.SetActive(true);
                }

                StartCoroutine("SetFullscreen");
            }

            else
            {
                isFullscreen = false;
                isNormalized = true;

                if (fullscreenImage != null && normalizeImage != null)
                {
                    fullscreenImage.SetActive(true);
                    normalizeImage.SetActive(false);
                }

                StartCoroutine("SetNormalized");
            }
        }

        public void FocusToWindow() { gameObject.transform.SetAsLastSibling(); }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            FocusToWindow();
        }

        IEnumerator SetFullscreen()
        {
            left = windowRect.offsetMin.x;
            right = -windowRect.offsetMax.x;
            top = -windowRect.offsetMax.y;
            bottom = windowRect.offsetMin.y;

            windowAnimator.Play("Panel Fullscreen");

            // Left and bottom
            windowRect.offsetMin = new Vector2(0, 0);

            // Right and top
            windowRect.offsetMax = new Vector2(0, 0);

            isFullscreen = true;
            isNormalized = false;

            if (windowResize != null)
                windowResize.gameObject.SetActive(false);

            yield return null;
        }

        IEnumerator SetNormalized()
        {
            windowAnimator.Play("Panel Normalize");

            // Left and bottom
            windowRect.offsetMin = new Vector2(left, bottom);

            // Right and top
            windowRect.offsetMax = new Vector2(-right, -top);

            isFullscreen = false;
            isNormalized = true;

            if (windowResize != null)
                windowResize.gameObject.SetActive(true);

            yield return null;
        }

        IEnumerator DisableObject()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }

        #region Mobile Functions
        public void ShrinkWindowMobile() { windowAnimator.Play("Panel Shrink"); }
        public void ShrinkWindowInstantMobile() { windowAnimator.Play("Panel Shrink Helper"); }
        public void ShrikWindowMinimizeMobile() { windowAnimator.Play("Panel Shrink Minimize"); }
        public void ExpandWindowMobile() { windowAnimator.Play("Panel Expand"); }
        public void MinimizeWindowMobile() { windowAnimator.Play("Panel Out"); }
        #endregion
    }
}