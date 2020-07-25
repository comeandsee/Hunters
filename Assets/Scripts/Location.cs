
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Location 
{
    public class Coordinates
    {
        public float lat;
        public float lon;
        public Coordinates()
        {

        }
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
                /*
                this.northWest = new Coordinates(54.367929f, 18.609691f);
                this.northEast = new Coordinates(54.367909f, 18.611730f);
                this.southEast = new Coordinates(54.366359f, 18.611526f);
                this.southWest = new Coordinates(54.366479f, 18.609584f);
                */

                // this.centerPoint = new Coordinates(54.367067f, 18.610597f);

                //duzy park

                this.northWest = new Coordinates(54.373336f, 18.606535f);
                this.northEast = new Coordinates(54.372036f, 18.613216f);
                this.southEast = new Coordinates(54.366405f, 18.611534f);
                this.southWest = new Coordinates(54.366405f, 18.611534f);
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


    public  Coordinates CalculateCoordinates(Coordinates location1, Coordinates location2, Coordinates location3,
        Coordinates location4)
    {
        Coordinates[] allCoords = { location1, location2, location3, location4 };
        float minLat = allCoords.Min(x => x.lat);
        float minLon = allCoords.Min(x => x.lon);
        float maxLat = allCoords.Max(x => x.lat);
        float maxLon = allCoords.Max(x => x.lon);

        System.Random r = new System.Random();
       
            Coordinates point = new Coordinates();
            do
            {
                var przyklad =  r.NextDouble() * (maxLat - minLat) + minLat;
                point.lat = Random.Range(minLat, maxLat);
                point.lon = Random.Range(minLon, maxLon);   //r.NextDouble() * (maxLon - minLon) + minLon;
            } while (!IsPointInPolygon(point, allCoords));

        return point;
     
    }

   
    private bool IsPointInPolygon(Coordinates point, Coordinates[] polygon)
    {
        int polygonLength = polygon.Length, i = 0;
        bool inside = false;
        // x, y for tested point.
        double pointX = point.lon, pointY = point.lat;
        // start / end point for the current polygon segment.
        double startX, startY, endX, endY;
        Coordinates endPoint = polygon[polygonLength - 1];
        endX = endPoint.lon;
        endY = endPoint.lat;
        while (i < polygonLength)
        {
            startX = endX;
            startY = endY;
            endPoint = polygon[i++];
            endX = endPoint.lon;
            endY = endPoint.lat;
            //
            inside ^= ((endY > pointY) ^ (startY > pointY)) /* ? pointY inside [startY;endY] segment ? */
                      && /* if so, test if it is under the segment */
                      (pointX - endX < (pointY - endY) * (startX - endX) / (startY - endY));
        }
        return inside;
    }

   
}
