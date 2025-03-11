using UnityEngine;

namespace _Project.Scripts.Core.EquippableWeapon
{
    [System.Serializable]
    public class EquippableWeaponAnimation
    {
        [field: SerializeField] public EquippableWeaponState State { get;  private set; }
        [field: SerializeField] public AnimationClip AnimationClip { get; private set; }
        [field: SerializeField] public float CrossFadeTime { get; private set; }
        [field: SerializeField] public bool Loop { get; private set; }
        [field: SerializeField, Range(0, 10)] public float AnimationTime { get; private set; }
    }
}