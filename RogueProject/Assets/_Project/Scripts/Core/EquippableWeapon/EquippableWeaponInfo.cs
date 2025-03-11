using D_Dev.UtilScripts.Entities.EntitiesInfo;
using UnityEngine;

namespace _Project.Scripts.Core.EquippableWeapon
{
    
    [CreateAssetMenu(menuName = "Game/Infos/Equippable Weapon Info")]
    public class EquippableWeaponInfo : EntityInfo
    {
        [field: SerializeField] public EquippableWeaponConfig Config { get; private set; }
    } 
}