using System;
using System.Security.Cryptography;
namespace Battleships
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello");
            int[] shipsize={3,4,2};
            PlayingBoard p1Board=new PlayingBoard(3,10,10,shipsize);
            (int,int)[] ship0={(1,1),(1,2),(1,3)};
            p1Board.PlaceShip(0,ship0);
            string[,] boardData=p1Board.GetBoardInformation(false);
            Console.WriteLine("goodbye");
        }
    }
}
