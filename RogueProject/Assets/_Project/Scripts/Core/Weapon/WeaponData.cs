using _Project.Scripts.Core.Combat.ActionsInfo;
using UnityEngine;

namespace _Project.Scripts.Core.Weapon
{
    [System.Serializable]
    public class WeaponData
    {
        #region Fields

        [SerializeField] private ActionInfo[] _weaponAction;

        #endregion

        #region Properties

        public ActionInfo[] WeaponAction
        {
            get => _weaponAction;
            set => _weaponAction = value;
        }

        #endregion
    }
}