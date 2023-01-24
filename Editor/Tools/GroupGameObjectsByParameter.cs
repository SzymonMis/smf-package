using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SMF.Editor.Tools
{
    public class GroupGameObjectsByParameter : EditorWindow
    {
        public string NameCriteria;
        public Component CommonComponent;

        private GroupingCriteria groupingMethod = GroupingCriteria.FirstPartOfName;
        private GroupingCriteria previousGroupingMethod;
        private List<GameObject> objectsMeetingCriteria;

        [MenuItem(itemName: "SMF Tools/Group GameObjects")]
        public static GroupGameObjectsByParameter Open()
        {
            return EditorWindow.GetWindow<GroupGameObjectsByParameter>("Group GameObjects");
        }

        private void OnGUI()
        {      
            GUILayout.BeginVertical();
            GUILayout.Space(10);
            {
                previousGroupingMethod = groupingMethod;
                groupingMethod = (GroupingCriteria)EditorGUILayout.EnumPopup("Grouping Criteria", groupingMethod);

                GUILayout.BeginHorizontal();
                {
                    switch (groupingMethod)
                    {
                        case GroupingCriteria.FirstPartOfName:
                            GUILayout.Label("Common Name Prefix");
                            NameCriteria = GUILayout.TextField(NameCriteria);
                            break;

                        case GroupingCriteria.LastPartOfName:
                            GUILayout.Label("Common Name Postfix");
                            NameCriteria = GUILayout.TextField(NameCriteria);
                            break;

                        case GroupingCriteria.AnyPartOfName:
                            GUILayout.Label("Common Name Part");
                            NameCriteria = GUILayout.TextField(NameCriteria);
                            break;

                        case GroupingCriteria.Script:
                            GUILayout.Label("uninmplenmentedtdd");
                            break;
                    }
                }
                GUILayout.EndHorizontal();

                if (groupingMethod != previousGroupingMethod)
                {
                    objectsMeetingCriteria = null;
                }
            }
            GUILayout.EndVertical();
            GUILayout.Space(10);

            //GUILayout.BeginVertical();
            {
                if (GUILayout.Button("Find Objects Meeting Criteria"))
                {
                    FindObjectsMeetingCriteria();
                }

                if (objectsMeetingCriteria != null)
                {
                    GUILayout.Box("Objects meeting criteria: " + objectsMeetingCriteria.Count);

                    if (objectsMeetingCriteria.Count == 0)
                    {
                        return;
                    }

                    if (GUILayout.Button("Combine Under Parent"))
                    {
                        CombineUnderCommonParent();
                    }
                }
                else
                {
                    GUILayout.Box("string can't be empty and must be atleast 3 characters long");
                }
            }
            //GUILayout.EndVertical();
        }

        private void FindObjectsMeetingCriteria()
        {
            if (ValidateString(NameCriteria) == false)
            {
                objectsMeetingCriteria = null;
                return;
            }

            objectsMeetingCriteria = new List<GameObject>();
            GameObject[] allGameObjects = FindObjectsOfType<GameObject>(true);
            List<GameObject> allGameObjectsList = new List<GameObject>();

            switch (groupingMethod)
            {
                case GroupingCriteria.FirstPartOfName:
                    allGameObjectsList = new List<GameObject>(allGameObjects);
                    objectsMeetingCriteria = allGameObjectsList.FindAll(x => x.name.StartsWith(NameCriteria));
                    break;

                case GroupingCriteria.LastPartOfName:                    
                    allGameObjectsList = new List<GameObject>(allGameObjects);
                    objectsMeetingCriteria = allGameObjectsList.FindAll(x => x.name.EndsWith(NameCriteria));
                    break;

                case GroupingCriteria.AnyPartOfName:
                    allGameObjectsList = new List<GameObject>(allGameObjects);
                    objectsMeetingCriteria = allGameObjectsList.FindAll(x => x.name.Contains(NameCriteria));
                    break;

                case GroupingCriteria.Script:
                    break;
            }
        }

        private bool ValidateString(string text)
        {
            if (text == string.Empty || text.Length < 3)
                return false;
            else 
                return true;
        }

        private void CombineUnderCommonParent()
        {
            GameObject commonParent = new GameObject();
            commonParent.transform.position = Vector3.zero;
            commonParent.name = "Objects gropued by " + groupingMethod.ToString() + " = " + NameCriteria;

            foreach (var go in objectsMeetingCriteria)
            {
                go.transform.parent = commonParent.transform;
            }
        }
    }

    public enum GroupingCriteria
    {
        FirstPartOfName,
        LastPartOfName,
        AnyPartOfName,
        Script
    }
}
