using System.Linq;
using Dreamteck;
using Feofun.Components;
using Feofun.World.Factory.ObjectFactory.Factories;
using Feofun.World.Model;
using SuperMaxim.Core.Extensions;
using UnityEngine;
using Zenject;

namespace App.Enemy.Dismemberment.Component.BodyMember.Sound
{
    public class DismembermentSoundPlayer : MonoBehaviour, IInitializable<Unit.Unit>
    {
        [SerializeField] private WorldObject _dismemberSound;

        private BodyMemberBehaviour[] _bodyMembers;

        [Inject] private ObjectPoolFactory _objectFactory;

        private void Awake() => _bodyMembers = GetComponentsInChildren<BodyMemberBehaviour>();

        public void Init(Unit.Unit unit) => 
            _bodyMembers.ForEach(it => it.OnMemberDetached += PlayDismemberSound);

        private void PlayDismemberSound(BodyMemberBehaviour bodyMember)
        {
            bodyMember.OnMemberDetached -= PlayDismemberSound;
            if (!_objectFactory.HasFreeObject(_dismemberSound.ObjectId)) return;
            var audioSource = _objectFactory.Create<AudioSource>(_dismemberSound.ObjectId);
            audioSource.transform.SetPositionAndRotation(bodyMember.transform.position, bodyMember.transform.rotation);
            audioSource.Play();
        }

        private void OnDisable()
        {
            _bodyMembers
                .Where(it => it != null)
                .ForEach(it => it.OnMemberDetached -= PlayDismemberSound);
        }
    }
}