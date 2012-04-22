using System;
using System.Collections.Generic;

namespace Qubez {

    /// <summary> A class representing a Board entitie. </summary>
    public class Board {
		
		///<summary> Default constructor </summary>
        public Board() : this(-1, 1) { }
		
		public Board(int id, uint size){ 
			this.Id = id;
			this.Size = size;
			_letters = new List<char>[Size, Size]; 
		}
		
		///<summary> The Board's id. </summary>
		public int Id { get; set; }
		
		///<summary> The Board's Size. </summary>
		public uint Size { get; set; }
		
		///<summary> An array of List<char> holding the letters of each cell. </summary>
		private List<char>[,] _letters;
		
		public void AddLetter(uint x, uint y, char letter){
			if(x < Size && y < Size){
				_letters[x,y].Add(letter);
			}
		}
		
		public void RemoveLetter(uint x, uint y, char letter){
			if(x < Size && y < Size){
				_letters[x,y].Remove(letter);
			}
		}
		
		public uint GetHeight(uint x, uint y){
			if(x < Size && y < Size){
				return (uint) _letters[x,y].Count;
			}
            return 0;
		}
		
	}
}
