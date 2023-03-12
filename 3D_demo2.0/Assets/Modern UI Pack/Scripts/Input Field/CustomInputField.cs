using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace Michsky.DreamOS
{
    [RequireComponent(typeof(TMP_InputField))]
    [RequireComponent(typeof(Animator))]
    public class CustomInputField : MonoBehaviour
    {
        [Header("Resources")]
        public TMP_InputField inputText;
        public Animator inputFieldAnimator;

        [Header("Settings")]
        public bool processSubmit = false;

        [Header("Events")]
        public UnityEvent onSubmit;

        // Hidden variables
        private string inAnim = "In";
        private string outAnim = "Out";

        void Awake()
        {
            if (inputText == null) { inputText = gameObject.GetComponent<TMP_InputField>(); }
            if (inputFieldAnimator == null) { inputFieldAnimator = gameObject.GetComponent<Animator>(); }

            inputText.onValueChanged.AddListener(delegate { UpdateState(); });
            inputText.onSelect.AddListener(delegate { AnimateIn(); });
            inputText.onEndEdit.AddListener(delegate { AnimateOut(); });
        }

        void OnEnable()
        {
            inputText.ForceLabelUpdate();
            UpdateState();
        }

        void Update()
        {
            if (processSubmit == false)
                return;

            if (string.IsNullOrEmpty(inputText.text) == true || EventSystem.current.currentSelectedGameObject != inputText.gameObject)
                return;

#if ENABLE_LEGACY_INPUT_MANAGER
            if (Input.GetKeyDown(KeyCode.Return)) { onSubmit.Invoke(); inputText.text = ""; }
#elif ENABLE_INPUT_SYSTEM
            if (Keyboard.current.enterKey.wasPressedThisFrame) { onSubmit.Invoke(); }
#endif
        }

        public void AnimateIn()
        {
            if (inputFieldAnimator.gameObject.activeInHierarchy == true) { inputFieldAnimator.Play(inAnim); }
        }

        public void AnimateOut()
        {
            if (inputText.text.Length == 0 && inputFieldAnimator.gameObject.activeInHierarchy == true) { inputFieldAnimator.Play(outAnim); }
        }

        public void UpdateState()
        {
            if (inputText.text.Length == 0) { AnimateOut(); }
            else { AnimateIn(); }
        }
    }
}