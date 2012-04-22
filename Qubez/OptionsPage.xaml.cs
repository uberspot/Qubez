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
using System.IO.IsolatedStorage;
using System.Xml.Linq;

namespace Qubez
{
    public partial class OptionsPage : PhoneApplicationPage
    {
        private readonly string BOARD_FILE = "Board.xml";

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsPage"/> class.
        /// </summary>
        public OptionsPage()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                LoadOptions();
            };
        }

        private void LoadOptions()
        {
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
                            ChangeValue(txtSize, int.Parse(boardSize));
                            ChangeValue(txtPlayers, int.Parse(players));
                            ChangeValue(txtLetters, int.Parse(letters));
                            cbxUseDictionary.IsChecked = bool.Parse(useDictionary);
                        }
                        catch
                        {
                            ChangeValue(txtSize, 10);
                            ChangeValue(txtPlayers, 5);
                            ChangeValue(txtLetters, 7);
                            cbxUseDictionary.IsChecked = true;
                        }
                    }
                    else
                    {
                        ChangeValue(txtSize, 10);
                        ChangeValue(txtPlayers, 5);
                        ChangeValue(txtLetters, 7);
                        cbxUseDictionary.IsChecked = true;
                    }
                }
            }
        }

        private void btnIncreaseSize_Click(object sender, RoutedEventArgs e)
        {
            int size = int.Parse(txtSize.Text);
            if (size < 15)
            {
                ChangeValue(txtSize, size + 1);
            }
        }

        private void btnDecreaseSize_Click(object sender, RoutedEventArgs e)
        {
            int size = int.Parse(txtSize.Text);
            if (size > 1)
            {
                ChangeValue(txtSize, size - 1);
            }
        }

        private void btnIncreasePlayers_Click(object sender, RoutedEventArgs e)
        {
            int players = int.Parse(txtPlayers.Text);
            if (players < 10)
            {
                ChangeValue(txtPlayers, players + 1);
            }
        }

        private void btnDecreasePlayers_Click(object sender, RoutedEventArgs e)
        {
            int players = int.Parse(txtPlayers.Text);
            if (players > 2)
            {
                ChangeValue(txtPlayers, players - 1);
            }
        }

        private void btnIncreaseLetters_Click(object sender, RoutedEventArgs e)
        {
            int letters = int.Parse(txtLetters.Text);
            if (letters < 7)
            {
                ChangeValue(txtLetters, letters + 1);
            }
        }

        private void btnDecreaseLetters_Click(object sender, RoutedEventArgs e)
        {
            int letters = int.Parse(txtLetters.Text);
            if (letters > 1)
            {
                ChangeValue(txtLetters, letters - 1);
            }
        }

        private void ChangeValue(TextBox txt, int value)
        {
            txt.Text = value.ToString();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            XDocument document = new XDocument(
                new XDeclaration("1.0", "utf-8", "true"),
                new XElement("board",
                    new XElement("size", txtSize.Text),
                    new XElement("maxPlayers", txtPlayers.Text),
                    new XElement("letters", txtLetters.Text),
                    new XElement("useDictionary", (cbxUseDictionary.IsChecked == true ? "true" : "false"))));

            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = storage.OpenFile("Board.xml", System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite))
                {
                    document.Save(stream);
                }
            }
            NavigationService.GoBack();
        }
    }
}