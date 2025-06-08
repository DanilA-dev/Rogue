using D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler;
using D_Dev.UtilScripts.PositionConfig;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Core.Weapon
{
    public class WeaponHolder : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Rigidbody _mainRigidBody;
        [Space]
        [Title("Weapon Settings")] 
        [SerializeField] private WeaponInfo _startWeaponInfo;
        [SerializeField, ReadOnly] private WeaponInfo _currentWeaponInfo;
        [SerializeField] private PositionConfig _weaponPosition;
        [Space]
        [Title("View Settings")]
        [SerializeField] private AnimationClipPlayableMixer _weaponAnimationClipPlayableMixer;
        [FoldoutGroup("Events")] 
        public UnityEvent OnEquip;
        
        #endregion


        #region Monobehaviour

        private void Start()
        {
            TrySetStartWeapon();
        }

        #endregion

        #region Public

        public void UseWeapon()
        {
          
        }

        #endregion
        
        #region Private

        private void TrySetStartWeapon()
        {
            if(_startWeaponInfo == null)
                return;
            
            SetWeapon(_startWeaponInfo);
        }

        private void SetWeapon(WeaponInfo info)
        {
            if (info == null)
            {
                Debug.LogError($"Can't create equippable weapon, info is null");
                return;
            }

            
        }

        #endregion

    }
}