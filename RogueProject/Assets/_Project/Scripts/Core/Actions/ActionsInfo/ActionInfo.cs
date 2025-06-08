using System;
using Cysharp.Threading.Tasks;
using D_Dev.Scripts.Runtime.UtilScripts.AnimatorView.AnimationPlayableHandler;
using D_Dev.UtilScripts.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Core.Combat.ActionsInfo
{
    [CreateAssetMenu(menuName = "Game/Infos/Action Info")]
    public class ActionInfo : ScriptableObject
    {
        #region Fields
        
        [Title("View Info")]
        [SerializeField] private string _actionName;
        [SerializeField] private string _actionDescription;
        [PreviewField(75, ObjectFieldAlignment.Right)]
        [SerializeField] private Sprite _actionIcon;
        [SerializeField] private AnimationPlayableClipConfig _actionAnimation;
        [SerializeField] private float _actionDelayTime;
        [Space]
        [Title("Action")]
        [SerializeReference] private ICombatAction _combatAction;
        
        #endregion

        #region Properties
        
        public string ActionName => _actionName;

        public string ActionDescription => _actionDescription;

        public Sprite ActionIcon => _actionIcon;

        public AnimationPlayableClipConfig ActionAnimation => _actionAnimation;

        #endregion

        #region Public

        public async UniTask DoAction(GameObject owner, GameObject target)
        {
            try
            {
                if(_actionAnimation.AnimationClip != null &&
                   owner.TryGetComponentInAny(out AnimationClipPlayableMixer animationMixer))
                {
                    animationMixer.Play(_actionAnimation);
                    await UniTask.Delay((TimeSpan.FromSeconds(_actionDelayTime)), cancellationToken: owner.GetCancellationTokenOnDestroy());
                }
                _combatAction.DoAction(target);
            }
            catch (OperationCanceledException e)
            {
                _combatAction.DoAction(target);
            }
        }

        #endregion
    }
}