using System;
using System.Collections.Generic;

namespace Qubez
{

    /// <summary> A class representing a StateHolder entitie. </summary>
    public class StateHolder
    {

        private static StateHolder _stateHolder = null;
        private static readonly object padlock = new object();

        ///<summary> Default constructor </summary>
        private StateHolder() { }

        ///<summary> The Date of the last change in the board. </summary>
        public State PreviousWordState { get; set; }

        ///<summary> The States representing. </summary>
        public CircularList<State> LetterStates { get; set; }

        ///<summary> The StateHolder's Current State. </summary>
        public State CurrentState { get; set; }

        // Singleton (design patterns rulezzz).
        public static StateHolder GetStateHolder
        {
            get
            {
                lock (padlock)
                {
                    if (_stateHolder == null)
                    {
                        _stateHolder = new StateHolder();
                    }
                    return _stateHolder;
                }
            }
        }
    }
}
