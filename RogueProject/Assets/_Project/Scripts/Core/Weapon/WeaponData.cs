using _Project.Scripts.Core.Combat.ActionsInfo;
using UnityEngine;

namespace _Project.Scripts.Core.Weapon
{
    [System.Serializable]
    public class WeaponData
    {
        #region Fields

        [SerializeField] private ActionInfo[] _weaponActions;

        #endregion

        #region Properties

        public ActionInfo[] WeaponActions
        {
            get => _weaponActions;
            set => _weaponActions = value;
        }

        #endregion
    }
}