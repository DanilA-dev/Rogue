using D_Dev.Scripts.Runtime.UtilScripts.AnimatorView;
using UnityEngine;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemyView : AnimatorView
    {
        #region Fields

        [SerializeField] private AnimationConfig _idleAnimation;
        [SerializeField] private AnimationConfig _patrolAnimation;
        [SerializeField] private AnimationConfig _chaseAnimation;
        [SerializeField] private AnimationConfig _attackAnimation;
        [SerializeField] private AnimationConfig _getHitAnimation;
        [SerializeField] private AnimationConfig _deathAnimation;

        #endregion

        #region Public

        public void PlayIdleAnimation() => PlayAnimation(_idleAnimation);
        public void PlayPatrolAnimation() => PlayAnimation(_patrolAnimation);
        public void PlayAttackAnimation() => PlayAnimation(_attackAnimation);
        public void PlayGetHitAnimation() => PlayAnimation(_getHitAnimation);
        public void PlayDeathAnimation() => PlayAnimation(_deathAnimation);
        public void PlayChaseAnimation() => PlayAnimation(_chaseAnimation);

        #endregion
    }
}