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
using Qubez.Core;

namespace Qubez.UserControls
{
    /// <summary>
    /// A user control representing a draggable rectangle for putting letters on the board
    /// </summary>
    public partial class Draggable : UserControl
    {
        #region Constants

        readonly TimeSpan TAP_THRESHOLD = new TimeSpan(0, 0, 0, 0, 300); // ms

        private readonly SolidColorBrush RED_BRUSH = new SolidColorBrush(Color.FromArgb(255, 176, 0, 0));

        private readonly SolidColorBrush DEFAULT_BRUSH = new SolidColorBrush(Color.FromArgb(255, 24, 24, 24));

        #endregion

        #region Private Members

        Letter _letter;

        bool _isMouseCaptured;
        double _positionX;
        double _positionY;

        double _initialPositionX;
        double _initialPositionY;

        double _originalSize;

        uint _cellSize;

        DateTime _lastClick = DateTime.Now;
        bool _isFirstClick = false;

        Point _clickPosition;

        bool _hasBorderHighlighted;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cell X.
        /// </summary>
        /// <value>
        /// The cell X.
        /// </value>
        public uint CellX { get; protected set; }

        /// <summary>
        /// Gets or sets the cell Y.
        /// </summary>
        /// <value>
        /// The cell Y.
        /// </value>
        public uint CellY { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has border highlighted.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has border highlighted; otherwise, <c>false</c>.
        /// </value>
        public bool HasBorderHighlighted
        {
            get { return _hasBorderHighlighted; }
            set
            {
                _hasBorderHighlighted = value;
                border.BorderBrush = _hasBorderHighlighted ? RED_BRUSH : DEFAULT_BRUSH;
            }
        }

        /// <summary>
        /// Defines whether this control is draggable in a specific moment.
        /// </summary>
        public bool IsDraggable { get; set; }

        /// <summary>
        /// Represents the letter of the specific control.
        /// </summary>
        public Letter Letter
        {
            get { return _letter; }

            set
            {
                _letter = value;
                tblLetter.Text = _letter.Char.ToString();
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Draggable"/> class.
        /// </summary>
        public Draggable(Letter letter)
        {
            InitializeComponent();
            IsDraggable = true;
            Letter = letter;
            DataBind();
            CellX = uint.MaxValue;
            CellY = uint.MaxValue;

            Loaded += new RoutedEventHandler(Draggable_Loaded);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Snaps the draggable to it's initial position.
        /// </summary>
        public void SnapToInitialPosition()
        {
            SnapToGrid(_initialPositionX, _initialPositionY - 104, _originalSize);
        }

        /// <summary>
        /// Snaps the draggable to the given coordinates and size.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="size">The size.</param>
        public void SnapToGrid(double x, double y, double size)
        {
            PositionElement(x, y);
            ResizeElement((uint)size);
            HighlightCells();
        }

        /// <summary>
        ///Binds the data.
        /// </summary>
        public void DataBind()
        {
            DataContext = this;
        }

        /// <summary>
        /// Determines whether the object is inside bounds.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if [is inside bounds]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInsideBounds()
        {
            return !(_positionY < 104 || _positionY > 584);
        }

        /// <summary>
        /// Positions the element on the board.
        /// </summary>
        /// <param name="positionX">The position X.</param>
        /// <param name="positionY">The position Y.</param>
        public void PositionElement(double positionX, double positionY)
        {
            this.SetValue(Canvas.LeftProperty, positionX);
            this.SetValue(Canvas.TopProperty, positionY);
            this.SetValue(Canvas.ZIndexProperty, ++Globals.ZIndex);
        }

        /// <summary>
        /// Resizes the element.
        /// </summary>
        /// <param name="newSize">The new size.</param>
        public void ResizeElement(uint newSize)
        {
            border.Width = border.Height = newSize;
            tblLetter.FontSize = 0.751 * newSize;
        }

        #endregion

        #region Events

        /// <summary>
        /// Called then the cell is positioned.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        public delegate void CellPositionedHandler(object sender, CellArgs e);

        /// <summary>
        /// Occurs when the cell is positioned.
        /// </summary>
        public event CellPositionedHandler CellPositioned;

        /// <summary>
        /// Called when a cell is found underneath the draggable object.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        public delegate void CellFoundHandler(object sender, CellArgs e);

        /// <summary>
        /// Occurs when [cell found].
        /// </summary>
        public event CellFoundHandler CellFound;

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

        /// <summary>
        /// Handles the changing of the highlighting.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public delegate void HighlightChangedHandler(object sender, EventArgs e);
        /// <summary>
        /// Occurs when [highlight changed].
        /// </summary>
        public event HighlightChangedHandler HighlightChanged;

        #endregion

        #region Event Handlers

        void Draggable_Loaded(object sender, RoutedEventArgs e)
        {
            GeneralTransform gt = this.TransformToVisual(Application.Current.RootVisual as UIElement);
            _initialPositionX = gt.Transform(new Point(0, 0)).X;
            _initialPositionY = gt.Transform(new Point(0, 0)).Y;
            _originalSize = border.Width;
        }

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
                
                if (_hasBorderHighlighted)
                {
                    if (HighlightChanged != null)
                    {
                        HighlightChanged(this, new EventArgs());
                    }
                }
            }
            else
            {
                Point position = e.GetPosition(this);
                if (Math.Abs(_clickPosition.X - position.X) < border.Width && Math.Abs(_clickPosition.Y - position.Y) < border.Height)
                {
                    if (OnDisplayPopup != null)
                    {
                        OnDisplayPopup(this, "Letter " + Letter.Char + " is worth " + ((Letter.Points <= 1) ? Letter.Points + " point." : Letter.Points + " points."));
                    }
                }

                _isFirstClick = false;
            }

            if (!IsDraggable) return;

            _isMouseCaptured = true;
            _positionY = e.GetPosition(null).Y;
            _positionX = e.GetPosition(null).X;

            CaptureMouse();

            base.OnMouseLeftButtonDown(e);
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseLeftButtonUp"/> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (!IsDraggable) return;

            HandleCollisions();

            _isMouseCaptured = false;
            _positionY = -1;
            _positionX = -1;

            base.OnMouseLeftButtonUp(e);
        }

        /// <summary>
        /// Called before the <see cref="E:System.Windows.UIElement.MouseMove"/> event occurs.
        /// </summary>
        /// <param name="e">The data for the event.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!IsDraggable) return;

            if (_isMouseCaptured)
            {
                // Calculate the current position of the object.
                double deltaV = e.GetPosition(null).Y - _positionY;
                double deltaH = e.GetPosition(null).X - _positionX;
                double newTop = deltaV + (double)this.GetValue(Canvas.TopProperty);
                double newLeft = deltaH + (double)this.GetValue(Canvas.LeftProperty);

                // Set new position of object.
                SnapToGrid(newLeft, newTop, _originalSize);

                // Update position global variables.
                _positionY = e.GetPosition(null).Y;
                _positionX = e.GetPosition(null).X;
            }

            base.OnMouseMove(e);
        }

        #endregion

        #region Collisions

        private void HandleCollisions()
        {
            if (IsInsideBounds()) // ---> The Draggable is inside the board's bounds.
            {
                DetectBottomCell();
                // So, the Draggable is over [cellX, cellY] cell.

                SnapToGrid(CellX * _cellSize, CellY * _cellSize, _cellSize);

                // Fire the event!
                if (CellPositioned != null)
                {
                    CellPositioned(this, new CellArgs { X = CellX, Y = CellY });
                }
            }
            else // ---> The Draggable is out of the board's bounds.
            {
                SnapToInitialPosition();
            }
        }

        private void HighlightCells()
        {
            if (IsInsideBounds())
            {
                DetectBottomCell();

                // Fire the event!
                if (CellFound != null)
                {
                    CellFound(this, new CellArgs { X = CellX, Y = CellY });
                }
            }
            else
            {
                CellX = uint.MaxValue;
                CellY = uint.MaxValue;
                // Fire the event!
                if (CellFound != null)
                {
                    CellFound(this, new CellArgs { X = -1, Y = -1 });
                }
            }
        }

        private void DetectBottomCell()
        {
            _cellSize = 480 / Globals.GameController.Board.Size;
            if (IsInsideBounds())
            {
                CellX = (uint)Math.Floor((_positionX) / _cellSize);
                CellY = (uint)Math.Floor((_positionY - 104) / _cellSize);
            }
        }

        #endregion
    }
}
