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
using System.Collections.Generic;
using System.Security.Cryptography;
using Qubez.Core;

namespace Qubez.Core
{
    /// <summary> A Circular list implementation. </summary>
    public class CircularList<T> : List<T> where T : Elem
    {

        /** Constants related to the direction of the rotating of the list.*/
        public readonly int RIGHT_TURN = -1, LEFT_TURN = 1;

        /// <summary>
        /// Null character used as a flag
        /// </summary>
        public readonly char NULL_CHAR = '-';

        /// <summary> Method that shuffles randomly a list. </summary>
        public void Shuffle()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = this.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = this[k];
                this[k] = this[n];
                this[n] = value;
            }
        }

        /// <summary>Prints all the elements of the LinkedList. </summary>
        public void PrintElements()
        {
            Console.WriteLine(this.ToString());
        }

        /// <summary>
        /// Returns a String that represents this instance.
        /// </summary>
        /// <returns>
        /// A string containing all the elements contained in the list.
        /// </returns>
        public override string ToString()
        {
            string result = "";
            if (this.Count != 0)
            {
                T element;
                for (int i = 0; i < this.Count && (element = this[i]) != null; i++)
                {
                    result += element.ToString() + " ";
                }
            }

            return result;
        }

        /// <summary>Resets the list so that the Null element is at the end. </summary>
        public void Reset()
        {
            int nullPosition = this.GetNullsPosition();
            if (nullPosition >= 0) this.SwapElement(nullPosition, this.RIGHT_TURN);
        }

        /// <summary>
        /// Swaps the i-th element to the end so that it will be after the null value.
        /// </summary>
        /// <param name="i">The position of the element to be swapped.</param>
        /// <param name="turn">The turn.</param>
        /// <returns>Returns the i-th element that was swapped to the end of the list.</returns>
        public T SwapElement(int i, int turn)
        {
            if (this.Count != 0 && i >= 0 && i < this.Count)
            {
                List<T> range = this.GetRange(i, this.Count - i);
                this.RemoveRange(i, this.Count - i);
                this.Rotate(range, turn);
                this.AddRange(range);
                if (turn <= this.RIGHT_TURN)
                    return this[this.Count - 1]; //.peekLast();
                if (turn >= this.LEFT_TURN)
                    return this[0]; //.peekFirst();
            }
            return default(T);
        }

        /// <summary>
        /// Rotates the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="turn">The turn (if positive it will rotate it to the right, otherwise to the left).</param>
        public void Rotate(List<T> list, int turn)
        {
            if (turn > 0 && list.Count != 0)
            {
                //left turn 
                while (turn != 0)
                {
                    T lastElement = list[list.Count - 1];
                    list.RemoveAt(list.Count - 1);
                    list.Insert(0, lastElement);
                    turn--;
                }
            }
            if (turn < 0 && list.Count != 0)
            {
                //right turn
                while (turn != 0)
                {
                    T lastElement = list[0];
                    list.RemoveAt(0);
                    list.Insert(list.Count, lastElement);
                    turn++;
                }
            }
        }

        /// <summary>
        /// Rotates the Elements in the list by "turn" values. If for example we have a list of [1, 2, 3, 4, 5] and we rotate it by 1 it will become [5, 1, 2, 3, 4]
        /// </summary>
        /// <param name="turn">The turn of the rotation.</param>
        /// <returns>Returns the Element that was rotated in the list.</returns>
        public T RotateElements(int turn)
        {
            if (this.Count != 0)
                return this.SwapElement(0, turn);
            return default(T);
        }

        /// <summary>
        /// Retrieves the last element rotated to the end and pushes it back to the front of the list.
        /// </summary>
        /// <returns>Returns the last element previously rotated to the end of the list.</returns>
        public T Undo()
        {
            return this.RotateElements(this.LEFT_TURN);
        }

        /// <summary>
        /// Retrieves the i-th element rotated to the end and pushes it back to the front of the list.
        /// </summary>
        /// <returns>Returns the i-th element previously rotated to the end of the list.</returns>
        public T Undo(int i)
        {
            return this.SwapElement(i, this.LEFT_TURN);
        }

        /// <summary>
        /// Returns the first element and rotates the List.
        /// </summary>
        /// <returns>Returns the first element of the List.</returns>
        public T GetNext()
        {
            return this.RotateElements(this.RIGHT_TURN);
        }

        /// <summary>
        /// Returns true if the list is fully rotated. The list is fully rotated when the null element is at the front.
        /// </summary>
        /// <returns>true if the null element is at the front.</returns>
        public bool FullyRotated()
        {
            return this[0].IsNull();
        }

        /// <summary> Shuffles the list randomly and then resets it so that the null element is at the end.</summary>
        public void ShuffleAndReset()
        {
            this.Shuffle();
            this.Reset();
        }

        /// <summary>
        /// Swaps the i-th element randomly to another one taken from the list up till the null/flag element.
        /// </summary>
        /// <param name="i">The position of the element to be swapped.</param>
        /// <returns>The new element</returns>
        public T SwapRandomlyElement(int i)
        {
            if (i >= 0 && i < this.Count)
            {
                for (int j = i + 1; j < this.Count; j++)
                {
                    if (this[j].IsNull())
                    {
                        int position = new Random().Next(i, j - 1);
                        T tempElem = this[position];
                        this.RemoveAt(position);
                        this.Insert(position, this[i]);
                        this.RemoveAt(i);
                        this.Insert(i, tempElem);
                    }
                }
                return this[i];
            }
            return default(T);
        }

        /// <summary>
        /// Returns the position of the null element.
        /// </summary>
        /// <returns></returns>
        public int GetNullsPosition()
        {
            if (this.Count != 0)
            {
                T temp;
                for (int i = 0; i < this.Count; i++)
                {
                    temp = this[i];
                    if (temp.IsNull())
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
    }
}
