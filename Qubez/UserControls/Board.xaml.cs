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
using System.Xml.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;

namespace Qubez.UserControls
{
    /// <summary>
    /// A user control representing tha game board.
    /// </summary>
    public partial class Board : UserControl
    {
        #region Constants

        private readonly int BOARD_SIZE;
        private readonly SolidColorBrush WHITE_BRUSH = new SolidColorBrush(Colors.White);
        private readonly LinearGradientBrush RECTANGLE_BRUSH = new LinearGradientBrush
        {
            GradientStops = new GradientStopCollection
            {
                new GradientStop { Color = Color.FromArgb(255, 216, 164, 252) },
                new GradientStop { Color = Colors.Black, Offset = 2}
            }
        };

        private readonly LinearGradientBrush RECTANGLE_DARK_BRUSH = new LinearGradientBrush {
            GradientStops = new GradientStopCollection
            {
                new GradientStop { Color = Color.FromArgb(255, 134, 102, 156) },
                new GradientStop { Color = Colors.Black, Offset = 2}
            }
        };

        private readonly LinearGradientBrush RED_BRUSH = new LinearGradientBrush
        {
            GradientStops = new GradientStopCollection
            {
                new GradientStop { Color = Color.FromArgb(255, 251, 167, 167) },
                new GradientStop { Color = Colors.Black, Offset = 2}
            }
        };

        readonly TimeSpan TAP_THRESHOLD = new TimeSpan(0, 0, 0, 0, 300); // ms

        #endregion

        #region Members

        private double _previousCellX;
        private double _previousCellY;

        DateTime _lastClick = DateTime.Now;
        bool _isFirstClick = false;

        Point _clickPosition;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Board"/> class.
        /// </summary>
        public Board()
        {
            InitializeComponent();

            BOARD_SIZE = Globals.BoardSize;
            CreateBoard();
            FillBoard();
            _previousCellX = _previousCellY = -1;
        }

        #endregion

        #region Private Methods

        private void CreateBoard()
        {
            double cellSize = 0.09;

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                LayoutRoot.RowDefinitions.Add(new RowDefinition { Height = new GridLength(cellSize, GridUnitType.Star) });
                LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(cellSize, GridUnitType.Star) });
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Fills the board with the letters from GameController.Board.
        /// </summary>
        public void FillBoard()
        {
            for (uint rowIndex = 0; rowIndex < LayoutRoot.RowDefinitions.Count; rowIndex++)
            {
                for (uint columnIndex = 0; columnIndex < LayoutRoot.ColumnDefinitions.Count; columnIndex++)
                {
                    Rectangle rectangle = new Rectangle(BOARD_SIZE)
                    {
                        CellX = columnIndex,
                        CellY = rowIndex
                    };

                    if (Globals.GameController.Board.GetHeight(columnIndex, rowIndex) != 0) {
                        rectangle.Background = RECTANGLE_DARK_BRUSH;
                    }else if (Globals.GameController.Board.IsARedSquare(columnIndex, rowIndex)) {
                        rectangle.Background = RED_BRUSH;
                    }

                    rectangle.DataBind((byte)Globals.GameController.Board.GetHeight(columnIndex, rowIndex), Globals.GameController.Board.GetTopLetter(columnIndex, rowIndex).Char);

                    rectangle.SetValue(Grid.RowProperty, (int)rowIndex);
                    rectangle.SetValue(Grid.ColumnProperty, (int)columnIndex);
                    LayoutRoot.Children.Add(rectangle);
                }
            }
        }

        /// <summary>
        /// Highlights the cells underneath the draggable object's coordinates.
        /// </summary>
        /// <param name="cellX">The cell X.</param>
        /// <param name="cellY">The cell Y.</param>
        public void HighlightCells(double cellX, double cellY)
        {
            foreach (Rectangle rectangle in LayoutRoot.Children)
            {
                //Highlight the appropriate cell.
                if (rectangle.CellX == cellX && rectangle.CellY == cellY) {
                    rectangle.Background = WHITE_BRUSH;
                }
                else if (rectangle.CellX == _previousCellX && rectangle.CellY == _previousCellY) {
                    if (Globals.GameController.Board.GetHeight(rectangle.CellX, rectangle.CellY) != 0) {
                        rectangle.Background = RECTANGLE_DARK_BRUSH;
                    }
                    else {
                        rectangle.Background = (Globals.GameController.Board.IsARedSquare((uint) _previousCellX, (uint) _previousCellY)) ? RED_BRUSH : RECTANGLE_BRUSH;
                    }
                }
            }

            _previousCellX = cellX;
            _previousCellY = cellY;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when [a popup message needs to be displayed].
        /// </summary>
        /// <param name="sender">The sender of the message.</param>
        /// <param name="message">The message to be displayed.</param>
        public delegate void OnDisplayPopUpHandler(object sender, string message);
        
        /// <summary>
        /// Occurs when [a popup message needs to be displayed].
        /// </summary>
        public event OnDisplayPopUpHandler OnDisplayPopup;

        #endregion

        #region Event handlers

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseLeftButtonDown"/> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (DateTime.Now - _lastClick > TAP_THRESHOLD || _isFirstClick == false)
            {
                _clickPosition = e.GetPosition(this);
                _isFirstClick = true;
                _lastClick = DateTime.Now;
            }
            else
            {
                Point position = e.GetPosition(this);
                if (Math.Abs(_clickPosition.X - position.X) < 480 / Globals.BoardSize && Math.Abs(_clickPosition.Y - position.Y) < 480 / Globals.BoardSize)
                {
                    foreach (Rectangle rectangle in LayoutRoot.Children)
                    {
                        if (rectangle.CellX < position.X / rectangle.ActualWidth && rectangle.CellX + 1 > position.X / rectangle.ActualWidth
                            && rectangle.CellY < position.Y / rectangle.ActualHeight && rectangle.CellY + 1 > position.Y / rectangle.ActualHeight)
                        {
                            if (OnDisplayPopup != null)
                            {
                                string list = Globals.GameController.Board.CellToString(rectangle.CellX, rectangle.CellY).Trim();
                                OnDisplayPopup(this, string.IsNullOrEmpty(list) ? "Empty" : list);
                            }

                            break;
                        }
                    }
                }

                _isFirstClick = false;
            }

            base.OnMouseLeftButtonDown(e);
        }

        #endregion
    }
}
