using _Project.Scripts.Core.Combat.ActionsInfo;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Project.Scripts.Core.Combat.UI.Items
{
    public class ActionItemView : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Image _actionIcon;
        [SerializeField] private Button _actionActivateButton;
        [SerializeField] private Button _actionDescriptionButton;
        [SerializeField] private TMP_Text _actionNameText;
        [SerializeField] private TMP_Text _actionPointsText;
        [SerializeField] private TMP_Text _actionDescriptionText;
        [SerializeField] private TMP_Text _actionCooldownText;
        [Space] 
        [SerializeField] private UnityEvent OnActionReady;
        [SerializeField] private UnityEvent OnActionCooldown;

        #endregion

        #region Properties

        public ActionInfo ActionInfo { get; private set; }

        #endregion
        
        #region Public

        public void SetData(ActionInfo actionInfo)
        {
            ActionInfo = actionInfo;
            SetView(ActionInfo);
        }

        #endregion

        #region Private

        private void SetView(ActionInfo actionInfo)
        {
            if(_actionIcon != null)
                _actionIcon.sprite = actionInfo.ActionIcon;
            
            if(_actionNameText != null)
                _actionNameText.text = actionInfo.ActionName;

            if(_actionDescriptionText != null)
                _actionDescriptionText.text = actionInfo.ActionDescription;
            
            if(_actionCooldownText!= null)
                _actionCooldownText.text = actionInfo.CombatAction.CooldownAmount.ToString();
            
            if(_actionPointsText!= null)
                _actionPointsText.text = actionInfo.CombatAction.ActionPoints.ToString();
        }

        #endregion
    }
}