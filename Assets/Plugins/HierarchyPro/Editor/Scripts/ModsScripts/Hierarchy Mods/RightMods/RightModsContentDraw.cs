﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using System.Runtime.InteropServices;

namespace EMX.HierarchyPlugin.Editor.Mods
{


    partial class RightModsManager
    {

        Color oldG1;
        Color oldG2;
        void DrawModulesContent()
        {
            //??
            //button.normal.textColor *= o.Active() ? Color.white : new Color( 1, 1, 1, 0.5f );



            if ( p.EVENT.type == EventType.Layout ) return;


            var HIDE_MODULES = p.modsController.rightModsManager.CheckSpecialButtonIfRightHidingEnabled();
            if ( HIDE_MODULES ) return;



            // var width = selectionRect.x + selectionRect.width;

            var sel = p.par_e.RIGHT_SHOWMODS_ONLY_IFHOVER ? p.ha.IsSelected(p.o.id) : false;

            if ( p.par_e.RIGHT_SHOWMODS_ONLY_IFHOVER && !p.par_e.RIGHT_USE_HIDE_ISTEAD_LOCK && !p.EVENT.alt && p.hashoveredItem && p.hoverID != p.o.id && !sel ) return;



            for ( int i = 0; i < p.window.modsCount; i++ )
            {

                // foreach ( var drawModule in modulesOrdered )
                //SKIP

                if ( !p.window.tempModsData[ i ].enabled ) break;/// throw new Exception( "mod disable content" );

                var targetMod = p.window.tempModsData[i].targetMod;

                /*  currentRect.width = Math.Max( drawModule.width, defWDTH );
                  currentRect.x -= currentRect.width;
                  var MIN = par.PADDING_LEFT;
                  if ( w != null && MIN > width - RIGHTPAD ) MIN = width - RIGHTPAD;
                  bool fade = (currentRect.x < MIN);
                  currentRect = ClipMINSizeRect( currentRect, width );
                  if ( currentRect.width < 2 ) continue;*/
                //  FadeRect(currentRect, par.HEADER_OPACITY ?? DefaultBGOpacity);

                var headerRect = p.window.tempModsData[i].rect;
                var fade = p.window.tempModsData[i].fade_narrow;

                if ( fade )
                {
                    oldG1 = GUI.color;
                    oldG2 = GUI.contentColor;
                    var c = GUI.color;
                    var t = headerRect.width / targetMod.savedData.width * 2;
                    c.a = Mathf.SmoothStep( 0, 1, t );
                    GUI.color = c;
                    c = GUI.contentColor;
                    c.a = Mathf.SmoothStep( 0, 1, t );
                    GUI.contentColor = c;
                }


                //DRAG
                //if ( p.window.CurrentRectContainsKey( w, drawModule ) ) drawRect.x = CurrentRect( w, drawModule ).x;
                headerRect.y = p.fullLineRect.y;

                targetMod.callFromExternal_objects = null;
                targetMod.drawRect = headerRect;
                if ( p.EVENT.type == EventType.Repaint && !p.baked_HARD_BAKE_ENABLED ) p.FADE_IF_NO_BAKE( headerRect );
                targetMod.Draw();

                if ( fade )
                {
                    GUI.color = oldG1;
                    GUI.contentColor = oldG2;
                }

				//if ( targetMod.CURRENT_STACK != null ) throw new Exception( "Cache not finalizing " + targetMod );
				if ( targetMod.CURRENT_STACK != null ) Root.TemperaryDisableThePlugin_FromCache();

			} // foreach

		}


        GUIContent _tempContent = new GUIContent();
        internal Rect DrawCursorRect( ref Rect rect, GUIStyle style, string self_ContentInstance )
        {
            _tempContent.text = self_ContentInstance == "" ? "-" : self_ContentInstance;
            return DrawCursorRect( ref rect, style, _tempContent );
        }
        internal Rect DrawCursorRect( ref Rect rect, GUIStyle style, GUIContent self_ContentInstance )
        {
            if ( p.modsController.rightModsManager.baked_SHRINK_BUTTONS )
            {
                var size = style.CalcSize( (self_ContentInstance) );
                var _r = rect;
                switch ( style.alignment )
                {
                    case TextAnchor.UpperLeft:
                    case TextAnchor.MiddleLeft:
                    case TextAnchor.LowerLeft:
                        _r.width = size.x;
                        if ( _r.width > rect.width ) _r.width = rect.width;
                        break;
                    case TextAnchor.LowerCenter:
                    case TextAnchor.MiddleCenter:
                    case TextAnchor.UpperCenter:
                        _r.x += (_r.width - size.x) / 2;
                        _r.width = size.x;
                        if ( _r.x < rect.x ) _r.x = rect.x;
                        if ( _r.x + _r.width > rect.x + rect.width ) _r.width = rect.x + rect.width - _r.x;
                        break;
                    case TextAnchor.UpperRight:
                    case TextAnchor.MiddleRight:
                    case TextAnchor.LowerRight:
                        _r.x += _r.width - size.x;
                        _r.width = size.x;
                        if ( _r.x < rect.x ) _r.x = rect.x;
                        if ( _r.x + _r.width > rect.x + rect.width ) _r.width = rect.x + rect.width - _r.x;
                        break;
                }

               // if ( p.modsController.rightModsManager.baked_CHANGE_CURSOR )EditorGUIUtility.AddCursorRect( _r, MouseCursor.Link );

                return _r;
            }

            //if ( p.modsController.rightModsManager.baked_CHANGE_CURSOR ) EditorGUIUtility.AddCursorRect( rect, MouseCursor.Link );

            return rect;
        }

     




        //VERTICAL LINES
        Color32 S_COL1 = new Color32(8, 8, 8, 50);
        Color32 S_COL2 = new Color32(132, 132, 132, 50);

        Color32 S_COL3 = new Color32(8, 8, 8, 20);
        Color32 S_COL4 = new Color32(255, 255, 255, 50);
        Rect vR;
        void DrawVerticalLines()
        {

            if ( !p.par_e.RIGHTDOCK_DRAW_VERTICAL_SEPARATORS ) return;

            vR.y = p._first_FullLineRect.y;
            vR.height = p._last_FullLineRect.y + p._last_FullLineRect.height - p._first_FullLineRect.y;
            vR.width = 1;

            for ( int i = 0; i < p.modsController.rightModsManager.rightMods.Length; i++ )
            {

                // foreach ( var drawModule in modulesOrdered )
                //SKIP
                /* drawRect.width = 2;

            GUI.DrawTexture(drawRect, GetIcon("SEPARATOR"));
            GUI.DrawTexture(drawRect, GetIcon("SEPARATOR")); */
                // Adapter.DrawRect( drawRect, S_COL3 );
                // Adapter.DrawRect( drawRect, S_COL4 );
                if ( !p.window.tempModsData[ i ].enabled ) break;

                vR.x = p.window.tempModsData[ i ].rect.x;


                if ( !EditorGUIUtility.isProSkin )
                {
                    vR.x -= 1;
                    EditorGUI.DrawRect( vR, S_COL3 );
                    vR.x += 1;
                    EditorGUI.DrawRect( vR, S_COL4 );
                }
                else
                {
                    vR.x -= 1;
                    EditorGUI.DrawRect( vR, S_COL1 );
                    vR.x += 1;
                    EditorGUI.DrawRect( vR, S_COL2 );
                }

                if ( i == 0 )
                {
                    vR.x += p.window.tempModsData[ i ].rect.width;
                    if ( !EditorGUIUtility.isProSkin )
                    {
                        vR.x -= 1;
                        EditorGUI.DrawRect( vR, S_COL3 );
                        vR.x += 1;
                        EditorGUI.DrawRect( vR, S_COL4 );
                    }
                    else
                    {
                        vR.x -= 1;
                        EditorGUI.DrawRect( vR, S_COL1 );
                        vR.x += 1;
                        EditorGUI.DrawRect( vR, S_COL2 );
                    }
                }
            }


        }
    }
}
