﻿#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Michsky.DreamOS
{
    [CustomEditor(typeof(NetworkManager))]
    public class NetworkManagerEditor : Editor
    {
        private NetworkManager networkTarget;
        private GUISkin customSkin;
        private int currentTab;

        private void OnEnable()
        {
            networkTarget = (NetworkManager)target;

            if (EditorGUIUtility.isProSkin == true) { customSkin = (GUISkin)Resources.Load("Editor\\Glass Skin Dark"); }
            else { customSkin = (GUISkin)Resources.Load("Editor\\Glass Skin Light"); }
        }

        public override void OnInspectorGUI()
        {
            DreamOSEditorHandler.DrawComponentHeader(customSkin, "Network Top Header");

            GUIContent[] toolbarTabs = new GUIContent[3];
            toolbarTabs[0] = new GUIContent("Network List");
            toolbarTabs[1] = new GUIContent("Resources");
            toolbarTabs[2] = new GUIContent("Settings");

            currentTab = DreamOSEditorHandler.DrawTabs(currentTab, toolbarTabs, customSkin);

            if (GUILayout.Button(new GUIContent("Network List", "Network List"), customSkin.FindStyle("Tab Content")))
                currentTab = 0;
            if (GUILayout.Button(new GUIContent("Resources", "Resources"), customSkin.FindStyle("Tab Resources")))
                currentTab = 1;
            if (GUILayout.Button(new GUIContent("Settings", "Settings"), customSkin.FindStyle("Tab Settings")))
                currentTab = 2;

            GUILayout.EndHorizontal();

            var networkItems = serializedObject.FindProperty("networkItems");
            var networkListParent = serializedObject.FindProperty("networkListParent");
            var networkItem = serializedObject.FindProperty("networkItem");
            var signalWeak = serializedObject.FindProperty("signalWeak");
            var signalStrong = serializedObject.FindProperty("signalStrong");
            var signalBest = serializedObject.FindProperty("signalBest");
            var dynamicNetwork = serializedObject.FindProperty("dynamicNetwork");
            var hasConnection = serializedObject.FindProperty("hasConnection");
            var currentNetworkIndex = serializedObject.FindProperty("currentNetworkIndex");
            var wrongPassSound = serializedObject.FindProperty("wrongPassSound");
            var feedbackSource = serializedObject.FindProperty("feedbackSource");
            var defaultSpeed = serializedObject.FindProperty("defaultSpeed");

            switch (currentTab)
            {
                case 0:
                    DreamOSEditorHandler.DrawHeader(customSkin, "Content Header", 6);

                    if (networkTarget.networkItems.Count != 0)
                    {
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        GUI.enabled = false;
                        EditorGUILayout.LabelField(new GUIContent("Selected Network:"), customSkin.FindStyle("Text"), GUILayout.Width(120));
                        GUI.enabled = true;
                        EditorGUILayout.LabelField(new GUIContent(networkTarget.networkItems[currentNetworkIndex.intValue].networkTitle), customSkin.FindStyle("Text"));

                        currentNetworkIndex.intValue = EditorGUILayout.IntSlider(currentNetworkIndex.intValue, 0, networkTarget.networkItems.Count - 1);

                        GUILayout.Space(2);
                        GUILayout.EndVertical();
                    }

                    else { EditorGUILayout.HelpBox("There's no item in the list.", MessageType.Warning); }

                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    GUILayout.BeginHorizontal();
                    EditorGUI.indentLevel = 1;

                    EditorGUILayout.PropertyField(networkItems, new GUIContent("Network Items"), true);
                    networkItems.isExpanded = true;

                    EditorGUI.indentLevel = 0;
                    GUILayout.EndHorizontal();

                    if (GUILayout.Button("+  Add a new network", customSkin.button)) { networkTarget.AddNetwork(); }

                    GUILayout.EndVertical();
                    break;

                case 1:
                    DreamOSEditorHandler.DrawHeader(customSkin, "Core Header", 6);
                    DreamOSEditorHandler.DrawProperty(networkListParent, customSkin, "Network List Parent");
                    DreamOSEditorHandler.DrawProperty(networkItem, customSkin, "Network Item");
                    DreamOSEditorHandler.DrawProperty(feedbackSource, customSkin, "Feedback Source");
                    break;

                case 2:
                    DreamOSEditorHandler.DrawHeader(customSkin, "Options Header", 6);
                    dynamicNetwork.boolValue = DreamOSEditorHandler.DrawToggle(dynamicNetwork.boolValue, customSkin, "Dynamic Network");

                    if (dynamicNetwork.boolValue == true)
                    {
                        hasConnection.boolValue = DreamOSEditorHandler.DrawToggle(dynamicNetwork.boolValue, customSkin, "Has Connection");
                        DreamOSEditorHandler.DrawProperty(signalWeak, customSkin, "Weak Signal Icon");
                        DreamOSEditorHandler.DrawProperty(signalStrong, customSkin, "Strong Signal Icon");
                        DreamOSEditorHandler.DrawProperty(signalBest, customSkin, "Best Signal Icon");
                        DreamOSEditorHandler.DrawProperty(wrongPassSound, customSkin, "Wrong Pass Sound");
                    }

                    else
                    {
                        DreamOSEditorHandler.DrawProperty(defaultSpeed, customSkin, "Default Speed");

                        EditorGUILayout.HelpBox("'Dynamic Network' is disabled. There won't be any dynamic network items, " +
                            "'Default Speed' will be used instead.", MessageType.Info);
                    }

                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif