using D_Dev.UtilScripts.Entities.EntitiesInfo;
using UnityEngine;

namespace _Project.Scripts.Core.Weapon
{
    
    [CreateAssetMenu(menuName = "Game/Infos/Weapon Info")]
    public class WeaponInfo : EntityInfo
    {
        [field: SerializeField] public WeaponData WeaponData { get; private set; }
    } 
}