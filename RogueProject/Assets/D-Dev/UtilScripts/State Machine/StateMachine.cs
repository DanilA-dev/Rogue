﻿using System;
using System.Collections.Generic;
using D_Dev.UtilScripts.State_Machine.Predicates;
using D_Dev.UtilScripts.State_Machine.Transitions;

namespace D_Dev.UtilScripts.State_Machine
{
    public partial class StateMachine
    {
        #region Fields

        private StateMachine.StateNode _currentStateNode;
        private Dictionary<Type, StateMachine.StateNode> _stateDic = new Dictionary<Type, StateMachine.StateNode>();
        private HashSet<ITransition> _anyTransitions = new HashSet<ITransition>();

        #endregion

        #region Public

        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }
        
        public void AddAnyTransition(IState to, IPredicate condition)
        {
            _anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
        }
        
        public void OnUpdate()
        {
            ITransition transition = GetTransition();
            if (transition != null)
                ChangeState(transition.To);
            
            _currentStateNode?.State.OnUpdate();
        }

        public void OnFixedUpdate()
        {
            _currentStateNode?.State.OnFixedUpdate();
        }
        
        
        public void SetState(IState state)
        {
            _currentStateNode = _stateDic[state.GetType()];
            _currentStateNode?.State.OnEnter();
        }

        #endregion

        #region Private

        private void ChangeState(IState state)
        {
            if(_currentStateNode.State == state)
                return;

            var prevState = _currentStateNode.State;
            var nextState = _stateDic[state.GetType()];
            prevState?.OnExit();
            nextState.State?.OnEnter();
            _currentStateNode = nextState;
        }

        private ITransition GetTransition()
        {
            foreach (var transition in _anyTransitions)
            {
                if (transition.Condition.Evaluate())
                    return transition;
            }
            
            foreach (var transition in _currentStateNode.Transitions)
            {
                if (transition.Condition.Evaluate())
                    return transition;
            }

            return null;
        }

        private StateMachine.StateNode GetOrAddNode(IState state)
        {
            var statenode = _stateDic.GetValueOrDefault(state.GetType());
            if (statenode == null)
            {
                statenode = new StateMachine.StateNode(state);
                _stateDic.Add(state.GetType(), statenode);
            }
            return statenode;
        }

        #endregion
    }
}
