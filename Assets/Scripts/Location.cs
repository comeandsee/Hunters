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

        public GameAreaCoordinates(Coordinates northWest, Coordinates northEast, Coordinates southEast, Coordinates southWest)
        {
            this.northWest = northWest;
            this.northEast = northEast;
            this.southEast = southEast;
            this.southWest = southWest;
        }

        public GameAreaCoordinates()
        {
            this.northWest = new Coordinates(60.193635f, 24.967477f);
            this.northEast = new Coordinates(60.193736f, 24.971189f);
            this.southEast = new Coordinates(60.192200f, 24.9687445f);
            this.southWest = new Coordinates(60.191901f, 24.970663f);
        }
        public GameAreaCoordinates(bool isGdansk)
        {
            this.northWest = new Coordinates(54.367873f, 18.609933f);
            this.northEast = new Coordinates(54.367857f, 18.612052f);
            this.southEast = new Coordinates(54.366369f, 18.611564f);
            this.southWest = new Coordinates(54.366553f, 18.609472f);
        }
    }
}
