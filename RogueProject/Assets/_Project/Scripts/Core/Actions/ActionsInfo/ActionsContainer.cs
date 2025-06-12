using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Core.Combat.ActionsInfo
{
    public class ActionsContainer : MonoBehaviour
    {
        #region Fields
        
        [SerializeField] private List<ActionInfo> _actionInfos = new List<ActionInfo>();
        
        #endregion

        #region Properties
        
        public IReadOnlyList<ActionInfo> ActionInfos => _actionInfos;
            
        #endregion

        #region Public Methods
        
        public void AddAction(ActionInfo actionInfo)
        {
            if (actionInfo != null && !_actionInfos.Contains(actionInfo))
                _actionInfos.Add(actionInfo);
        }

        public void RemoveAction(ActionInfo actionInfo)
        {
            if (actionInfo != null)
            {
                if (_actionInfos.Contains(actionInfo))
                    _actionInfos.Remove(actionInfo);
            }
        }
        
        
        #endregion
    }
} 