# Sudoku Solver



#### Preface

Hello, and welcome to my sudoku solver app!.

This app, pretty self explanatory, is a sudoku solver. How it generally works is it inputs a string from the user, translates it to a sudoku board and solves it! I hope you will enjoy this app, and have a beautiful day.



#### User's Guide

Before you use this solver program, you need to first run the code in your project. You will need to have the program's files installed on your device. And only then, inside your main program, run the command '**SudokuPlayer.Play()**' to initiate the program's command line interface.

Important note: the Play() command mentioned above does receive a parameter. The parameter tells the program which size board to expect from the user. If left blank, the size expected will be 9. If the parameter is 0 (**SudokuPlayer.Play(0)**), it activates dynamic mode that means it will create a board according to the user's input size. any other value needs to be a square of some integer, otherwise an error will appear.



After getting the program running, a nice greeting message will appear, and right below it a line asking for a sudoku board string. The message will look like '**Please enter a sudoku board ('quit' to exit):>\_**' and the sudoku board should be added to the right of the message. after entering the whole string press the ENTER key to submit the board. The program will draw and print the board on screen. Then, it will solve it and print the solved board with an elapsed time, as a fun informative gem.



To exit the program, simply write 'quit' after the input message. the program will close to run another day.



#### Developer's Guide

The purpose of this guide it to demonstrate the structure of the program and logic behind the algorithms. This section will be divided into three pieces: Motivation, Structural build, and Algorithmic overview.



##### Motivation

The approach to this solver program is to solve it as humanly as possible. Meaning, the program is written so it portrays the human approach to solving sudoku puzzles, with code. The reason for this being for simpler, more intuitive programming. For the cost of possible efficiency.



##### Structural Build

I chose to structure the project, as object oriented as possible. This helps the program visualize the sudoku board as an object instead of a dump of data.



The objects implemented in this program are:

###### **Square**

Has fields:

1. Notes: A set of all possible solutions for the square.
2. Coordinates: A tuple of the squares location on the square matrix (the board).
3. Value: The solution of said square or 0 if it hasn't been solved yet.

Object methods include checking off options, solving the square, resetting the note set etc.



###### **Board**

Has fields:

1. Size: An integer representing the length of each side in the board, also the maximal value a square can have.
2. Root\_Size: An integer representing the square root of the side, which is the size of a block within the board.
3. Board\_Mat: A matrix of squares, it represents the content of the board itself. The matrix size is Size X Size.

Object methods include printing the board, loading a board, checking validity and calling to a solver method.



###### **Move**

This object represents one move, it can either be a checking off of a note in a square or the solving of a square.

It holds an instance of a square, representing the old square.



###### **Board Solver**

This object contains the Board object, along with some more fields. This object contains the solving algorithm. fields are:

1. board: an instance of the board.
2. squares: the square matrix, stored in a more convenient variable.
3. size: the size of the board, stored in a more convenient variable.
4. move stack: stores the past moves in LIFO order. The last move committed will be the first one to be undone in case of an undo.

Object methods include checking if the board is solved, solving a square, guessing a square etc.



###### **Exceptions**

The program uses 2 main exceptions to deal with any sort of input.

1. InvalidBoardSizesException: occurs when the size of the board when initiated or loaded are illegal. Either not squares or not equal.
2. InvalidInputException: occurs when a method is called with an impossible or unsolvable result. this exception has two inheritors.

* UnsolvableBoardException: occurs when a board cannot be solved by the algorithm.
* InvalidCharacterException: occurs when an illegal character is in the input string.



###### **Additional Data Structures**

* A stack is used to store past moves. It is a regular LIFO stack (Last In First Out). meaning the most recently pushed move into the stack, will be the first to be popped out of the stack. This is perfect for keeping track of moves in algorithms like these.
* Hash Sets are used to store integer variables and Square objects. Objects in a set are stored with no certain order, and without multiple equal objects. For this reason, sets are good to store notes of a square, or collect neighboring squares to iterate through.





##### **Algorithmic Overview**

The program solves the sudoku puzzle in different phases, each accomplishing a different stride towards the solution of the puzzle.

1. ###### Input Processing

Inside the user interface class, right before creating and loading the sudoku board. The input string needs to be translated into an integer matrix. In order to do that we have the following methods:

* GetUserInput() - reads the string from the CLI, if the input is 'quit', it changes the status of the player to end the program. If the input wasn't the exiting command, the function calls StrToMat.
* StrToMat() - ensures the dimensions of the input are legal, sizes are equal and squared. If so, it allocates an integer matrix and puts each value inside it, to represent the real board matrix. In order to deal with more than 9 digits potentially, the method calls another function, CharToInt().
* CharToInt() - gets a character from the input string, and calculates the value that the square will be assigned. the definitions are: characters 0-9 will get their numeric values, and letters a-z (uppercase or lowercase) will get a larger value, when values are consecutive and a starts with 10, b is 11 and so forth.



###### 2\. Board Building

After an input string has been successfully processed into an integer matrix. The program constructs the board with its size (fixed size or matching input size, depends on the Play size parameter), and then loads the matrix into the board object.

* LoadBoard() - Receives the integer matrix and builds the board matrix accordingly. if the square already has a value, it builds it as a fixed valued square with a null notes set, if the value is 0, it initiates the square with the value 0 and a notes set consisting of all the values from 1 to the board size.



###### 3\. Solving

The solving algorithm is based on a recursive backtracking approach and is broken down to three more parts. Note taking, Solving and guessing.

* Note taking - For each square, all its neighbors are visited, and if a neighbor is already solved to a certain value. that value will be checked off of the squares notes set.
* Solving - After we made out notes, There might be some squares that only have one possible solution, or has a unique possibility in one of its groups (row, column, block). such square is solvable with certainty, so we solve the square, document the move, and push the move down the moves stack.
* Guessing - After all the solvable squares have been solved, there might be some squares left with indefinite possibilities for their solutions. In such cases, the program will guess the first possibility, and try to solve for the rest of the board (recursively) if the board is not solvable, then the guess was incorrect. in this case, we will undo our solve, and guess for the next possible solution. And so on until we reach a valid solution, and present it to the user, or we run out of possible solutions and admit defeat to the user. Calling that board unsolvable.





#### Personal Reflection

I will start this part from the conclusion: I am not happy with how the app turned out. The solver, although reaches a correct solution for all the board I've tested it on, is too slow for my liking and that is because the algorithm is too heavy. There are countless ways to optimize the algorithm so it will run more lightly, solving the hardest puzzles in a fraction of a second, but it is not the case with my project. The reason for this screw up is bad planning from my behalf. I haven't managed my time right for the task, and it shows. I would really like to apologize for the poor performance and to say I have learned my lesson. I find my peace with the fact that the algorithm could at least solve the hard puzzles i gave it, but it fails to meet the project's efficiency requirement. What I learned from this experience is that when faced with two tasks, I will from now on prioritize the more important task to avoid similar incidents going forward. Thank you so much for joining me on this brief journey, have a nice rest of your day.

