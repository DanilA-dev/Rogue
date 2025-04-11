using D_Dev.Scripts.Runtime.UtilScripts.AnimatorView;
using D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler;
using UnityEngine;

namespace _Project.Scripts.Core.EquippableWeapon
{
    [System.Serializable]
    public class EquippableWeaponAnimation
    {
        [field: SerializeField] public EquippableWeaponState State { get;  private set; }
        [field: SerializeField] public AnimationPlayableClipConfig AnimationConfig { get; set; }
    }
}