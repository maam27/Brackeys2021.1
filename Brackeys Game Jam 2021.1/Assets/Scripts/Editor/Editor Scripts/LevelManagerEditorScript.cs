using System;
using System.Collections.Generic;
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

            int registedPIndex = 0;
            Vector3 closestPoint = Vector3.zero;
            for (int p = 0; p < m_LevelManager.targetWaypointProfiles.Count; p++)
            {
                List<Vector3> targetWaypointList = m_LevelManager.targetWaypointProfiles[p].targetWaypointList;
                for (int w = 0; w < targetWaypointList.Count; w++)
                {
                    if (LevelManager.PublicAccess.IsObjectInVicinityOfAnotherObject(worldPos,
                        targetWaypointList[w], 50))
                    {
                        Vector3 levelOffset;
                        Vector3 pos =
                            Handles.PositionHandle(targetWaypointList[w], Quaternion.identity) +
                            m_LevelManager.transform.position + (levelOffset = new Vector3(m_LevelManager.levelOffset.x,
                                0, m_LevelManager.levelOffset.y));

                        targetWaypointList[w] =
                            pos - m_LevelManager.transform.position - levelOffset;

                        m_LevelManager.targetWaypointProfiles[p].targetWaypointList = targetWaypointList;
                        break;
                    }
                }
            }
        }
    }
}