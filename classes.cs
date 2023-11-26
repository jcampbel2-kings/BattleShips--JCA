using System;
using Microsoft.Win32;
namespace Battleships
{
    class Ship{
        int size;
        bool[] health;
        (int x, int y)[] location;
        bool isplaced;

        public int Size {get { return size;} }  
        public bool IsAlive{get {return ShipAlive();}}
        public bool IsPlaced{get {return isplaced;}}
        public (int x, int y)[] Location {get {return location;}}
        public bool[] Health {get {return health;}} 

        public Ship(int s){
            size=s;
            health=new bool[size];
            isplaced=false;
            for (int i=0; i<size;i++){
                health[i]=true;
            }
            location=new (int x, int y)[size];
            return;
        }

        public bool Place((int x, int y)[] newLocation){
            if (newLocation.Length != size) {
                return false;
            } else {
                for (int i=0; i<size; i++){
                    location[i].x =newLocation[i].x;
                    location[i].y =newLocation[i].y;
                }
                isplaced=true;
                return true;
            }
        }

        public bool Hit((int x, int y) square){
            bool rval=false;
            for (int i=0; i<size;i++){
                if (square.x==location[i].x && square.y==location[i].y){
                    health[i]=false;
                    rval=true;
                }
            }  
            return rval;
        }

        private bool ShipAlive(){
            bool alive=false;
            foreach (bool loc in health){
                if (loc){
                    alive=true;
                }
            }
            return alive;
        }

    }

    class PlayingBoard{
        int numShips;
        Ship[] ships;
        int[,] board;
        int maxX;
        int maxY;
        bool validboard;
        string msg;
        bool error;
        const int Empty=0, Miss=1, ShipNotHit=2, ShipHit=3;

        public int NumShips {get{return numShips;}}
        public bool ValidBoard {get{return validboard;}}
        public int MaxX {get{return maxX;}}
        public int MaxY {get{return maxY;}}
        public string Msg {get {return msg;}}
        public bool Error {get {return error;}} 
        public const string Unknown="?", ShipDamaged="D", ShipNotFound="*", ShipSunk="S", ShotMiss="X",NotFiredOn="-" ;

        public int GetShipSize(int shipID){
            return ships[shipID].Size;
        }

        public bool GetShipAlive(int shipID){
            return ships[shipID].IsAlive;
        }

        public bool PlayerLost(){
            bool rval=true;
            foreach(Ship s in ships){
                if (s.IsAlive){
                    rval=false;
                }
            }
            return rval;
        }

        public PlayingBoard(int ns, int mx, int my, int[] shipsizes){
            validboard=true;
            error=false;
            msg="";
            if (ns > 0 && shipsizes.Length==ns){
                numShips=ns;
                ships=new Ship[numShips];
                for (int i=0; i<ns; i++){
                    ships[i]=new Ship(shipsizes[i]);
                }
            } else {
                numShips=0;
                ships=new Ship[1];
                validboard=false;
                error=true;
                msg="Invalid ship data";
            }
            if (mx>0 && my>0){
                maxX=mx;
                maxY=my;
                board=new int[maxX, maxY];
                for (int x=0; x<maxX; x++){
                    for (int y=0; y<maxY; y++){
                        board[x,y]=Empty;
                    }
                }
            } else {
                maxX=0;maxY=0;
                validboard=false;
                error=true;
                msg="Invalid board size";
                board=new int[1,1];
                board[0,0]=Empty;
            }

        }

        private int FindShip((int x, int y) coordinate){
            int rval=-1;
            error=true;
            msg="Error in Ships not found";
            for (int i=0;i<numShips;i++){
                Ship shipstemp=ships[i];
                for (int j=0;j<ships[i].Size;j++){
                    if (ships[i].Location[j].x==coordinate.x &&ships[i].Location[j].y==coordinate.y){
                        rval=i;
                        error=false;
                        msg="Ship found";
                        break;
                    }
                }
            }
            return rval;
        }

        public void Shoot((int x, int y) coordinate){
            error =false;
            if (ValidCoordinate(coordinate)){
                if (board[coordinate.x, coordinate.y]==Empty){
                    board[coordinate.x, coordinate.y]=Miss;
                    error=false;
                    msg="Shot misses";
                } else if (board[coordinate.x, coordinate.y]==ShipNotHit){
                    board[coordinate.x, coordinate.y]=ShipHit;
                    //need to find ship and add damage to it
                    int shipnum = FindShip(coordinate);
                    ships[shipnum].Hit(coordinate);
                    error =false;
                    msg="Ship Hit";
                } else {
                    //already shot there so invalid move
                    error=true;
                    msg="Already shot at this location";
                } 
            }
            return;
        }

        private bool ValidCoordinate((int x, int y) coordinate) {
            bool rval=true;
            error=false;
            if (coordinate.x > maxX || coordinate.y > maxY){
                    rval=false;
                    error=true;
                    msg="coordinates beyond size of board";
                }
                if (coordinate.x < 0 || coordinate.y < 0){
                    rval=false;
                    error=true;
                    msg="some coordinates are negative";
                }
            return rval;
        }

        private bool ValidCoordinates((int x, int y)[] coordinates){
            bool rval=true;
            foreach ((int x, int y) coordinate in coordinates){
                if (!ValidCoordinate(coordinate)){
                    rval=false;
                    break;
                }  
            }
            return rval;
        }

        public string[,] GetBoardInformation(bool fogOfWar){
            string[,] boardData=new string[maxX,maxY];
            error =false;
            for (int x=0; x<maxX; x++){
                for (int y=0; y<maxY; y++){
                    if (board[x,y]==Empty){
                        boardData[x,y]=NotFiredOn;
                    } else if  (board[x,y]==Miss) {
                        boardData[x,y]=ShotMiss;    
                    } else {
                        boardData[x,y]=Unknown;
                    } 
                }
            }
            for (int i=0; i<numShips; i++){
                foreach ((int x,int y) coordinate in ships[i].Location){
                    if (board[coordinate.x,coordinate.y]==ShipNotHit){
                        if (fogOfWar) {
                            boardData[coordinate.x,coordinate.y]=NotFiredOn;
                        } else {
                            boardData[coordinate.x,coordinate.y]=ShipNotFound; 
                        }
                    } else if (board[coordinate.x,coordinate.y]==ShipHit){
                        if (ships[i].IsAlive){
                            boardData[coordinate.x,coordinate.y]=ShipDamaged;
                        } else {
                            boardData[coordinate.x,coordinate.y]=ShipSunk;
                        }
                    } else {
                        error=true;
                        msg="Error in ship data";
                    }
                }
            }
            return boardData;
        }

        public bool PlaceShip(int shipNumber, (int x, int y)[] coordinates ){
            bool areaClear=true;
            bool rval;
            if (!ValidCoordinates(coordinates)){
                rval=false;
            } else {
                for (int i=0; i<coordinates.Length; i++){
                    if (board[coordinates[i].x,coordinates[i].x ]!=Empty ){
                        areaClear=false;
                    }
                }
                if (!areaClear) {
                    error=true;
                    msg="Ship cant be placed here as partly occupied";
                    rval=false;
                } else {
                    if (ships[shipNumber].Place(coordinates)) {
                        foreach ((int x, int y)coordinate in coordinates){
                            board[coordinate.x, coordinate.y]=ShipNotHit;
                        }
                        error=false;
                        msg="Ship placed";
                        rval=true;    
                    }  else {
                        error=true;
                        msg="Unknown error in ship placement";
                        rval=false;
                    }
                }
            }
            return rval;
        }

    }


}