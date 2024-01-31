using System;
using System.Security.Cryptography;
namespace Battleships
{
    class Program
    {

        static void OutputBoard(string[,] board){
            Console.Write("   ");
            for (int x =0; x<board.GetLength(1); x++){
                Console.Write($"|{Convert.ToString(x).PadLeft(2,' ').PadRight(3,' ')}");
            }
            Console.WriteLine("|");
            for (int y =0; y<board.GetLength(1); y++){
                Console.Write("    ");
                for (int x =0; x<board.GetLength(0); x++){
                    Console.Write("---+");
                }
                Console.WriteLine("");
                
                Console.Write($"{Convert.ToString(y).PadLeft(3,' ')}");

                for (int x =0; x<board.GetLength(0); x++){
                    string outchar=board[x,y];
                    if (outchar=="-" || outchar=="?" ){
                        outchar=" ";
                    }
                    Console.Write($"| {outchar} ");
                }
                Console.WriteLine("|");
            }
            Console.Write("    ");
            for (int x =0; x<board.GetLength(1); x++){
                Console.Write("---+");
            }
            Console.WriteLine("");
            return;
        }

        static void TestBoard1(){
            //board has 5 ships and is 10 x 10
            int[] shipsize={2,3,3,4,5};
            PlayingBoard p1Board=new PlayingBoard(5,10,10,shipsize);
            p1Board.PlaceShip(0,(1,1),'h');
            p1Board.PlaceShip(1,(5,0),'h');
            p1Board.PlaceShip(4,(8,2),'v');
            p1Board.PlaceShip(3,(4,4),'h');
            p1Board.PlaceShip(2,(1,5),'v');
            p1Board.Shoot((0,0));
            p1Board.Shoot((7,6));
            p1Board.Shoot((8,2));
            p1Board.Shoot((1,1));
            p1Board.Shoot((2,1));
            
            //print board with no "fog of war"
            string[,] boardData=p1Board.GetBoardInformation(false);
            OutputBoard(boardData);
            Console.WriteLine("");
            //print board with  "fog of war" on
            boardData=p1Board.GetBoardInformation(true);
            OutputBoard(boardData);
            
            return;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("hello");

            
            Console.WriteLine("goodbye");

        }
    }
}
