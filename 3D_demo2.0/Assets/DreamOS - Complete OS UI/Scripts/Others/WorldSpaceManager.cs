using System.Collections;
using UnityEngine;
using UnityEngine.Events;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Michsky.DreamOS
{
    public class WorldSpaceManager : MonoBehaviour
    {
        // Resources
        public Transform mainCamera;
        public Transform enterMount;
        public Camera projectorCam;
        public Canvas osCanvas;
        public FloatingIconManager useFloatingIcon;

        // Settings
        public bool requiresOpening = true;
        public bool autoGetIn = false;
        public bool lockCursorWhenOut = false;
        public string playerTag = "Player";
#if ENABLE_LEGACY_INPUT_MANAGER
        public KeyCode getInKey = KeyCode.E;
        public KeyCode getOutKey = KeyCode.Escape;
#elif ENABLE_INPUT_SYSTEM
        public InputAction getInKey;
        public InputAction getOutKey;
#endif
        [Range(0.1f, 50f)] public float transitionSpeed = 10f;
        public float transitionInTimer = 0.7f;
        public float transitionOutTimer = 0.55f;
        public TransitionMode transitionMode;

        // Events
        public UnityEvent onEnter;
        public UnityEvent onEnterEnd;
        public UnityEvent onExit;
        public UnityEvent onExitEnd;

        // Hidden & Helpers
        public int selectedTagIndex = 0;
        public bool isInSystem = false;
        bool isInTrigger = false;
        bool enableCameraIn = false;
        bool enableCameraOut = false;
        bool takenStaticRootPos;

        RenderTexture uiRT;
        CanvasGroup osCG;
        Quaternion camRotHelper;
        Vector3 targetRootPos = new Vector3(0, 0, 0);

        public enum TransitionMode { Static, Dynamic }

        void Start()
        {
            uiRT = projectorCam.targetTexture;
            uiRT.width = Screen.currentResolution.width;
            uiRT.height = Screen.currentResolution.height;
            
            osCG = osCanvas.GetComponent<CanvasGroup>();
            osCG.interactable = false;
            osCG.blocksRaycasts = false;
            if (requiresOpening == true) { osCanvas.gameObject.SetActive(false); }
        }

        void Update()
        {
            if (isInTrigger == false && isInSystem == true)
            {
#if ENABLE_LEGACY_INPUT_MANAGER
                if (Input.GetKeyDown(getOutKey)) { TransitionOutHelper(); }
#elif ENABLE_INPUT_SYSTEM
                if (getOutKey.triggered) { TransitionOutHelper(); }
#endif
            }

            else if (isInSystem == false)
            {
#if ENABLE_LEGACY_INPUT_MANAGER
                if (Input.GetKeyDown(getInKey)) { TransitionInHelper(); }
#elif ENABLE_INPUT_SYSTEM
                if (getInKey.triggered) { TransitionInHelper(); }
#endif
            }

            if (enableCameraIn == true)
            {
                mainCamera.position = Vector3.Lerp(mainCamera.position, enterMount.position, transitionSpeed * Time.deltaTime);
                mainCamera.rotation = Quaternion.Slerp(mainCamera.rotation, enterMount.rotation, transitionSpeed * Time.deltaTime);
            }

            else if (enableCameraOut == true)
            {
                mainCamera.localPosition = Vector3.Lerp(mainCamera.localPosition, targetRootPos, transitionSpeed * Time.deltaTime);
                mainCamera.localRotation = Quaternion.Slerp(mainCamera.localRotation, camRotHelper, transitionSpeed * Time.deltaTime);
            }
        }

        public void GetOut() { TransitionOutHelper(); }
        public void GetIn() { TransitionInHelper(); }

        void TransitionOutHelper()
        {
            onExit.Invoke();
            osCG.interactable = false;
            osCG.blocksRaycasts = false;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            osCanvas.renderMode = RenderMode.ScreenSpaceCamera;
            projectorCam.targetTexture = uiRT;
            projectorCam.gameObject.SetActive(true);
            enableCameraOut = true;

            StopCoroutine("TransitionIn");
            StopCoroutine("TransitionOut");
            StartCoroutine("TransitionOut");
        }

        IEnumerator TransitionOut()
        {
            yield return new WaitForSeconds(transitionOutTimer);

            enableCameraIn = false;
            enableCameraOut = false;
            isInSystem = false;
            isInTrigger = true;

            onExitEnd.Invoke();
        }

        void TransitionInHelper()
        {
            if (isInTrigger == false || isInSystem == true)
                return;

            onEnter.Invoke();

            osCG.interactable = true;
            osCG.blocksRaycasts = true;
            osCanvas.gameObject.SetActive(true);

            if (transitionMode == TransitionMode.Dynamic) { targetRootPos = mainCamera.position; }
            else if (transitionMode == TransitionMode.Static && takenStaticRootPos == false)
            {
                targetRootPos = mainCamera.localPosition;
                takenStaticRootPos = true;
            }

            camRotHelper = mainCamera.localRotation;

            enableCameraOut = false;
            enableCameraIn = true;
            isInTrigger = false;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            StopCoroutine("TransitionIn");
            StopCoroutine("TransitionOut");
            StartCoroutine("TransitionIn");
        }

        IEnumerator TransitionIn()
        {
            yield return new WaitForSeconds(transitionInTimer);

            osCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            projectorCam.gameObject.SetActive(false);

            enableCameraIn = false;
            enableCameraOut = false;
            isInSystem = true;

            onEnterEnd.Invoke();
        }


        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == playerTag)
            {
                isInTrigger = true;
                if (autoGetIn == true) { TransitionInHelper(); }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == playerTag) { isInTrigger = false; }
        }
    }
}