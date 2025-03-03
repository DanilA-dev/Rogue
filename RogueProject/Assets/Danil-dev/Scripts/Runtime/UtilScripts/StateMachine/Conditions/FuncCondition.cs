﻿using System;

namespace D_Dev.Scripts.Runtime.UtilScripts.SimpleStateMachine
{
    public class FuncCondition : IStateCondition
    {
        private Func<bool> _func;

        public FuncCondition(Func<bool> func)
        {
            _func = func;
        }
        public bool IsMatched() => _func.Invoke();
    }
}