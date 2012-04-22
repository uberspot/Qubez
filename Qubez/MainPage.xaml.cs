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
using Microsoft.Phone.Controls;
using Qubez.Core;
using Qubez.UserControls;
using System.Threading;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;

namespace Qubez
{
    /// <summary>
    /// The main game page (containing the board etc).
    /// </summary>
    public partial class MainPage : PhoneApplicationPage
    {
        private bool _isVerifying;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            _isVerifying = false;
            gameBoard.OnDisplayPopup += new UserControls.Board.OnDisplayPopUpHandler(OnDisplayPopup);

            Globals.GameController.DraggablesDefined += new GameController.DraggablesDefinedHandler(GameController_DraggablesDefined);
            Globals.GameController.OnRenewPlayers += new GameController.OnRenewPlayersHandler(GameController_OnRenewPlayers);
            Globals.GameController.OnDisplayPopup += new GameController.OnDisplayPopUpHandler(OnDisplayPopup);
            Globals.GameController.WinnerFound += new GameController.WinnerFoundHandler(GameController_WinnerFound);

            Loaded += (s, e) =>
            {
                Globals.GameController.BeginGame();
            };

        }

        #endregion

        #region Event handlers

        void GameController_DraggablesDefined(object sender, DraggableEventArgs e)
        {
            if (e.DeleteExistingDraggables)
            {
                for (int i = 0; i < gameCanvas.Children.Count; i++)
                {
                    UIElement element = gameCanvas.Children[i];
                    if (element is Draggable)
                    {
                        gameCanvas.Children.Remove(element);
                        i--;
                    }
                }
                gameBoard.HighlightCells(-1, -1);
            }
            if (e.DisplayNewDraggables && e.Letters != null && e.Letters.Count != 0)
            {
                uint positionLeft = 0;
                foreach (Letter letter in e.Letters)
                {
                    if (letter != null)
                    {
                        Draggable draggable = new Draggable(letter);

                        Canvas.SetTop(draggable, 553);
                        Canvas.SetLeft(draggable, positionLeft);

                        draggable.CellFound += new Draggable.CellFoundHandler(Draggable_CellFound);
                        draggable.CellPositioned += new Draggable.CellPositionedHandler(Draggable_CellPositioned);
                        draggable.OnDisplayPopup += new Draggable.OnDisplayPopUpHandler(OnDisplayPopup);
                        draggable.HighlightChanged += new Draggable.HighlightChangedHandler(Draggable_HighlightChanged);

                        gameCanvas.Children.Add(draggable);

                        positionLeft += 68;
                    }
                }
            }
        }

        void GameController_OnRenewPlayers(object sender, EventArgs e)
        {
            playersList.FillPlayers();
        }

        void GameController_WinnerFound(object sender, string name)
        {
            NavigationService.Navigate(new Uri("/GameOverPage.xaml?winner=" + name, UriKind.Relative));
        }

        void OnDisplayPopup(object sender, string message)
        {
            MessageBox.Show(message);
        }

        private void Draggable_CellFound(object sender, CellArgs e)
        {
            gameBoard.HighlightCells(e.X, e.Y);
        }

        private void Draggable_CellPositioned(object sender, CellArgs e)
        {
            Draggable moved = sender as Draggable;
            foreach (UIElement element in gameCanvas.Children)
            {
                if (element is Draggable)
                {
                    Draggable drag = element as Draggable;
                    if (drag != moved && drag.CellX == moved.CellX && drag.CellY == moved.CellY)
                    {
                        infoBox.tblmessage.Text = "Can't add a letter on a newly added letter.";
                        moved.SnapToInitialPosition();
                    }
                }
            }
        }

        void Draggable_HighlightChanged(object sender, EventArgs e)
        {
            foreach (UIElement element in gameCanvas.Children)
            {
                if (element is Draggable)
                {
                    Draggable drag = element as Draggable;
                    if (drag.HasBorderHighlighted)
                    {
                        drag.HasBorderHighlighted = false;
                    }
                }
            }
            Draggable changed = sender as Draggable;

            changed.Letter = Globals.GameController.SwapLetter(changed.Letter);
            

        }

        private void BtnSwapLetter_Click(object sender, EventArgs e)
        {
            if (!_isVerifying)
            {
                infoBox.tblmessage.Text = "Choose a letter to swap with a new one.";
                foreach (UIElement element in gameCanvas.Children)
                {
                    if (element is Draggable)
                    {
                        Draggable drag = element as Draggable;
                        if (drag.CellX == uint.MaxValue && drag.CellY == uint.MaxValue && !drag.HasBorderHighlighted)
                        {
                            drag.HasBorderHighlighted = true;
                        }
                        else
                        {
                            drag.HasBorderHighlighted = false;
                            infoBox.tblmessage.Text = string.Empty;
                        }
                    }
                }
            }
        }

        private void BtnLoseTurn_Click(object sender, EventArgs e)
        {
            if (!_isVerifying)
                Globals.GameController.PassTurn(true);
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (!_isVerifying)
            {
                foreach (UIElement element in gameCanvas.Children)
                {
                    if (element is Draggable)
                    {
                        Draggable drag = element as Draggable;
                        if (drag.CellX != uint.MaxValue && drag.CellY != uint.MaxValue)
                        {
                            Globals.GameController.ApplyChange(new Change(drag.CellX, drag.CellY, drag.Letter));

                            
                        }
                    }
                }
                if (Globals.GameController.Changes.Count != 0 && Globals.GameController.IsFinalWordValid())
                {
                    infoBox.tblmessage.Text = "Waiting for verification by other players...";
                    GameController_DraggablesDefined(this, new DraggableEventArgs(null, true, false));

                    VerificationButtons verificationBtns = new VerificationButtons();
                    verificationBtns.OnVerification += (s, decision) =>
                    {
                        bool accepted = decision;
                        gameCanvas.Children.Remove(verificationBtns);
                        _isVerifying = false;
                        if (accepted)
                        {
                            Globals.GameController.ChangeTurn();
                            gameBoard.FillBoard();
                            if (Globals.GameController.IsFirstTime)
                                Globals.GameController.IsFirstTime = false;
                        }
                        else
                        {
                            Globals.GameController.PassTurn(false);
                            gameBoard.FillBoard();
                        }
                    };

                    Canvas.SetTop(verificationBtns, 553);
                    Canvas.SetLeft(verificationBtns, 0);
                    gameBoard.FillBoard();
                    _isVerifying = true;
                    gameCanvas.Children.Add(verificationBtns);
                }
            }
        }

        private void ItemShareTwitter_Click(object sender, EventArgs e)
        {
            if (!_isVerifying)
            {
                string names = string.Empty;
                foreach (Player player in Globals.GameController.Players)
                {
                    names += ", " + player.Name;
                }
                names = names.Remove(0, 2);

                twitter.Navigate("http://twitter.com/?status=" + names + " playing Qubez!");
                twitter.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void ItemScreenshot_Click(object sender, EventArgs e)
        {
            if (!_isVerifying)
            {
                WriteableBitmap screenshot = new WriteableBitmap(480, 800);
                screenshot.Render(ContentPanel, null);

                Globals.Screenshot = screenshot;

                NavigationService.Navigate(new Uri("/GameOverPage.xaml", UriKind.Relative));
            }
        }

        #endregion
    }
}