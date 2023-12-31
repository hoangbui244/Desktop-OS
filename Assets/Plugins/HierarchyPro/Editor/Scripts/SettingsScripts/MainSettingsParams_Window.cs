﻿using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;
using UnityEditor.SceneManagement;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Diagnostics.Eventing.Reader;

namespace EMX.HierarchyPlugin.Editor.Settings
{
	class MainSettingsParams_Window : ScriptableObject
	{

	}



	[CustomEditor( typeof( MainSettingsParams_Window ) )]
	class MainSettingsParamsEditor : MainRoot
	{




		public override void OnInspectorGUI()
		{
			_GUI( (IRepaint)this );
		}
		public static void _GUI( IRepaint w )
		{

			Draw.RESET( w );

			Draw.BACK_BUTTON( w );

			// GUI.Button( Draw.R2, "  Main Hierarchy Settings", Draw.s( "preToolbar" ) );
			Draw.TOG_TIT( "Main Hierarchy Settings" );

			// GUI.Button( Draw.R, "Common Settings", Draw.s( "insertionMarker" ) );
			using ( GRO( w ).UP() )
			{



				QWE( w, p.par_e.HIER_WIN_SET, () => {
					//   using ( GRO(w).UP( 0 ) )
					{
						Draw.TOG( "Fun 'legacy' override all editor fonts size (bugs)", "USE_WHOLE_FUN_UNITY_FONT_SIZE", ( valBefore ) => {
							if ( !valBefore.AS_BOOL && EditorUtility.DisplayDialog( "" + Root.HierarchyPro + "", "Do you want to Override all editor fonts size? After when you disable it, you have to restart the editor", "Yes, I wanna try", "No, please stop!" ) ) return true;
							if ( valBefore.AS_BOOL && EditorUtility.DisplayDialog( "" + Root.HierarchyPro + "", "Do you want to disable fun custom editor fonts size? You would also need to restart the editor to fully restore fonts sizes", "Yes", "No" ) ) return true;
							return false;
						},
					( valAfter ) => {
						p.modsController.REBUILD_PLUGINS();
						if ( !valAfter.AS_BOOL ) FunEditorFontsModification.Modificate( 12 );
					}, EnableRed: false );
						using ( ENABLE( w ).USE( Draw.GetSetter( "USE_WHOLE_FUN_UNITY_FONT_SIZE" ) ) ) Draw.FIELD( "Fun unity font size '12'", "WHOLE_FUN_UNITY_FONT_SIZE", 4, 60 );
					}
					Draw.HRx05( Draw.R );
					Draw.COLOR( "Buttons tap color", /*-*/"BUTTON_TAP_COLOR" );

					Draw.HRx05( Draw.R );
					//	Draw.Sp( 10 );
					{
						GUI.Label( Draw.R, Draw.CONT( "Quick settings presets (Copy settings above to os text buffer):" ) );
						var R = Draw.R15;
						R.width /= 2;
						if ( GUI.Button( R, Draw.CONT( "Copy", @"Copy settings above (and background colors) to os buffer, settings like:
- Line Height/Indent
- Labels/Icons Size
- Background Chess Colors" ) ) )
						{
							Root.p[ 0 ].par_e.CopyPresetToBuffer();
						}
						R.x += R.width;
						var e = GUI.enabled;
						GUI.enabled &= Root.p[ 0 ].par_e.PastePresetToBufferValidate();
						if ( GUI.Button( R, Draw.CONT( "Paste", @"Paste settings above (and background colors) from os buffer, settings like:
- Line Height/Indent
- Labels/Icons Size
- Background Chess Colors" ) ) )
						{
							Root.p[ 0 ].par_e.PastePresetToBuffer();
						}
						GUI.enabled = e;
						Draw.Sp( 10 );
					}

					// Draw.HRx2();









				}, () => {


					Draw.Sp( 10 );
					using ( MainRoot.GRO( w ).UP( 0 ) )
					{

						Draw.TOG_TIT( "Events:", EnableRed: true );


						//Draw.Sp( 10 );

						Draw.TOG_TIT( "Arrow Keys UP/DOWN/LEFT/RIGHT:", EnableRed: false );//GUI.Label( Draw.R, Draw.CONT( "Arrow Keys UP/DOWN/LEFT/RIGHT:" ) );




						Draw.TOG( "UP/DOWN - t", "SELECTION_MOVETOGETHER_UPDOWNARROWS" );
						Draw.HRx05( Draw.R );

						using ( MainRoot.ENABLE( w ).USE( "HIDE_HOVER_BG", true, overrideObject: p.par_e.HIER_WIN_SET, padding: 0 ) ) 
							Draw.TOG( "LEFT/RIGHT - Expand hover item (even if item wasn't select)", "RIGHTARROW_EXPANDS_HOVERED" );
						using ( MainRoot.ENABLE( w ).USE( "ENABLE_ALL" ) )
							Draw.TOG( "Disable hover gray rect", "HIDE_HOVER_BG", overrideObject: p.par_e.HIER_WIN_SET );

						Draw.HRx05( Draw.R );

						Draw.TOG( "DOUBLE CLICK - Expands item (instead framing in scene view)", "DOUBLECLICK_TO_EXPAND", EnableRed: false );
						Draw.HELP( w, "In this case, if you need move the camera to object (by default double-click), you can use right click on the SetActive button" );

						Draw.HRx05( Draw.R );

						Draw.TOG( "ESCAPE - closes edit prefab mode", "ESCAPE_CLOSES_PREFABMODE", EnableRed: false );
						using ( ENABLE( w ).USE( Draw.GetSetter( "ESCAPE_CLOSES_PREFABMODE" ) ) ) Draw.TOG( "Close only if Hierarchy or SceneView are focus", "CLOSE_PREFAB_KEY_FORHIER_ANDSCENE" );

						Draw.HRx05( Draw.R );

						Draw.TOG( "Use OnMosueDown instead OnMouseUp for modules", "ONDOWN_ACTION_INSTEAD_ONUP" );
						Draw.TOG( "Swap Left/Right mouse buttons for modules", "USE_SWAP_FOR_BUTTONS_ACTION" );
						//using ( ENABLE(w).USE( "USE_SWAP_FOR_BUTTONS_ACTION", 0 ) ) 
						Draw.HELP( w, "- default: left to open menu, right to search\n- swapped: left to search, right to open menu." );
						//  Draw.TOG( "Use horizontal scroll", "USE_HORISONTAL_SCROLL" );
						//    using ( GRO(w).UP( 0 ) )


						Draw.Sp( 10 );
					}

					//Draw.HRx1();

					Draw.Sp( 10 );
					using ( MainRoot.GRO( w ).UP( 0 ) )
					{

						Draw.TOG_TIT( "Additional:", EnableRed: true );
						//FINAL

						Draw.TOG( "Custom hierarchy pro windows opening animation", "ENABLE_CUSTOMWINDOWS_OPENANIMATION" );
						Draw.TOG( "Ping changed objects", "ENABLE_OBJECTS_PING" );
						Draw.TOG( "Tracking and log compile time", "TRACKING_COMPILE_TIME" );
						Draw.Sp( 10 );
					}




				}, () => {


					Draw.Sp( 10 );
					using ( GRO( w ).UP( 0 ) )
					{
						Draw.TOG_TIT( "Some other features:", EnableRed: true );

						Draw.TOG( "Use hover color for modules buttons (bugs)", "USE_HOVER_FOR_BUTTONS" );
						using ( ENABLE( w ).USE( "USE_HOVER_FOR_BUTTONS", 0 ) ) Draw.HELP( w, "For some strange reason unity 2019 has strange behavior, it works only for internal styles with null textures, but mby in any latest version it will be fixed." );
						Draw.TOG( "Use expansion animation for hierarchy lines (bugs)", "USE_EXPANSION_ANIMATION" );
						using ( ENABLE( w ).USE( "USE_EXPANSION_ANIMATION", 0 ) ) Draw.HELP( w, "I could not catch a rectangle for animating elements, the unity always returns strange 0 y positions" );
						Draw.TOG( "Use dynamic GL batching for drawing", "USE_DINAMIC_BATCHING" );
						using ( ENABLE( w ).USE( "USE_DINAMIC_BATCHING", 0 ) ) Draw.HELP( w, "You can turn it off if you see any artifacts with textures." );

					}
					///  #########################################################################################################################################################################################

				} );




			}
		}


		internal static void QWE( IRepaint w, EditorSettingsAdapter.WindowSettings KEY, Action a1, Action a2, Action a3 )
		{


			using ( MainRoot.GRO( w ).UP( 0 ) )
			{
				Draw.Sp( 10 );

				// Draw.TOG_TIT( "Style" );
				Draw.TOG( "Override lines height", /*-*/"USE_LINE_HEIGHT", overrideObject: KEY, EnableRed: false );
				using ( MainRoot.ENABLE( w ).USE( Draw.GetSetter(/*-*/"USE_LINE_HEIGHT", overrideObject: KEY ) ) )
				{
					Draw.FIELD( "Line Height '16'", /*-*/"LINE_HEIGHT", 4, 60, overrideObject: KEY );
					//    if ( p.par_e.USE_LINE_HEIGHT && p.par_e.LINE_HEIGHT < 16 ) Draw.HELP(w, "Warning! Since Unity 2017, line height < 16 sometimes throws DrawSelection Exeption!", new Color( 0.9f, 0.7f, 0.3f, 1 ) );
				}

				Draw.TOG( "Override child indent", /*-*/"USE_CHILD_INDENT", overrideObject: KEY, EnableRed: false );
				using ( MainRoot.ENABLE( w ).USE( Draw.GetSetter(/*-*/"USE_CHILD_INDENT", overrideObject: KEY ) ) ) Draw.FIELD( "Child Indent", /*-*/"CHILD_INDENT", 0, 60, overrideObject: KEY );
				Draw.TOG( "Override default icons size", /*-*/"USE_OVERRIDE_DEFAULT_ICONS_SIZE", overrideObject: KEY, EnableRed: false );
				using ( MainRoot.ENABLE( w ).USE( Draw.GetSetter(/*-*/"USE_OVERRIDE_DEFAULT_ICONS_SIZE", overrideObject: KEY ) ) ) Draw.FIELD( "default icons size", /*-*/"OVERRIDE_DEFAULT_ICONS_SIZE", 2, 60, overrideObject: KEY );


				//     Draw.TOG( "Use horizontal scroll bar 'TEST'", "USE_HORISONTAL_SCROLL" );
				//  using ( GRO(w).UP( 0 ) )
				{
					Draw.TOG( "Override font size for Labels of GameObjects names", /*-*/"USE_OVERRIDE_FOR_GAMEOBJECTS_NAMES_LABELS_FONT_SIZE", overrideObject: KEY, EnableRed: false );
					using ( MainRoot.ENABLE( w ).USE( Draw.GetSetter(/*-*/"USE_OVERRIDE_FOR_GAMEOBJECTS_NAMES_LABELS_FONT_SIZE", overrideObject: KEY ) ) )
						Draw.FIELD( "Objects Labels font size '11'", /*-*/"OVERRIDE_FOR_GAMEOBJECTS_NAMES_LABELS_FONT_SIZE", 4, 60, overrideObject: KEY );
				}
				// FS

				Draw.HRx05( Draw.R );
				Draw.FIELD( "Hierarchy pro windows text font size '11'", "COMMON_LABELS_FONT_SIZE", 4, 60, overrideObject: KEY );//
				Draw.HRx05( Draw.R );
				//Draw.HRx1();

				///  #########################################################################################################################################################################################

				a1();

			}



			a2();
			//OTHER



			// Draw.TOG( "Double click expands item", "DRAW_HIERARHCHY_CHESS_FILLS" );

			// Draw.HRx2();
			Draw.Sp( 10 );
			using ( MainRoot.GRO( w ).UP( 0 ) )
			{
				Draw.TOG_TIT( "Use background decorations", /*-*/"USE_BACKGROUNDDECORATIONS_MOD", overrideObject: KEY );

				using ( MainRoot.ENABLE( w ).USE( Draw.GetSetter(/*-*/"USE_BACKGROUNDDECORATIONS_MOD", overrideObject: KEY ), 0 ) )
				{


					Draw.TOG( "Draw hierarchy child lines", /*-*/"DRAW_HIERARHCHY_CHILD_LINES", overrideObject: KEY );
					using ( MainRoot.ENABLE( w ).USE( Draw.GetSetter(/*-*/"DRAW_HIERARHCHY_CHILD_LINES", overrideObject: KEY ) ) )
					{
						Draw.COLOR( "Child lines color", /*-*/"COLOR_HIERARHCHY_CHILD_LINES", overrideObject: KEY );
						Draw.TOOLBAR( new[] { "Solid", "Solid/Dotted", "Dotted", "Dotted/Transp" }, /*-*/"HIERARHCHY_CHILD_LINES_DOTS", overrideObject: KEY );
						var en = GUI.enabled;
						GUI.enabled &= KEY.HIERARHCHY_CHILD_LINES_DOTS != 0;
						Draw.FIELD( "Dot size:", /*-*/"HIERARHCHY_CHILD_LINES_DOT_SIZE", 1, 20, overrideObject: KEY );
						GUI.enabled = en;
						Draw.FIELD( "Root lines alpha:", /*-*/"HIERARHCHY_CHILD_LINES_TALPHA", 0, 1, overrideObject: KEY );
					}

					//Draw.HRx1();
					Draw.HRx05( Draw.R );


					GUI.Label( Draw.R, Draw.CONT( "Draw background chess fills:" ) );
					Draw.TOOLBAR( new[] { "No", "Clamped fills", "Full fills" }, /*-*/"DRAW_HIERARHCHY_CHESS_FILLS", overrideObject: KEY, enabled: new[] { true, KEY.pluginID == 0, true } );
					using ( MainRoot.ENABLE( w ).USE( Draw.GetSetter(/*-*/"DRAW_HIERARHCHY_CHESS_FILLS", overrideObject: KEY ) ) ) Draw.COLOR( "Background fill color", /*-*/"COLOR_HIERARHCHY_CHESS_FILLS", overrideObject: KEY );


					GUI.Label( Draw.R, Draw.CONT( "Draw horizontal separation lines:" ) );
					Draw.TOOLBAR( new[] { "No", "Clamped separations", "Full separations" }, /*-*/"DRAW_HIERARHCHY_CHESS_LINES", overrideObject: KEY, enabled: new[] { true, KEY.pluginID == 0, true } );
					using ( MainRoot.ENABLE( w ).USE( Draw.GetSetter(/*-*/"DRAW_HIERARHCHY_CHESS_LINES", overrideObject: KEY ) ) ) Draw.COLOR( "Horizontal lines color", /*-*/"COLOR_HIERARHCHY_CHESS_LINES", overrideObject: KEY );


					//Draw.HRx1();
					Draw.HRx05( Draw.R );

					//CHILD COUNT
					Draw.TOG( "Draw child count number", /*-*/"DRAW_CHILDS_COUNT", overrideObject: KEY );
					using ( MainRoot.ENABLE( w ).USE( Draw.GetSetter(/*-*/"DRAW_CHILDS_COUNT", overrideObject: KEY ) ) )
					{
						Draw.COLOR( "Numbers Color", /*-*/"DRAW_CHILDS_COLOR", overrideObject: KEY );
						Draw.TOG( "Invert numbers colors", /*-*/"DRAW_CHILDS_INVERCE_COLOR", overrideObject: KEY );
						Draw.TOG( "Hide numbs for root object", /*-*/"HIDE_CHILDCOUNT_IFROOT", overrideObject: KEY );
						Draw.TOG( "Hide numbs for expanded object", /*-*/"HIDE_CHILDCOUNT_IFEXPANDED", overrideObject: KEY );
						Draw.TOOLBAR( new[] { "Align Left", "Align Middle", "Align Right" }, /*-*/"CHILDCOUNT_ALIGMENT", overrideObject: KEY );
						Draw.FIELD( "Child numbs offset X", /*-*/"CHILDCOUNT_OFFSET_X", -200, 200, overrideObject: KEY );
						Draw.FIELD( "Child numbs offset Y", /*-*/"CHILDCOUNT_OFFSET_Y", -200, 200, overrideObject: KEY );

					}
				}
			}
			a3();

		}

	}
}
