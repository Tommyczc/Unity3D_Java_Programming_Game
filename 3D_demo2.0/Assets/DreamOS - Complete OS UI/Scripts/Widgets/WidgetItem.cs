using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace Michsky.DreamOS
{
    [RequireComponent(typeof(Animator))]
    public class WidgetItem : MonoBehaviour, IEndDragHandler
    {
        [Header("Resources")]
        public Animator widgetAnimator;

        [Header("Settings")]
        public bool isOn;

        [Header("Events")]
        public UnityEvent enabledEvents;
        public UnityEvent disabledEvents;

        float widgetPosX;
        float widgetPosY;
        bool firstTime = true;

        void Start()
        {
            PrepareWidget();
            firstTime = false;
        }

        void OnEnable()
        {
            if (firstTime == true)
                return;

            if (PlayerPrefs.GetString("Widget" + gameObject.name) == "enabled") { widgetAnimator.Play("Widget In"); }
            else { widgetAnimator.Play("Widget Out"); }
        }

        public void PrepareWidget()
        {
            if (widgetAnimator == null) { widgetAnimator = gameObject.GetComponent<Animator>(); }

            if (!PlayerPrefs.HasKey("Widget" + gameObject.name) && isOn == true)
            {
                AlignToCenter();

                widgetPosX = gameObject.transform.position.x;
                widgetPosY = gameObject.transform.position.y;

                PlayerPrefs.SetFloat("Widget" + gameObject.name + "PosX", widgetPosX);
                PlayerPrefs.SetFloat("Widget" + gameObject.name + "PosY", widgetPosY);
                PlayerPrefs.SetString("Widget" + gameObject.name, "enabled");
              
                widgetAnimator.Play("Widget In");
                enabledEvents.Invoke();

                isOn = true;
            }

            else if (!PlayerPrefs.HasKey("Widget" + gameObject.name) && isOn == false)
            {
                AlignToCenter();

                widgetPosX = gameObject.transform.position.x;
                widgetPosY = gameObject.transform.position.y;

                PlayerPrefs.SetFloat("Widget" + gameObject.name + "PosX", widgetPosX);
                PlayerPrefs.SetFloat("Widget" + gameObject.name + "PosY", widgetPosY);
                PlayerPrefs.SetString("Widget" + gameObject.name, "disabled");

                widgetAnimator.Play("Widget Out");
                disabledEvents.Invoke();
             
                isOn = false;
            }

            else if (PlayerPrefs.GetString("Widget" + gameObject.name) == "enabled")
            {
                widgetPosX = PlayerPrefs.GetFloat("Widget" + gameObject.name + "PosX");
                widgetPosY = PlayerPrefs.GetFloat("Widget" + gameObject.name + "PosY");
                
                gameObject.transform.position = new Vector3(widgetPosX, widgetPosY, 0);
                widgetAnimator.Play("Widget In");
                enabledEvents.Invoke();
                
                isOn = true;
            }

            else if (PlayerPrefs.GetString("Widget" + gameObject.name) == "disabled")
            {
                widgetAnimator.Play("Widget Out");
                disabledEvents.Invoke();
                
                isOn = false;
            }
        }

        public void EnableWidget()
        {
            StopCoroutine("DisableObject");
            gameObject.SetActive(true);
          
            widgetPosX = PlayerPrefs.GetFloat("Widget" + gameObject.name + "PosX");
            widgetPosY = PlayerPrefs.GetFloat("Widget" + gameObject.name + "PosY");
            PlayerPrefs.SetString("Widget" + gameObject.name, "enabled");

            gameObject.transform.position = new Vector3(widgetPosX, widgetPosY, 0);
            widgetAnimator.Play("Widget In");
            enabledEvents.Invoke();

            isOn = true;
        }

        public void DisableWidget()
        {
            PlayerPrefs.SetString("Widget" + gameObject.name, "disabled");

            widgetAnimator.Play("Widget Out");
            disabledEvents.Invoke();
            StartCoroutine("DisableObject");

            isOn = false;
        }

        public void AlignToCenter()
        {
            gameObject.transform.localPosition = new Vector3(0, 0, 0);

            widgetPosX = gameObject.transform.position.x;
            widgetPosY = gameObject.transform.position.y;

            PlayerPrefs.SetFloat("Widget" + gameObject.name + "PosX", widgetPosX);
            PlayerPrefs.SetFloat("Widget" + gameObject.name + "PosY", widgetPosY);
        }

        public void OnEndDrag(PointerEventData data)
        {
            widgetPosX = gameObject.transform.position.x;
            widgetPosY = gameObject.transform.position.y;

            PlayerPrefs.SetFloat("Widget" + gameObject.name + "PosX", widgetPosX);
            PlayerPrefs.SetFloat("Widget" + gameObject.name + "PosY", widgetPosY);

            gameObject.transform.position = new Vector3(widgetPosX, widgetPosY, 0);
        }

        IEnumerator DisableObject()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
            StopCoroutine("DisableObject");
        }
    }
}