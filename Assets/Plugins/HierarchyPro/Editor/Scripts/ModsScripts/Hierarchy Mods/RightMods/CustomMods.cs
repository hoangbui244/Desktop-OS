﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;




namespace EMX.CustomizationHierarchy.ExtensionInterface_CustomRightMod
{

    public abstract class Slot_1 : EMX.HierarchyPlugin.Editor.CustomHierarchyMod { }
    public abstract class Slot_2 : EMX.HierarchyPlugin.Editor.CustomHierarchyMod { }
    public abstract class Slot_3 : EMX.HierarchyPlugin.Editor.CustomHierarchyMod { }
    public abstract class Slot_4 : EMX.HierarchyPlugin.Editor.CustomHierarchyMod { }
    public abstract class Slot_5 : EMX.HierarchyPlugin.Editor.CustomHierarchyMod { }
    public abstract class Slot_6 : EMX.HierarchyPlugin.Editor.CustomHierarchyMod { }
    public abstract class Slot_7 : EMX.HierarchyPlugin.Editor.CustomHierarchyMod { }
    public abstract class Slot_8 : EMX.HierarchyPlugin.Editor.CustomHierarchyMod { }
    public abstract class Slot_9 : EMX.HierarchyPlugin.Editor.CustomHierarchyMod { }
}


namespace EMX.HierarchyPlugin.Editor
{

    public abstract class CustomHierarchyMod : IEquatable<CustomHierarchyMod>
    {
        // [InitializeOnLoadMethod]
        // internal void InitializeMod( ) { (new CustomHierarchyMod()).AssignMod(); }


        public CustomHierarchyMod()
        {
            var T = GetType();
            var last = T.BaseType.Name.ToCharArray().Last();
            var index = int.Parse(last.ToString()) - 1;
            currentID = index;
            Editor.Mods.RightModsManager.RegistrateRightCustomMod( this, index );
        }

        public abstract string NameOfModule { get; }
        public abstract void Draw( Rect drawRect, GameObject o, bool mouseHover );
        public abstract string ToString( GameObject o );

        /// <summary>Opens window with input field.
        /// </summary>
        /// <param name="defaultValue">value that will be in the input field.</param>
        /// <param name="OnValueChanged">will be invoked when the value is changed.</param>
        public static void
            SHOW_IntInput( string windowName, int defaultValue, Action<int> OnValueChanged )
        {
            m_OpenIntInput_W( windowName, defaultValue, OnValueChanged );
        }

        /// <summary>Opens window with input field.
        /// </summary>
        /// <param name="defaultValue">value that will be in the input field.</param>
        /// <param name="OnValueChanged">will be invoked when the value is changed.</param>
        public static void
            SHOW_IntInput( int defaultValue, Action<int> OnValueChanged )
        {
            m_OpenIntInput( defaultValue, OnValueChanged );
        }

        /// <summary>Opens window with input field.
        /// </summary>
        /// <param name="defaultValue">value that will be in the input field.</param>
        /// <param name="OnValueChanged">will be invoked when the value is changed.</param>
        public static void
            SHOW_StringInput( string windowName, string defaultValue, Action<string> OnValueChanged )
        {
            m_OpenStringInput_W( windowName, defaultValue, OnValueChanged );
        }

        /// <summary>Opens window with input field.
        /// </summary>
        /// <param name="defaultValue">value that will be in the input field.</param>
        /// <param name="OnValueChanged">will be invoked when the value is changed.</param>
        public static void
            SHOW_StringInput( string defaultValue, Action<string> OnValueChanged )
        {
            m_OpenStringInput( defaultValue, OnValueChanged );
        }

        /// <summary>Opens the drop-down menu with the specified parameters.
        /// </summary>
        /// <param name="defaultIndex">designated index of item.</param>
        /// <param name="Items">names of menu items.</param>
        /// <param name="OnIndexChanged">will be invoked when the value is changed.</param>
        /// <param name="OnItemAdded">will be invoked when the value is added.
        /// if OnItemAdded == null then 'add new' menu item won't available.</param>
        public static void
            SHOW_DropDownMenu( int defaultIndex, string[] Items, Action<int> OnIndexChanged, Action<string> OnItemAdded = null )
        {
            m_OpenDropDownMenu( defaultIndex, Items, OnIndexChanged, OnItemAdded );
        }

        public static void ChangeCursorAccordingWithSettings( Rect rect )
        {
            m_ChangeCursorAccordingWithSettings( rect );
        }


        private static GUIContent emptyContent = new GUIContent();
        public static bool BUTTON_AccordingWithSettings( Rect rect, GUIContent content )
        {
            return m_BUTTON_AccordingWithSettings2( rect, content, string.IsNullOrEmpty( content.text ) || content.text == "-", null );
        }
        public static bool BUTTON_AccordingWithSettings( Rect rect, string content, GUIStyle style, bool overrideHasContentFlag )
        {
            emptyContent.text = emptyContent.tooltip = content;
            return m_BUTTON_AccordingWithSettings2( rect, emptyContent, overrideHasContentFlag, style );
        }
        public static bool BUTTON_AccordingWithSettings( Rect rect, GUIContent content, GUIStyle style )
        {
            return m_BUTTON_AccordingWithSettings2( rect, content, string.IsNullOrEmpty( content.text ) || content.text == "-", style );
        }
        public static bool BUTTON_AccordingWithSettings( Rect rect, GUIContent content, GUIStyle style, bool overrideHasContentFlag )
        {
            return m_BUTTON_AccordingWithSettings2( rect, content, overrideHasContentFlag, style );
        }
        public static bool BUTTON_AccordingWithSettings( Rect rect, string content )
        {
            return m_BUTTON_AccordingWithSettings( rect, content, string.IsNullOrEmpty( content ) || content == "-" );
        }
        public static bool BUTTON_AccordingWithSettings( Rect rect, string content, bool overrideHasContentFlag )
        {
            return m_BUTTON_AccordingWithSettings( rect, content, overrideHasContentFlag );
        }

        internal static Func<Rect, GUIContent, bool, GUIStyle, bool> m_BUTTON_AccordingWithSettings2;
        internal static Func<Rect, string, bool, bool> m_BUTTON_AccordingWithSettings;
        internal static Action<Rect> m_ChangeCursorAccordingWithSettings;
        internal static Action<string, int, Action<int>> m_OpenIntInput_W;
        internal static Action<int, Action<int>> m_OpenIntInput;
        internal static Action<string, string, Action<string>> m_OpenStringInput_W;
        internal static Action<string, Action<string>> m_OpenStringInput;

        internal static Action<int, string[], Action<int>, Action<string>> m_OpenDropDownMenu;
        // bool CreateDefaultUndo();


        int currentID;
        public override bool Equals( object obj ) { return currentID == ((CustomHierarchyMod)obj).currentID; }
        public bool Equals( CustomHierarchyMod obj ) { return currentID == obj.currentID; }
        public override int GetHashCode() { return currentID; }
    }


    class M_UserModulesRoot_Slot1 : Mod_UserModulesRoot
    {
        public
            M_UserModulesRoot_Slot1( int restWidth, int sibbildPos, bool enable, PluginInstance dadapter ) : base( restWidth, sibbildPos, enable, dadapter ) { }
    }

    class M_UserModulesRoot_Slot2 : Mod_UserModulesRoot
    {
        public
            M_UserModulesRoot_Slot2( int restWidth, int sibbildPos, bool enable, PluginInstance dadapter ) : base( restWidth, sibbildPos, enable, dadapter ) { }
    }

    class M_UserModulesRoot_Slot3 : Mod_UserModulesRoot
    {
        public
            M_UserModulesRoot_Slot3( int restWidth, int sibbildPos, bool enable, PluginInstance dadapter ) : base( restWidth, sibbildPos, enable, dadapter ) { }
    }

    class M_UserModulesRoot_Slot4 : Mod_UserModulesRoot
    {
        public
            M_UserModulesRoot_Slot4( int restWidth, int sibbildPos, bool enable, PluginInstance dadapter ) : base( restWidth, sibbildPos, enable, dadapter ) { }
    }

    class M_UserModulesRoot_Slot5 : Mod_UserModulesRoot
    {
        public
            M_UserModulesRoot_Slot5( int restWidth, int sibbildPos, bool enable, PluginInstance dadapter ) : base( restWidth, sibbildPos, enable, dadapter ) { }
    }

    class M_UserModulesRoot_Slot6 : Mod_UserModulesRoot
    {
        public
            M_UserModulesRoot_Slot6( int restWidth, int sibbildPos, bool enable, PluginInstance dadapter ) : base( restWidth, sibbildPos, enable, dadapter ) { }
    }

    class M_UserModulesRoot_Slot7 : Mod_UserModulesRoot
    {
        public
            M_UserModulesRoot_Slot7( int restWidth, int sibbildPos, bool enable, PluginInstance dadapter ) : base( restWidth, sibbildPos, enable, dadapter ) { }
    }

    class M_UserModulesRoot_Slot8 : Mod_UserModulesRoot
    {
        public
            M_UserModulesRoot_Slot8( int restWidth, int sibbildPos, bool enable, PluginInstance dadapter ) : base( restWidth, sibbildPos, enable, dadapter ) { }
    }

    class M_UserModulesRoot_Slot9 : Mod_UserModulesRoot
    {
        public
            M_UserModulesRoot_Slot9( int restWidth, int sibbildPos, bool enable, PluginInstance dadapter ) : base( restWidth, sibbildPos, enable, dadapter ) { }
    }
}

