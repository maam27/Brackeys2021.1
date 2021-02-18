using System;
using System.Linq;
using Level;
using UnityEditor;
using UnityEngine;

namespace Editor.Editor_Scripts
{
    [CustomEditor(typeof(LevelManager))]
    public class LevelManagerEditorScript : UnityEditor.Editor
    {
        private LevelManager m_LevelManager;
        private Camera m_MainCam;

        private void OnEnable()
        {
            SceneView.duringSceneGui += SceneViewOnduringSceneGui;
            m_LevelManager = target as LevelManager;
            m_MainCam = Camera.main;
        }


        private void OnDisable()
        {
            SceneView.duringSceneGui -= SceneViewOnduringSceneGui;
        }

        private void SceneViewOnduringSceneGui(SceneView obj)
        {
            Vector3 mousePosition = Event.current.mousePosition;
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
            float height = 0;

            float dstToDrawPlane = (height - ray.origin.y) / ray.direction.y;
            Vector3 worldPos = ray.GetPoint(dstToDrawPlane);


            Handles.Label(worldPos + Vector3.right * -1.15f, $"{worldPos}");
            HandleUtility.Repaint();


            for (int i = 0; i < m_LevelManager.targetWaypointsToTraverseTo.Count; i++)
            {
                if (LevelManager.PublicAccess.IsObjectInVicinityOfAnotherObject(worldPos,
                    m_LevelManager.targetWaypointsToTraverseTo[i], 0.1f))
                {
                    Vector3 pos =
                        Handles.PositionHandle(m_LevelManager.targetWaypointsToTraverseTo[i], Quaternion.identity) +
                        m_LevelManager.transform.position;

                    m_LevelManager.targetWaypointsToTraverseTo[i] = pos - m_LevelManager.transform.position;
                }
            }
        }
    }
}