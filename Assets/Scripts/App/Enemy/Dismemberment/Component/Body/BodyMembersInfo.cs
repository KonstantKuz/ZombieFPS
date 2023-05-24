using System;
using System.Collections.Generic;
using System.Linq;
using App.Enemy.Dismemberment.Component.BodyMember;
using App.Enemy.Dismemberment.Model;
using SuperMaxim.Core.Extensions;

namespace App.Enemy.Dismemberment.Component.Body
{
    public class BodyMembersInfo
    {
        private readonly HashSet<BodyMemberType> _availableMembers;

        public event Action OnMembersChanged;
        
        public BodyMembersInfo(BodyMemberBehaviour[] members)
        {
            _availableMembers = members.Select(it => it.BodyMemberType).ToHashSet();
            members.ForEach(it=>it.OnMemberDetached += OnMemberDetached);
        }
        public bool IsAvailable(List<BodyMemberType> memberTypes) =>
            memberTypes.IsNullOrEmpty() || memberTypes.All(type => _availableMembers.Contains(type));

        public bool IsNotAvailable(List<BodyMemberType> memberTypes) => 
            memberTypes.IsNullOrEmpty() || memberTypes.All(type => !_availableMembers.Contains(type));

        private void OnMemberDetached(BodyMemberBehaviour memberBehaviour)
        {
            memberBehaviour.OnMemberDetached -= OnMemberDetached;
            _availableMembers.Remove(memberBehaviour.BodyMemberType);
            OnMembersChanged?.Invoke();
        }
    }
}