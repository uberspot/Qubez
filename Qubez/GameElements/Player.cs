using System;
using System.Collections.Generic;

namespace Qubez {
    /// <summary> A class representing Player entities. </summary>
    public class Player {
		
		///<summary> Default constructor </summary>
		public Player() :this(string.Empty, string.Empty, 0){
		}
		
		public Player(string id, string name, uint score){ 
			Id = id; 
			Name = name; 
			Score = score;
			Chars = new CharacterPool();
		}
		
		///<summary> The player's id </summary>
		public string Id { get; set; }
		
        /// <summary> The name of the Player. </summary>
        public string Name{ get; set; }

        /// <summary> The score of the Player. </summary>
        public uint Score { get; set; }
		
		/// <summary> A pool holding all the available characters of the player. </summary>
		public CharacterPool Chars { get; set; }
    }
}
