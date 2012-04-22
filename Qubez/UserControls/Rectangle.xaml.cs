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
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Qubez.UserControls
{
    /// <summary>
    /// User control representing a rectangle to be drawn in the board.
    /// </summary>
    public partial class Rectangle : UserControl
    {
        #region Properties

        /// <summary>
        /// Represents the number of the elements in a game stack.
        /// </summary>
        public byte Number { get; set; }

        /// <summary>
        /// Represents the letter of the specific control.
        /// </summary>
        public char Letter { get; set; }

        /// <summary>
        /// Represents the letter font size of the specific control.
        /// </summary>
        public double LetterSize { get; set; }

        /// <summary>
        /// Rectangle's horizontal index.
        /// </summary>
        public uint CellX { get; set; }

        /// <summary>
        /// Rectangle's vertical index.
        /// </summary>
        public uint CellY { get; set; }

        /// <summary>
        /// Rectangle's background.
        /// </summary>
        /// <returns>The brush that provides the background of the control. The default is null.</returns>
        public Brush Background
        {
            get { return spBackground.Background; }
            set { spBackground.Background = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Rectangle"/> class.
        /// </summary>
        /// <param name="numberOfCells">The number of cells.</param>
        public Rectangle(int numberOfCells)
        {
            InitializeComponent();

            LetterSize = 0.783 * (480 / numberOfCells);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Datas the bind.
        /// </summary>
        /// <param name="number">The number of levels in the cell.</param>
        /// <param name="letter">The letter.</param>
        public void DataBind(byte number, char letter)
        {
            txtNumber.Visibility = number == 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;

            Number = number;
            Letter = letter;

            DataContext = this;
        }

        #endregion
    }
}
