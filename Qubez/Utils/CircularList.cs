using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Qubez
{
    /// <summary> A Circular list. </summary>
    public class CircularList<T> : List<T>
    {

        /** Constants related to the direction of the rotating of the list.*/
        public readonly int RIGHT_TURN = -1, LEFT_TURN = 1;

        public readonly char NULL_CHAR = '-';

        /// <summary> Static method that shuffles randomly a list. </summary>
        /// <typeparam name="T"> The type of the elements in the list.</typeparam>
        /// <param name="list"> The list to be shuffled. </param>
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
            if (this.Count != 0)
            {
                T element;
                for (int i = 0; i < this.Count && (element = this[i]) != null; i++)
                {
                    Console.WriteLine(element + " ");
                }
            }
        }

        /// <summary> Returns true when the given element matches the null one. </summary>
        /// <param name="t">The element to be checked.</param>
        public virtual bool IsNull(T t)
        {
            return true;
        }

        /// <summary>Resets the list so that the Null element is at the end. </summary>
        public void Reset()
        {
            if (this.Count != 0)
            {
                T temp;
                for (int i = 0; i < this.Count; i++)
                {
                    temp = this[i];
                    if (this.IsNull(temp))
                    {
                        //this.swapElement(i, Elements.RIGHT_TURN);
                    }
                }
            }
        }

        /// <summary> Shuffles the list randomly and then resets it so that the null element is at the end.</summary>
        public void ShuffleAndReset()
        {
            this.Shuffle();
            this.Reset();
        }
    }
}
