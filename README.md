Qubez
==================

Qubez is a word game in which 2 or more players score points by 
forming words from individual letters on a square 3D gameboard. The 
words are formed across and down in crossword fashion. The goal
of the game is to get the most points at the end of the game.

The game was developed by Pavlos Sarbinowski and Vangos Pterneas 
as part of the Human Computer Interaction course in A.U.E.B.

Rules
------

The first player must form a horizontal or vertical word with at 
least one letter on one of the red squares at the middle of the 
board. In each consecutive round a player can form a new word
by either 

 *  connecting a new word to any letter of an existing word on the board

or by

 *  stacking one or more letters on top of an existing word on the board.

Notes: 

 * You can't place a new letter on an existing one that's the same. 
(e.g. putting the letter 'A' on top of letter 'A')
 * If you connect new letters to an existing word on the board each 
newly created word must be valid.
 * You can't stack more than five letters on each square.

You can choose to do automatic dictionary checks on each new word via 
the settings. In general the following categories of words are not allowed:

 * Names
 * Acronyms
 * Words that need an apostrophe (')
 * Words that need a dash (-)
 * Definite articles

If you cannot form a word on your round you can either pass your turn
or you can change one of the letters you have in front of you with a random
new one. If you swap a letter you lose your turn.

The game ends when:

 * one of the players has no more letters to use
 * no player can form any more words
 * each player passes his turn to the next one in a round

The player with the highest score wins!

License
-------

    Qubez is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
    
    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.
    
    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
