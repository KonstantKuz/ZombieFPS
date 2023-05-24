using System;
using System.Collections.Generic;
using System.Linq;
using App.Enemy.Dismemberment.Config;

namespace App.Enemy.Dismemberment.Model
{
    public class EnemyBodyModel
    {
        private IReadOnlyDictionary<BodyMemberType, BodyMemberModel> _members;

        public EnemyBodyModel(EnemyBodyConfig config)
        {
            _members = config.Members
                .ToDictionary(it => it.Type, it => new BodyMemberModel(it));
        }

        public BodyMemberModel GetMember(BodyMemberType memberType)
        {
            if (!_members.ContainsKey(memberType)) {
                throw new NullReferenceException($"No BodyMemberModel for id {memberType} in EnemyBodyModel");
            }
            return _members[memberType];
        }
    }
}