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
using System.Xml.Linq;
using System.IO.IsolatedStorage;

namespace Qubez
{
    /// <summary>
    /// A page adding the choice to add more users to the game.
    /// </summary>
    public partial class AddPlayersPage : PhoneApplicationPage
    {
        private readonly string BOARD_FILE = "Board.xml";

        /// <summary>
        /// Initializes a new instance of the <see cref="AddPlayersPage"/> class.
        /// </summary>
        public AddPlayersPage()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(AddPlayersPage_Loaded);
        }

        /// <summary>
        /// Handles the Loaded event of the AddPlayersPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        void AddPlayersPage_Loaded(object sender, RoutedEventArgs e)
        {
            int maxNumberOfPlayers;

            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                bool exists = storage.FileExists(BOARD_FILE);
                using (IsolatedStorageFileStream stream = storage.OpenFile(BOARD_FILE, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite))
                {
                    if (exists && stream.Length != 0) 
                    {
                        XDocument document = XDocument.Load(stream);
                        var boardSize = (from node in document.Descendants("size")
                                         select node.Value).FirstOrDefault();
                        var players = (from node in document.Descendants("maxPlayers")
                                       select node.Value).FirstOrDefault();
                        var letters = (from node in document.Descendants("letters")
                                       select node.Value).FirstOrDefault();
                        var useDictionary = (from node in document.Descendants("useDictionary")
                                             select node.Value).FirstOrDefault();


                        try
                        {
                            Globals.BoardSize = int.Parse(boardSize);
                            maxNumberOfPlayers = int.Parse(players);
                            Globals.NumberOfLetters = int.Parse(letters);
                            Globals.UseDictionary = bool.Parse(useDictionary);
                        }
                        catch
                        {
                            Globals.BoardSize = 10;
                            maxNumberOfPlayers = 5;
                            Globals.NumberOfLetters = 7;
                            Globals.UseDictionary = true;
                        }
                    }
                    else
                    {
                        Globals.BoardSize = 10;
                        maxNumberOfPlayers = 5;
                        Globals.NumberOfLetters = 7;
                        Globals.UseDictionary = true;
                    }
                }
            }

            // Generate the items list on the fly...
            List<StackPanel> items = new List<StackPanel>();
            for (int index = 2; index <= maxNumberOfPlayers; index++)
            {
                TextBlock tbl = new TextBlock { FontSize = 30, Text = string.Format("Player {0}", index) };
                TextBox txt = new TextBox
                {
                    Text = string.Empty,
                    Width = 450,
                    MaxLength = 40,
                    InputScope = new InputScope() { Names = { new InputScopeName { NameValue = InputScopeNameValue.Url } } }
                }; // It simply works, folk!
                txt.KeyDown += (s, evt) =>
                {
                    if (evt.Key == Key.Enter)
                    {
                        lbPlayers.Focus();
                    }
                };

                StackPanel sp = new StackPanel
                {
                    Orientation = System.Windows.Controls.Orientation.Vertical,
                    Children = { tbl, txt }
                };

                items.Add(sp);
            }

            lbPlayers.ItemsSource = items;
        }

        /// <summary>
        /// Handles the Click event of the ApplicationBarIconButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            List<string> names = GetValidNames();
            if (names.Count != 1 && !HasDuplicateNames(names))
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Please, provide one or more non-empty and unique usernames.", "Invalid names", MessageBoxButton.OK);
            }

            Globals.GameController = new GameController(false, names, (uint)Globals.BoardSize);
        }

        /// <summary>
        /// Gets the valid names entered in the textboxes.
        /// </summary>
        /// <returns></returns>
        private List<string> GetValidNames()
        {
            List<string> names = new List<string> { Globals.FirstPlayerName };

            ItemCollection items = lbPlayers.Items;
            for (int index = 0; index < items.Count; index++)
            {
                StackPanel sp = items[index] as StackPanel;
                TextBox txt = sp.Children[1] as TextBox;
                if (!string.IsNullOrEmpty(txt.Text.Trim()))
                {
                    names.Add(txt.Text.Trim());
                }
            }

            return names;
        }

        /// <summary>
        /// Determines whether [has duplicate] [in the specified names].
        /// </summary>
        /// <param name="names">The names to be checked.</param>
        /// <returns>
        ///   <c>true</c> if [has duplicates] [in the specified names]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasDuplicateNames(List<string> names)
        {
            for (byte i = 0; i < names.Count; i++)
            {
                for (byte j = 0; j < names.Count; j++)
                {
                    if (i != j && names[i] == names[j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}