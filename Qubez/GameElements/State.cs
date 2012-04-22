using System;
using System.Collections.Generic;

namespace Qubez
{

    /// <summary> A class representing a State entitie. </summary>
    public class State
    {

        ///<summary> Default constructor </summary>
        public State() { }

        ///<summary> The Date of the last change in the board. </summary>
        public DateTime Time { get; set; }

        ///<summary> The State's Players. </summary>
        public List<Player> Players { get; set; }

        ///<summary> The State's Board. </summary>
        public Board Board { get; set; }

    }
}
