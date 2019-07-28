using System;
using System.Collections.Generic;

namespace RockPaperScissorsApp {
    class Program {

        static IDictionary<char, char> winsAgainstDict = new Dictionary<char, char>();
        static IDictionary<char, int> turnsTaken = new Dictionary<char, int>();

        static void Main(string[] args) {
          /*
          Takes an optional command line argument, 'human' or 'computer' which
          determines whether the game will run with two players, or one player
          against the computer. The default (no argument) is for two players.
          */
          bool againstComputer = false;
          try {
            if ( args[0].Equals("computer") ) {
              againstComputer = true;
            } else if ( args[0].Equals("human") ) {
              againstComputer = false;
            } else {
              Console.WriteLine("Illegal command line argument - use either"
                                + " 'human' or 'computer' (default = human).");
              System.Environment.Exit(1);
            }
          } catch (IndexOutOfRangeException e) {}

          winsAgainstDict['r'] = 's';
          winsAgainstDict['p'] = 'r';
          winsAgainstDict['s'] = 'p';

          turnsTaken['r'] = 0;
          turnsTaken['p'] = 0;
          turnsTaken['s'] = 0;

          int winner = 0;
          int numRounds = 0;

          do {
            winner = NextRound(againstComputer);
            numRounds++;
            switch (winner) {
              case 0:
                Console.WriteLine("Both players made the same move.");
                break;
              case -1:
                Console.WriteLine("An error has occured: invalid player moved received.");
                System.Environment.Exit(1);
                break;
            }
          } while (winner == 0); //Look NextTurn method while both players are giving the same move.

          //When there is a winner, output game stats.
          Console.WriteLine("Player {0} won after {1} round(s).", formatPlayerName(winner), numRounds);
          Console.WriteLine("Each move was used this many times:");
          Console.WriteLine("Rock: "+turnsTaken['r']);
          Console.WriteLine("Paper: "+turnsTaken['p']);
          Console.WriteLine("Scissors: "+turnsTaken['s']);
        }

        static int NextRound(bool againstComputer) {
          /*
          Gets both players' next move, and determines which player won, or
          whether there was a tie.
          */
          char playerOneMove = getMoveFromPlayer(1);

          char playerTwoMove;
          if (againstComputer) {
            playerTwoMove = getRandomMove();
          } else {
            playerTwoMove = getMoveFromPlayer(2);
          }

          if (playerOneMove == playerTwoMove) {
            return 0;
          }

          if (winsAgainstDict[playerOneMove] == playerTwoMove) {
            return 1;
          }

          if (winsAgainstDict[playerTwoMove] == playerOneMove) {
            if (againstComputer) {
              return 3;
            } else {
              return 2;
            }
          }

          return -1;
        }

        static char getMoveFromPlayer(int playerNum) {
          /*
          Reads key input from given player to determine move, and checks for invalid inputs.
          Runs recursively until R, P or S is entered.
          */
          Console.Write("Player {0} enter move [R/P/S]: ", formatPlayerName(playerNum));
          char playerMove = Char.ToLower(Console.ReadKey().KeyChar);
          Console.WriteLine();
          try {
            turnsTaken[playerMove]++;
          } catch (KeyNotFoundException e) {
            Console.WriteLine("Please enter a valid move.");
            Console.WriteLine();
            return getMoveFromPlayer(playerNum);
          }
          return playerMove;
        }

        static char getRandomMove() {
          /*
          Returns a random move for when playing against the computer.
          */
          char move = 'x';

          Random random = new Random();
          switch (random.Next(3)) {
            case 0:
              move = 'r'; break;
            case 1:
              move = 'p'; break;
            case 2 :
              move = 's'; break;
          }
          turnsTaken[move]++;
          return move;
        }

        static string formatPlayerName(int playerNum) {
          /*
          Stringifies player number so that 'player one' is displayed instead of 'player 1',
          and that 'player two' or 'computer' is displayed instead of 'player 2'.
          */
          string name = "";
          switch (playerNum) {
            case 1:
              name = "one"; break;
            case 2:
              name = "two"; break;
            case 3:
              name = "computer"; break;
            default:
              Console.WriteLine(playerNum);
              Console.WriteLine("An error has occured: Invalid player index.");
              System.Environment.Exit(2);
              break;
        }
        return name;
      }
    }
}
