using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;

namespace MeshTK
{
	public class NormalsEditor : EJMMeshEditor
	{
		#region VARIABLES
		Vector3[] meshvertices;
		Vector3[] meshnormals;
		Rect windowrect = new Rect (10, 100, 150, 150);
		//Selection Information
		private int selectiontype = PlayerPrefs.GetInt ("meshtk-selection", 0);
		int geoselection = 0;
		private HashSet<int> selectedNormals = new HashSet<int> ();
		private Vector3 worldselectioncenter;
		private Vector3 localselectioncenter;
		private Quaternion selectionrotation;
		#endregion

		#region PUBLIC METHODS
		public NormalsEditor(EJMMesh target){
			ejmmesh = target;
			ReloadData ();
		}

		public override void DrawSceneGUI()
		{
			if (!(Tools.current == Tool.View || (Event.current.isMouse && Event.current.button > 0) || Event.current.type == EventType.ScrollWheel || (Event.current.alt && Event.current.isMouse))) {
				if (Event.current.type == EventType.Layout) {
					HandleUtility.AddDefaultControl (GUIUtility.GetControlID (FocusType.Passive));
				} else {
					TryShortcuts();
				}
				DrawNormals ();
				if (Event.current.shift || !TryTranslation ()) {
					if (geoselection==1){
						TryTriangleSelect();
					} else if (selectiontype==0){
						TrySingleSelect ();
					} else if (selectiontype==1){
						TryBoxSelect ();
					} else {
						TryPaintSelect ();
					}
				}
				Handles.BeginGUI ();
				windowrect = GUI.Window (0, windowrect, this.GUIWindow, "Normal Tools");
				Handles.EndGUI ();
			}
		}

		//Override the reload method
		public override void ReloadData()
		{
			if (ejmmesh.SharedMesh.normals != meshnormals) {
				meshvertices = ejmmesh.SharedMesh.vertices;
				meshnormals = ejmmesh.SharedMesh.normals;
				selectedNormals.Clear ();
			}
		}

		public void UpdateMesh()
		{
			ejmmesh.SharedMesh.normals = meshnormals;
		}
		#endregion

		#region GUI
		private void GUIWindow (int windowID)
		{
			GUI.DragWindow (new Rect (0, 0, 10000, 20));
			GUI.color = Color.white;
			Rect tooltipBox = GUILayoutUtility.GetRect (100f, 200f, 18f, 18f);
			GUILayout.Space (tooltipBox.height);
			EditorGUI.BeginChangeCheck ();
			geoselection = EditorGUILayout.Popup (geoselection, new string[2]{"Select By Vertex", "Select By Triangle"});
			if (geoselection==0){
				selectiontype = EditorGUILayout.Popup (selectiontype, new string[3]{"Single Select (Shift-1)", "Box Select (Shift-2)", "Paint Select (Shift-3)"});
			}
			if (EditorGUI.EndChangeCheck ()) {
				PlayerPrefs.SetInt ("meshtk-selection", selectiontype);
			}
			if (GUILayout.Button (new GUIContent("Select All", "Select All (Shift-a)"), EditorStyles.miniButton)){
				for (int i=0;i<meshnormals.Length;++i){
					selectedNormals.Add (i);
				}
				UpdateSelectionCenter();
			}
			if (GUILayout.Button ("Invert Selected", EditorStyles.miniButton)) {
				foreach (int i in selectedNormals){
					meshnormals[i] = -meshnormals[i];
				}
				UpdateMesh ();
			}
			if (GUILayout.Button ("Calculate All", EditorStyles.miniButton)) {
				ejmmesh.SharedMesh.RecalculateNormals ();
				ReloadData ();
			}
			EditorGUI.HelpBox (tooltipBox, string.IsNullOrEmpty (GUI.tooltip) ? "Current tool: Rotate":GUI.tooltip, MessageType.None);
		}

		private void TryShortcuts(){
			if (Event.current.type == EventType.KeyUp && Event.current.shift){
				switch (Event.current.keyCode){
				case KeyCode.Alpha1:
					selectiontype = 0;
					break;
				case KeyCode.Alpha2:
					selectiontype = 1;
					break;
				case KeyCode.Alpha3:
					selectiontype = 2;
					break;
				case KeyCode.A:
					selectedNormals.Clear ();
					foreach (int item in Enumerable.Range(0,meshvertices.Length)){
						selectedNormals.Add(item);
					}
					UpdateSelectionCenter();
					break;
				}
			}
		}

		private void DrawNormals()
		{
			Handles.matrix = ejmmesh.LocalToWorld;
			float size = HandleUtility.GetHandleSize(ejmmesh.SharedMesh.bounds.center);
			for (int i = 0; i < meshvertices.Length; i++) {
				if (selectedNormals.Contains (i))
					Handles.color = SelectionColor;
				else
					Handles.color = DefaultColor;
				Handles.DrawLine(meshvertices [i], meshvertices[i] + meshnormals[i] * size);
			}
			Handles.matrix = Matrix4x4.identity;
		}
		#endregion

		#region SELECTION
		private void TryTriangleSelect()
		{
			if (Event.current.type == EventType.MouseDown && !Event.current.shift) {
				selectedNormals.Clear ();
			} else if (Event.current.type == EventType.MouseUp) {
				Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
				ray.origin = ejmmesh.LocalToWorld.inverse.MultiplyPoint3x4(ray.origin);
				ray.direction = ejmmesh.LocalToWorld.inverse.MultiplyVector(ray.direction);//targettransform.InverseTransformDirection(ray.direction);
				float closestdist = Mathf.Infinity;
				int closestindex = -1;
				for (int i = 0; i < ejmmesh.triangles.Length; i+=3) {
					float intersectdist = EditorTools.RayTriangleIntersection(ray, meshvertices[ejmmesh.triangles[i]], meshvertices[ejmmesh.triangles[i+1]], meshvertices[ejmmesh.triangles[i+2]]);
					if (intersectdist!=-1 && intersectdist < closestdist)
					{
						closestindex = i/3;
						closestdist = intersectdist;
					}
				}
				if (closestindex!=-1){
					if (!selectedNormals.Contains (ejmmesh.triangles[closestindex*3])){
						selectedNormals.Add (ejmmesh.triangles[closestindex*3]);
					}
					if (!selectedNormals.Contains (ejmmesh.triangles[closestindex*3+1])){
						selectedNormals.Add (ejmmesh.triangles[closestindex*3+1]);
					}
					if (!selectedNormals.Contains (ejmmesh.triangles[closestindex*3+2])){
						selectedNormals.Add (ejmmesh.triangles[closestindex*3+2]);
					}
				}
				UpdateSelectionCenter();
			}
		}
		private void TrySingleSelect()
		{
			if (Event.current.type == EventType.MouseDown && !Event.current.shift) {
				selectedNormals.Clear ();
			} else if (Event.current.type == EventType.MouseUp) {
				float closestdistance = 100f;
				float dist = 0f;
				Vector2 screenpt;
				int closestindex = -1;
				for (int i=0; i<meshvertices.Length; i++) {
					screenpt = Camera.current.WorldToScreenPoint (ejmmesh.LocalToWorld.MultiplyPoint3x4(meshvertices [i]));
					dist = Vector2.Distance(screenpt, EditorTools.GUIToScreenPoint(Event.current.mousePosition));
					if (dist < closestdistance) {
						closestdistance = dist;
						closestindex = i;
					}
				}
				if (closestindex != -1) {
					if (!selectedNormals.Contains (closestindex)){
						selectedNormals.Add (closestindex);
					} else {
						selectedNormals.Remove (closestindex);
					}
				}
				UpdateSelectionCenter();
			}
		}
		private void TryBoxSelect()
		{
			if (Event.current.type == EventType.MouseDown && !Event.current.shift) {
				selectedNormals.Clear ();
			}
			Rect? temprect = EditorTools.TryBoxSelect ();
			if (temprect != null) {
				Rect selectedRect = EditorTools.GUIToScreenRect(temprect.GetValueOrDefault ());
				for (int i = 0; i < meshvertices.Length; i++) {
					if (selectedRect.Contains (Camera.current.WorldToScreenPoint (ejmmesh.LocalToWorld.MultiplyPoint3x4(meshvertices [i])))) {
						if (selectedNormals.Contains (i)) {
							selectedNormals.Remove (i);
						} else {
							selectedNormals.Add (i);
						}
					}
				}
				UpdateSelectionCenter();
			}
		}
		private void TryPaintSelect()
		{
			if (Event.current.type == EventType.MouseDown && !Event.current.shift) {
				selectedNormals.Clear ();
			} else if (Event.current.type == EventType.MouseDrag) {
				float closestdistance = 100f;
				float dist = 0f;
				Vector2 screenpt;
				int closestindex = -1;
				for (int i=0; i<meshvertices.Length; i++) {
					screenpt = Camera.current.WorldToScreenPoint (ejmmesh.LocalToWorld.MultiplyPoint3x4(meshvertices [i]));
					dist = Vector2.Distance(screenpt, EditorTools.GUIToScreenPoint(Event.current.mousePosition));
					if (dist < closestdistance) {
						closestdistance = dist;
						closestindex = i;
					}
				}
				if (closestindex != -1) {
					if (!selectedNormals.Contains (closestindex)){
						selectedNormals.Add (closestindex);
					}
				}
				UpdateSelectionCenter();
			}
		}
		public void UpdateSelectionCenter()
		{
			Vector3 average = Vector3.zero;
			foreach (int index in selectedNormals) {
				average += meshvertices [index];
			}
			localselectioncenter = average / selectedNormals.Count;
			worldselectioncenter = ejmmesh.LocalToWorld.MultiplyPoint3x4(localselectioncenter);
			selectionrotation = Quaternion.LookRotation (ejmmesh.targettransform.forward);
		}
		#endregion

		#region MODIFICATION
		private bool TryTranslation()
		{
			//Normals can only be rotated
			if (selectedNormals.Count > 0){
				Quaternion newrotation = Handles.RotationHandle (selectionrotation, worldselectioncenter);
				if (newrotation != selectionrotation){
					Quaternion rotationOffset = Quaternion.Inverse(selectionrotation) * newrotation;
					foreach (int index in selectedNormals) {
						meshnormals [index] = rotationOffset * meshnormals[index];
					}
					selectionrotation = newrotation;
					UpdateMesh();
					return true;
				}
			}
			return false;
		}
		#endregion
	}
}
