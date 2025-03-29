using D_Dev.Scripts.Runtime.UtilScripts.AnimatorView;
using UnityEngine;

namespace _Project.Scripts.Core.EquippableWeapon
{
    [System.Serializable]
    public class EquippableWeaponAnimation
    {
        [field: SerializeField] public EquippableWeaponState State { get;  private set; }
        [field: SerializeField] public AnimationConfig AnimationConfig { get; set; }
    }
}