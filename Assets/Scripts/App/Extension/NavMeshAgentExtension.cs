using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

namespace App.Extension
{
    public static class NavMeshAgentExtension
    {
        private const float UNITY_RECOMENDED_HEIGHT_MULTIPLIER = 2f;
        
        public static void PlaceOnNavMesh(this NavMeshAgent agent, Vector3 pos)
        {
            if (!agent.isOnNavMesh)
            {
                pos = agent.SampleNavMeshPosition(pos);
            }

            agent.enabled = true;
            var isWarped = agent.Warp(pos);
            Assert.IsTrue(isWarped, $"failed to place unit to {pos.ToString()}");
        }

        private static Vector3 SampleNavMeshPosition(this NavMeshAgent agent, Vector3 position)
        {
            // To avoid frame rate issues, it is recommended that you specify a maxDistance of twice the agent height.
            var filter = new NavMeshQueryFilter
            {
                agentTypeID = agent.agentTypeID,
                areaMask = agent.areaMask
            };
            if (!NavMesh.SamplePosition(position, out var hit, UNITY_RECOMENDED_HEIGHT_MULTIPLIER * agent.height, filter))
            {
                throw new Exception($"Failed to place agent on nav mesh in pos {position}");
            }

            return hit.position;
        }
    }
}