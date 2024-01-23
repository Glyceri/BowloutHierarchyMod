using BowloutModManager.BowloutMod;
using BowloutModManager.BowloutMod.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BowloutHierarchyMod
{
    public class BowloutHierarchyMod : IBowloutMod
    {
        public static BowloutHierarchyMod Instance;

        public string Name => "Bowlout Hierarchy Mod";

        public Version Version => new Version(1, 0, 0, 0);

        public string Description => "A Hierarchy for Bowlout.";

        public IBowloutConfiguration Configuration { get; private set; }
        public BowloutHierarchyModConfiguration SampleModConfiguration => (BowloutHierarchyModConfiguration)Configuration;

        public bool Enabled { get; set; }

        public void OnSetup()
        {
            Instance = this;
            Configuration = this.GetConfiguration<BowloutHierarchyModConfiguration>() ?? new BowloutHierarchyModConfiguration();
            this.SaveConfiguration(Configuration);
          
        }

        public void Dispose()
        {
            
        }

        public void OnEnable()
        {

        }

        public void OnDisable()
        {
            
        }

        public void OnUpdate()
        {
            
        }

        public void OnFixedUpdate()
        {
            
        }

        public void OnLateUpdate()
        {

        }

        bool hierarchyActive = true;
        public void OnGUI()
        {
            GUILayout.Space(10); 
            GUI.skin.button.alignment = TextAnchor.MiddleCenter;
            if (GUILayout.Button("Bowlout Hierarchy"))
            {
                hierarchyActive = !hierarchyActive;
            }
            if (!hierarchyActive) return;
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            Scene ActiveScene = SceneManager.GetActiveScene();
            if (ActiveScene == null) return;
            GameObject[] gObjs = ActiveScene.GetRootGameObjects();
            foreach (GameObject obj in gObjs)
                RecursiveGameObject(obj, 0);
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.Label("          ");
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            if (activeObject != null) 
            {
                GUILayout.Box(activeObject.name, GUILayout.Width(500));
                GUILayout.Space(10);
                DrawComponents();
            }
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.Label("          ");
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            if (activeComponent != null)
            {
                GUILayout.Box(activeComponent.GetType().Name, GUILayout.Width(500));
                GUILayout.Space(10);
                DrawStuff();
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        void DrawStuff()
        {

            foreach (FieldInfo info in activeComponent.GetType().GetFields().Where(f => f.IsPublic || f.GetCustomAttribute<SerializeField>() != null))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box(info.Name, GUILayout.Width(250));
                object value = info.GetValue(activeComponent);
                GUILayout.Box($"{value}", GUILayout.Width(250));
                GUILayout.EndHorizontal();
            }
        }

        void DrawComponents()
        {
            Component[] components = activeObject.GetComponents<Component>();
            foreach(Component component in components)
            {
                if (GUILayout.Button(component.GetType().Name))
                {
                    if (activeComponent == component) activeComponent = null;
                    else activeComponent = component;
                }
            }
        }

        List<GameObject> expandedList = new List<GameObject>();

        GameObject activeObject = null;
        Component activeComponent = null;

        void RecursiveGameObject(GameObject gObj, int indent)
        {
            int localIndent = indent;
            GUI.skin.button.alignment = TextAnchor.MiddleLeft;
            GUI.skin.box.alignment = TextAnchor.MiddleLeft;
            GUILayout.BeginHorizontal();
            GUILayout.Space(localIndent * 12);
            bool hasChildren = gObj.transform.childCount > 0;
            if (hasChildren)
            {
                if (GUILayout.Button(gObj.name, GUILayout.Width(480 - (indent * 12))))
                {
                    if (expandedList.Contains(gObj)) expandedList.Remove(gObj);
                    else expandedList.Add(gObj);
                }
            }
            else GUILayout.Box(gObj.name, GUILayout.Width(480 - (indent * 12)));

            if (GUILayout.Button(activeObject == gObj ? "<" : ">", GUILayout.Width(20)))
            {
                activeComponent = null;
                if (activeObject == gObj) activeObject = null;
                else activeObject = gObj;
            }

            GUILayout.EndHorizontal();

            if (!expandedList.Contains(gObj)) return;
            for (int i = 0; i < gObj.transform.childCount; i++)
            {
                RecursiveGameObject(gObj.transform.GetChild(i).gameObject, localIndent + 1);
            }
        }
    }
}
