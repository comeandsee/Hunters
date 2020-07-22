using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location 
{
    public class Coordinates
    {
        public float lat;
        public float lon;
        public Coordinates(float lat, float lon)
        {
            this.lat = lat;
            this.lon = lon;
        }
    }

    public class GameAreaCoordinates
    {
        public Coordinates northWest;
        public Coordinates northEast;
        public Coordinates southEast;
        public Coordinates southWest;

        public GameAreaCoordinates(Coordinates northWest, Coordinates northEast, Coordinates southEast, Coordinates southWest, Coordinates centerPoint)
        {
            this.northWest = northWest;
            this.northEast = northEast;
            this.southEast = southEast;
            this.southWest = southWest;
        }

        public GameAreaCoordinates(bool isGdansk)
        {
            if (isGdansk)
            {
                this.northWest = new Coordinates(54.367929f, 18.609691f);
                this.northEast = new Coordinates(54.367909f, 18.611730f);
                this.southEast = new Coordinates(54.366359f, 18.611526f);
                this.southWest = new Coordinates(54.366479f, 18.609584f);

               // this.centerPoint = new Coordinates(54.367067f, 18.610597f);
            }
            else
            {
                this.northWest = new Coordinates(60.191901f, 24.970663f);
                this.northEast = new Coordinates(60.191901f, 24.970663f);
                this.southEast = new Coordinates(60.190312f, 24.970084f);
                this.southWest = new Coordinates(60.190477f, 24.965600f);

               // this.centerPoint = new Coordinates(60.191238f, 24.967917f);
            }
           
        }
    }
}
