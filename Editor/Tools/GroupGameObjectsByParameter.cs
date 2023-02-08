using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SMF.Editor.Tools
{
    public class GroupGameObjectsByParameter : EditorWindow
    {
        public string NameCriteria;
        public string ParentName;

        private GroupingCriteria groupingMethod = GroupingCriteria.FirstPartOfName;
        private GroupingCriteria previousGroupingMethod;
        private List<GameObject> objectsMeetingCriteria;
        private string previousNameCriteria;

        [MenuItem(itemName: "SMF Tools/Group GameObjects")]
        public static GroupGameObjectsByParameter Open()
        {
            return EditorWindow.GetWindow<GroupGameObjectsByParameter>("Group GameObjects");
        }

        private void OnGUI()
        {      
            //GUILayout.BeginVertical();
            GUILayout.Space(10);
            {
                GUILayout.BeginHorizontal();
                {
                    previousGroupingMethod = groupingMethod;
                    GUILayout.Label("Grouping Criteria", GUILayout.ExpandWidth(true));
                    groupingMethod = (GroupingCriteria)EditorGUILayout.EnumPopup(string.Empty, groupingMethod);
                    
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(10);
                GUILayout.BeginHorizontal();
                {
                    previousNameCriteria = NameCriteria;

                    switch (groupingMethod)
                    {
                        case GroupingCriteria.FirstPartOfName:
                            GUILayout.Label("Common Name Prefix");
                            NameCriteria = GUILayout.TextField(NameCriteria, GUILayout.ExpandWidth(true));
                            break;

                        case GroupingCriteria.LastPartOfName:
                            GUILayout.Label("Common Name Postfix");
                            NameCriteria = GUILayout.TextField(NameCriteria);
                            break;

                        case GroupingCriteria.AnyPartOfName:
                            GUILayout.Label("Common Name Part");
                            NameCriteria = GUILayout.TextField(NameCriteria);
                            break;
                    }
                }
                GUILayout.EndHorizontal();

                if (groupingMethod != previousGroupingMethod || NameCriteria != previousNameCriteria)
                {
                    objectsMeetingCriteria = null;
                }

                if (ValidateString(NameCriteria) == false)
                {
                    GUILayout.Box("string can't be empty and must be atleast 3 characters long");
                    return;
                }
            }
            //GUILayout.EndVertical();
            GUILayout.Space(10);

            //GUILayout.BeginVertical();
            {
                if (GUILayout.Button("Find Objects Meeting Criteria"))
                {
                    FindObjectsMeetingCriteria();
                }

                if (objectsMeetingCriteria != null)
                {
                    GUILayout.Box("Objects meeting criteria found: " + objectsMeetingCriteria.Count);
                    GUILayout.Space(10);
                }

                if (objectsMeetingCriteria != null && objectsMeetingCriteria.Count > 0)
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.Label("Parent Name (optional)");
                        ParentName = GUILayout.TextField(ParentName);
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Space(10);

                    if (objectsMeetingCriteria.Count == 0)
                    {
                        return;
                    }

                    if (GUILayout.Button("Combine Under Parent"))
                    {
                        CombineUnderCommonParent();
                    }
                }
            }
            //GUILayout.EndVertical();
        }

        private void FindObjectsMeetingCriteria()
        {
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
            }
        }

        private bool ValidateString(string text)
        {
            if (text == null || text == string.Empty || text.Length < 3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void CombineUnderCommonParent()
        {
            var parentName = ParentName == string.Empty ? "Grouped by " + groupingMethod.ToString() + " (" + NameCriteria + ')' : ParentName;
            GameObject commonParent = GameObject.Find(parentName);

            if (commonParent == null)
            {
                commonParent = new GameObject();
                commonParent.transform.position = Vector3.zero;
                commonParent.name = parentName;
            }

            foreach (var go in objectsMeetingCriteria)
            {
                if (go == commonParent) continue;
                go.transform.parent = commonParent.transform;
            }

            Selection.activeGameObject = commonParent;
        }
    }

    public enum GroupingCriteria
    {
        FirstPartOfName,
        LastPartOfName,
        AnyPartOfName
    }
}
