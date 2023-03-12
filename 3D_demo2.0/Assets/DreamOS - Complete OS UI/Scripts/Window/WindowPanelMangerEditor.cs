#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(WindowPanelManager))]
    public class WindowPanelManagerEditor : Editor
    {
        private WindowPanelManager wpmTarget;
        private GUISkin customSkin;
        private int currentTab;
        string newPanelName = "New Tab";
        Sprite panelIcon;

        private void OnEnable()
        {
            wpmTarget = (WindowPanelManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = (GUISkin)Resources.Load("Editor\\Glass Skin Dark"); }
            else { customSkin = (GUISkin)Resources.Load("Editor\\Glass Skin Light"); }
        }

        public override void OnInspectorGUI()
        {
            DreamOSEditorHandler.DrawComponentHeader(customSkin, "WPM Top Header");

            GUIContent[] toolbarTabs = new GUIContent[2];
            toolbarTabs[0] = new GUIContent("Content");
            toolbarTabs[1] = new GUIContent("Settings");

            currentTab = DreamOSEditorHandler.DrawTabs(currentTab, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Chat List", "Chat List"), customSkin.FindStyle("Tab Content")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 1;

            GUILayout.EndHorizontal();

            var panels = serializedObject.FindProperty("panels");
            var currentPanelIndex = serializedObject.FindProperty("currentPanelIndex");
            var editMode = serializedObject.FindProperty("editMode");

            switch (currentTab)
            {
                case 0:
                    DreamOSEditorHandler.DrawHeader(customSkin, "Content Header", 6);
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    EditorGUI.indentLevel = 1;

                    EditorGUILayout.PropertyField(panels, new GUIContent("Panel Items"), true);
                    panels.isExpanded = true;

                    EditorGUI.indentLevel = 0;
                    GUILayout.EndHorizontal();

                    if (wpmTarget.panels.Count != 0 && wpmTarget.panels[wpmTarget.panels.Count - 1] != null
                        && wpmTarget.panels[wpmTarget.panels.Count - 1].panelObject != null
                        && wpmTarget.panels[wpmTarget.panels.Count - 1].buttonObject != null)
                    {
                        DreamOSEditorHandler.DrawHeader(customSkin, "Customization Header", 10);
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        GUILayout.BeginHorizontal();
                        
                        EditorGUILayout.LabelField(new GUIContent("Panel Name"), customSkin.FindStyle("Text"), GUILayout.Width(85));
                        newPanelName = (string)EditorGUILayout.TextField(newPanelName);
                        
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(new GUIContent("Panel Icon"), customSkin.FindStyle("Text"), GUILayout.Width(85));
                        panelIcon = (Sprite)EditorGUILayout.ObjectField(panelIcon, typeof(Sprite), true);

                        GUILayout.EndHorizontal();

                        if (GUILayout.Button("+  Create a new panel", customSkin.button))
                        {
                            GameObject panelGO = Instantiate(wpmTarget.panels[0].panelObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                            panelGO.transform.SetParent(wpmTarget.panels[0].panelObject.transform.parent, false);
                            panelGO.gameObject.name = newPanelName;

                            Transform contentGO = panelGO.transform.Find("Content");

                            foreach (Transform child in contentGO)
                                DestroyImmediate(child.gameObject);

                            try 
                            {
                                TMPro.TextMeshProUGUI panelTitleGO = panelGO.transform.Find("Title").GetComponent<TMPro.TextMeshProUGUI>(); ;
                                panelTitleGO.text = newPanelName;
                            }

                            catch { }

                            CanvasGroup tempCG = panelGO.GetComponent<CanvasGroup>();
                            tempCG.alpha = 0;

                            GameObject buttonGO = Instantiate(wpmTarget.panels[0].buttonObject, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                            buttonGO.transform.SetParent(wpmTarget.panels[0].buttonObject.transform.parent, false);
                            buttonGO.gameObject.name = newPanelName;

                            NavDrawerButton tempNDB = buttonGO.gameObject.GetComponent<NavDrawerButton>();
                            tempNDB.onClickEvents.RemoveAllListeners();
                            UnityEditor.Events.UnityEventTools.RemovePersistentListener(tempNDB.onClickEvents, 0);
                            UnityEditor.Events.UnityEventTools.AddStringPersistentListener(tempNDB.onClickEvents, new UnityEngine.Events.UnityAction<string>(wpmTarget.OpenPanel), newPanelName);

                            ButtonManager tempBM = buttonGO.GetComponent<ButtonManager>();
                            tempBM.buttonText = newPanelName;
                            tempBM.buttonIcon = panelIcon;

                            WindowPanelManager.PanelItem newPanelItem = new WindowPanelManager.PanelItem();
                            newPanelItem.panelName = newPanelName;
                            newPanelItem.panelObject = panelGO;
                            newPanelItem.buttonObject = buttonGO;
                            wpmTarget.panels.Add(newPanelItem);
                        }

                        GUILayout.EndVertical();
                    }

                    break;

                case 1:
                    DreamOSEditorHandler.DrawHeader(customSkin, "Options Header", 6);
                    editMode.boolValue = DreamOSEditorHandler.DrawToggle(editMode.boolValue, customSkin, "Edit Mode");

                    if (wpmTarget.panels.Count != 0)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);

                        EditorGUILayout.LabelField(new GUIContent("Selected Window:"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        currentPanelIndex.intValue = EditorGUILayout.IntSlider(currentPanelIndex.intValue, 0, wpmTarget.panels.Count - 1);

                        GUILayout.Space(2);
                        EditorGUILayout.LabelField(new GUIContent(wpmTarget.panels[currentPanelIndex.intValue].panelName), customSkin.FindStyle("Text"));

                        if (editMode.boolValue == true)
                        {
                            EditorGUILayout.HelpBox("While Edit Mode is enabled, you can change the visibility of window objects by changing the slider value.", MessageType.Info);

                            for (int i = 0; i < wpmTarget.panels.Count; i++)
                            {
                                if (i == currentPanelIndex.intValue)
                                    wpmTarget.panels[currentPanelIndex.intValue].panelObject.GetComponent<CanvasGroup>().alpha = 1;
                                else
                                    wpmTarget.panels[i].panelObject.GetComponent<CanvasGroup>().alpha = 0;
                            }
                        }

                        GUILayout.EndVertical();
                    }

                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif