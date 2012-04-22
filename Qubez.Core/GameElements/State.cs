using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Qubez.Core
{
    /// <summary> A class representing a State entitie. </summary>
    public class State
    {
        //[XmlArray]
        ///<summary> The State's Players. </summary>
        public List<Player> Players { get; set; }

        //[XmlElement]
        ///<summary> The Date of the last change in the board. </summary>
        public DateTime Time { get; set; }

        //[XmlElement]
        ///<summary> The State's Board. </summary>
        public Board Board { get; set; }

        //public Player[] Players
        //{
        //    get { return _players.ToArray(); }
        //    //set?
        //}

        public State()
        {
            Players = new List<Player>();
        }
    }
}
