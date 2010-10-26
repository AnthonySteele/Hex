Hex is an implementation in C# of the board game "Hex" as described here:
http://en.wikipedia.org/wiki/Hex_%28board_game%29

The board is made up of hexagonal cells, and the red and blue players take turns to fill a cell on the board, both try to win by connecting thier opposite sides.
Only one player can suceed in this, since connecting opposite sides blocks connection between the other pair of opposite sides. In terms of style of play and complexity, it is somewheer between Tic-tac-toe and Go.
 
The player can play against a computer oponent, or two people can use the on-screen board. The computer player uses the Minimax algorithm with alpha-beta pruning. 
See: http://en.wikipedia.org/wiki/Minimax http://en.wikipedia.org/wiki/Alpha-beta_pruning

The computer player can be slow, especially if the board is large and the skill level (i.e. lookahead depth) is high.

The user interface is in WPF.

Licence: 
Hex is open source, and is made available under the terms of the Microsoft Reciprocal License (Ms-RL).
This licence can be found in the included file "License.txt"
Or online at  http://www.opensource.org/licenses/ms-rl.html

History:
I have tinkered with this code on several occasions. It was the first non-trivial program that I wrote in C#. I updated it to try out generic lists when C# 2.0 came out. In 2010 I made a WPF interface, reformatted the code as per StyleCop, made more test cases and revisited the minimax algorithm, and finally upload it to github and release it under an opehn licence. 

Anthony Steele