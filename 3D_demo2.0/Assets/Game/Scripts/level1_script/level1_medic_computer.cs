using javaCompiler;
using Michsky.UI.MTP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace level.one
{
    public class level1_medic_computer : MonoBehaviour
    {
        // Start is called before the first frame update
        private GameObject computer_function;
        public string theText;
        private string room="medic";
        public GameObject canvas;
        private Animator anim;
        private GameObject textObject;
        void Start()
        {
            computer_function = GameObject.Find("Computer_functions");

            anim = canvas.GetComponent<Animator>();
            anim.SetInteger("UI_open", 0);
            canvas.SetActive(false);


            Transform[] myTransforms = canvas.gameObject.GetComponentsInChildren<Transform>();
            foreach (var child in myTransforms)
            {
                if (child.name == "Text")
                {
                    textObject = child.gameObject;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnTriggerEnter(Collider other)
        {
            if (textObject != null)
            {
                if (textObject.GetComponent<TextItem>().text != theText)
                {
                    textObject.GetComponent<TextItem>().text = theText;
                    textObject.GetComponent<TextItem>().UpdateText();
                }
                canvas.SetActive(true);
                anim.SetInteger("UI_open", 1);
            }

        }

        private void OnTriggerStay(Collider other)
        {
            if (Input.GetKeyDown(KeyCode.F) && computer_function.gameObject.GetComponent<level0_computer_functions>().computer_open==false)
            {
                testJavaCompiler.theLocation=room;
                computer_function.GetComponent<level0_computer_functions>().boot_computer();
                anim.SetInteger("UI_open", 2);
                
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (textObject != null)
            {
                if (textObject.GetComponent<TextItem>().text != theText)
                {
                    textObject.GetComponent<TextItem>().text = theText;
                    textObject.GetComponent<TextItem>().UpdateText();
                }
                if (anim.GetInteger("UI_open") != 2)
                {
                    anim.SetInteger("UI_open", 2);
                }

                //canvas.SetActive(false);

            }
        }
    }
}
