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

namespace Qubez.Core {

    /// <summary>
    /// Represents a change on the board. Consists of the letter added and it's coordinates. 
    /// </summary>
    public class Change : Elem{

        /// <summary>
        /// Initializes a new instance of the <see cref="Change"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="addedLetter">The added letter.</param>
        public Change(uint x, uint y, Letter addedLetter) {
            X = x;
            Y = y;
            AddedLetter = addedLetter;
        }

        /// <summary>The X coordinate of the changed letter. </summary>
        public uint X {get; set; }

        /// <summary>The Y coordinate of the changed letter. </summary>
        public uint Y {get; set; }

        /// <summary>
        /// Gets or sets the added letter.
        /// </summary>
        /// <value>The added letter.</value>
        public Letter AddedLetter { get; set; }

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
            return this.Equals(right as Change);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Change other) {
            return (other.X == this.X) && (other.Y==this.Y) && (other.AddedLetter==this.AddedLetter);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return ((int) (X*7 +Y)*3) +  AddedLetter.Char.GetHashCode() *2;
        }

        /// <summary>
        /// Returns true when the given element matches the null one.
        /// </summary>
        /// <returns></returns>
        public override bool IsNull() {
            return this.AddedLetter.IsNull() || this.AddedLetter.Char==' ';
        }
    }
}

