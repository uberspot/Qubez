/*
This code is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;

namespace Qubez.Core
{
    /// <summary>
    /// Class representing a Letter.
    /// </summary>
    public class Letter:  Elem, IEquatable<Letter>
    {

        ///<summary> Default constructor </summary>
        public Letter() : this('-', 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Letter"/> class.
        /// </summary>
        /// <param name="ch">The character of the letter.</param>
        public Letter(char ch): this(ch, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Letter"/> class.
        /// </summary>
        /// <param name="ch">The character of the letter.</param>
        /// <param name="points">The points it's worth.</param>
        public Letter(char ch, uint points)
        {
            Char = ch;
            Points = points;
        }

        ///<summary> The character the letter represents. </summary>
        public char Char { get; set; }

        ///<summary> The Letter's points. </summary>
        public uint Points { get; set; }

        /// <summary>
        /// Returns a string that represents the letter.
        /// </summary>
        /// <returns>
        /// The character contained in the letter.
        /// </returns>
        public override string ToString() {
            return this.Char.ToString();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="right">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(Object right) {
            if (Object.ReferenceEquals(right, null))
                return false;
            if (Object.ReferenceEquals(this, right))
                return true;

            if (this.GetType() != right.GetType())
                return false;
            return this.Equals(right as Letter);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Letter other) {
            return other.Char == this.Char;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return this.Char;
        }

        /// <summary>
        /// Returns true when the given element matches the null one.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if the specified l is null; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsNull() { return this.Char == '-'; }
    }
}


