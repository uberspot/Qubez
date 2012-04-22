using System;

namespace Qubez {
    public class Letter {

        ///<summary> Default constructor </summary>
        public Letter() : this('-', 0) { }

        public Letter(char ch, uint points){
            this.Char = ch;
            this.Points = points;
        }
		
		///<summary> The character the letter represents. </summary>
		public char Char{ get; set; }
		
		///<summary> The Letter's points. </summary>
		public uint Points { get; set; }

        /// <summary> Returns true when the given element matches the null one. </summary>
        /// <param name="t">The element to be checked.</param>
        public bool isNull() { 
            return this.Char == '-'; 
        }
    }
}


