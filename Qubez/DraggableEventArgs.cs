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
using Qubez.Core;

namespace Qubez
{
    /// <summary>
    /// Event Arguments for the DraggableSefined event
    /// </summary>
    public class DraggableEventArgs: EventArgs
    {
        /// <summary>
        /// Gets or sets the letters.
        /// </summary>
        /// <value>
        /// The list of letters given as argument for the event.
        /// </value>
        public List<Letter> Letters { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [delete existing draggables].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [delete existing draggables]; otherwise, <c>false</c>.
        /// </value>
        public bool DeleteExistingDraggables { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [display new draggables].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [display new draggables]; otherwise, <c>false</c>.
        /// </value>
        public bool DisplayNewDraggables { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DraggableEventArgs"/> class.
        /// </summary>
        /// <param name="letters">The letters.</param>
        public DraggableEventArgs(List<Letter> letters)
            : this(letters, true, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DraggableEventArgs"/> class.
        /// </summary>
        /// <param name="letters">The letters.</param>
        /// <param name="deleteExistingDraggables">if set to <c>true</c> [delete existing draggables].</param>
        /// <param name="displayNewDraggables">if set to <c>true</c> [display new draggables].</param>
        public DraggableEventArgs(List<Letter> letters, bool deleteExistingDraggables, bool displayNewDraggables)
        {
            Letters = letters;
            DeleteExistingDraggables = deleteExistingDraggables;
            DisplayNewDraggables = displayNewDraggables;
        }
    }
}
