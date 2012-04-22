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
    /// Class representing a simple element of the circular list.
    /// </summary>
    public class Elem
    {

        /// <summary> Returns true when the given element matches the null one. </summary>
        public virtual bool IsNull() { return false; }
    }
}
