using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Qubez
{
    /// <summary> A Circular list containing all of the characters the user can still use. </summary>
    public class CharacterPool : CircularList<Letter>
    {

        /// <summary> Default constructor that fills the pool with the appropriate number of Characters. </summary>
        public CharacterPool()
        {
            for (int i = 0; i < 9; i++) this.Add(new Letter('A', 1));
            for (int i = 0; i < 2; i++) this.Add(new Letter('B', 3));
            for (int i = 0; i < 2; i++) this.Add(new Letter('C', 3));
            for (int i = 0; i < 4; i++) this.Add(new Letter('D', 2));
            for (int i = 0; i < 12; i++) this.Add(new Letter('E', 1));
            for (int i = 0; i < 2; i++) this.Add(new Letter('F', 4));
            for (int i = 0; i < 3; i++) this.Add(new Letter('G', 2));
            for (int i = 0; i < 2; i++) this.Add(new Letter('H', 4));
            for (int i = 0; i < 9; i++) this.Add(new Letter('I', 1));
            for (int i = 0; i < 1; i++) this.Add(new Letter('J', 8));
            for (int i = 0; i < 1; i++) this.Add(new Letter('K', 5));
            for (int i = 0; i < 4; i++) this.Add(new Letter('L', 1));
            for (int i = 0; i < 2; i++) this.Add(new Letter('M', 3));
            for (int i = 0; i < 6; i++) this.Add(new Letter('N', 1));
            for (int i = 0; i < 8; i++) this.Add(new Letter('O', 1));
            for (int i = 0; i < 2; i++) this.Add(new Letter('P', 3));
            for (int i = 0; i < 1; i++) this.Add(new Letter('Q', 10));
            for (int i = 0; i < 6; i++) this.Add(new Letter('R', 1));
            for (int i = 0; i < 4; i++) this.Add(new Letter('S', 1));
            for (int i = 0; i < 6; i++) this.Add(new Letter('T', 1));
            for (int i = 0; i < 2; i++) this.Add(new Letter('V', 4));
            for (int i = 0; i < 4; i++) this.Add(new Letter('U', 1));
            for (int i = 0; i < 2; i++) this.Add(new Letter('W', 4));
            for (int i = 0; i < 1; i++) this.Add(new Letter('X', 8));
            for (int i = 0; i < 2; i++) this.Add(new Letter('Y', 4));
            for (int i = 0; i < 1; i++) this.Add(new Letter('Z', 10));
            for (int i = 0; i < 2; i++) this.Add(new Letter(' ', 0));
            this.Shuffle();
            this.Add(new Letter());
            //System.Diagnostics.(Debug.)WriteLine("List: " + this.ToString() ); //check if shuffling works well
        }

        ///<summary>Returns true if the list is fully rotated. The list is fully rotated when the null element is at the front.</summary>
        public bool FullyRotated()
        {
            return IsNull(this[0]);
        }

        /// <summary> Returns true when the given element matches the null one. </summary>
        /// <param name="t">The element to be checked.</param>
        public bool IsNull(Letter l) { return l.isNull(); }
    }
}
