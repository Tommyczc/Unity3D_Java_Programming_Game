using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Michsky.DreamOS
{
    public class BootManager : MonoBehaviour
    {
        // Events
        public UnityEvent onBootStart;
        public UnityEvent eventsAfterBoot;

        // Resources
        public Animator bootAnimator;
        public UserManager userManager;

        // Settings
        [Range(0, 30)] public float bootTime = 3f;

        void Start()
        {
            StartCoroutine("BootEventStart");
        }

        public void InvokeEvents()
        {
            bootAnimator.gameObject.SetActive(true);
            bootAnimator.Play("Boot Start");
            StartCoroutine("BootEventStart");
        }

        IEnumerator BootEventStart()
        {
            yield return new WaitForSeconds(bootTime);

            if (bootAnimator.gameObject.activeSelf == true) { bootAnimator.Play("Boot Out"); }
            userManager.lockScreen.gameObject.SetActive(true);
            eventsAfterBoot.Invoke();

            StopCoroutine("BootEventStart");
            StartCoroutine("DisableBootScreenHelper");
        }

        public void start_boot() { Debug.Log("hello com");  StartCoroutine("BootEventStart"); }

        IEnumerator DisableBootScreenHelper()
        {
            yield return new WaitForSeconds(1f);
            bootAnimator.gameObject.SetActive(false);
        }
    }
}