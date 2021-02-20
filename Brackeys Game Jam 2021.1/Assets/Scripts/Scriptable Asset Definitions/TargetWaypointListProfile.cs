using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "New Target Waypoint Profile",
        menuName = "Level Manager/Target Waypoints/Create Profile", order = 0)]
    public class TargetWaypointListProfile : ScriptableObject
    {
        public List<Vector3> targetWaypointList = new List<Vector3>();


        public Color pointColor = Color.yellow;
        public Color lineColor = Color.cyan;
        public Color detectionColor = Color.green;
    }
}