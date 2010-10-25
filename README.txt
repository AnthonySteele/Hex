Hex is an implementation in C# of the game of Hex as described here:
http://en.wikipedia.org/wiki/Hex_%28board_game%29

The red and blue players take turns to fill a hex, both try to win by connecting thier opposite sides.
Only one player can suceed in this.
 
The player can play against a computer oponent, or two people can use the on-screen board. The computer player uses the Minimax algorithm with alpha-beta pruning.

The user interface is in WPF.

History:
I have tinkered with this code on several occasions. It was the first non-trivial program that I wrote in C#. I updated it to try out generic lists when C# 2.0 came out. In 2010 I made a WPF interface, reformatted the code as per Stylecop, made more test cases and revisited the minimax algorithm, and upload it to github. 

Anthony Steele