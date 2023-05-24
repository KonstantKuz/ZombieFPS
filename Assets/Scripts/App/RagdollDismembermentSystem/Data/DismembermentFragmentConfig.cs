using System;
using App.RagdollDismembermentSystem.Component;
using UnityEngine;

namespace App.RagdollDismembermentSystem.Data
{
    [Serializable]
    public struct DismembermentFragmentConfig
    {
        [SerializeField] public DismembermentFragmentBone CrackedBone;
        [SerializeField] public Transform[] FragmentMeshesRoots;
    }
}