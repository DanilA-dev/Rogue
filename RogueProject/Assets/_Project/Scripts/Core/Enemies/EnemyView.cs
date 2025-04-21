using D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler;
using UnityEngine;

namespace _Project.Scripts.Core.Enemies
{
    public class EnemyView : MonoBehaviour
    {
        #region Fields

        [SerializeField] private AnimationClipPlayableMixer _locomotionPlayableMixer;
        [SerializeField] private AnimationClipPlayableMixer _oneShotPlayableMixer;
        [Space]
        [SerializeField] private AnimationPlayableClipConfig _idleAnimation;
        [SerializeField] private AnimationPlayableClipConfig _patrolAnimation;
        [SerializeField] private AnimationPlayableClipConfig _chaseAnimation;
        [SerializeField] private AnimationPlayableClipConfig _getHitAnimation;
        [SerializeField] private AnimationPlayableClipConfig _deathAnimation;

        #endregion

        #region Public

        public void PlayIdleAnimation() => _locomotionPlayableMixer.Play(_idleAnimation);
        public void PlayPatrolAnimation() => _locomotionPlayableMixer.Play(_patrolAnimation);
        public void PlayChaseAnimation() => _locomotionPlayableMixer.Play(_chaseAnimation);
        
        public void PlayGetHitAnimation() => _oneShotPlayableMixer.Play(_getHitAnimation);
        public void PlayDeathAnimation() => _oneShotPlayableMixer.Play(_deathAnimation);

        #endregion
    }
}