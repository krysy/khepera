using System;
using System.Collections.Generic;
using System.Linq;
using OpenCvSharp;

namespace CVTest
{
    public class TriangleFactory
    {
        private int FrameWidth { get; set; }
        private int FrameHeight { get; set; }
       
        public Triangle Produce(Point2f pointA, Point2f pointB, Point2f pointC)
        {
            var distances = new List<float>();
            distances.Add(CalculateDistance(pointA,pointB));
            distances.Add(CalculateDistance(pointB,pointC));
            distances.Add(CalculateDistance(pointA,pointC));
            var temp = new List<Point2f>{pointA, pointB, pointC};
                
            switch (distances.IndexOf(distances.Min()))
            {
                case 0:
                    //triangle is sorted properly
                    break;
                case 1:
                    temp = new List<Point2f>{pointB, pointC, pointA};
                    break;
                case 2:
                    temp = new List<Point2f>{pointC, pointA, pointB};
                    break;
            }

            return new Triangle(temp[0], temp[1], temp[2], FrameWidth);
        }
        
        private static float CalculateDistance(Point2f point1, Point2f point2)
        {
            return (float) Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }

        public TriangleFactory(int frameWidth=1920, int frameHeight=1080)
        {
            FrameHeight = frameHeight;
            FrameWidth = frameWidth;
        }

        public class Triangle
        {
            public Point2f PointA { get; private set; }
            public Point2f PointB { get; private set; }
            public Point2f PointC { get; private set; }
            public double Angle { get; private set; }

            public Triangle(Point2f pointA, Point2f pointB, Point2f pointC, int frameWidth, int frameHeight=0)
            {
                PointA = pointA;
                PointB = pointB;
                PointC = pointC;
                
                var A = PointA;
                var B = PointB;
                var C = PointC;

                var Sx = (A.X + B.X) / 2;
                var Sy = (A.Y + B.Y) / 2;
                var S = new Point2d(Sx, Sy);

                var vecU1 = S.X - C.X;
                var vecU2 = S.Y - C.Y;

                // a1 = 0       a2 = 1080
                // b1 = 1920    b2 = 1080
                var vecV1 = frameWidth;
                var vecV2 = frameHeight;

                var cosAngle = ((vecU1 * vecV1) + (vecU2 * vecV2)) /
                               ( Math.Sqrt( Math.Pow(vecU1, 2) + Math.Pow(vecU2, 2) ) *
                                 Math.Sqrt( Math.Pow(vecV1, 2) + Math.Pow(vecV2, 2) ) );
                var angleDeg = Math.Acos(cosAngle)* (180 / Math.PI);
                if (C.Y < S.Y)
                    Angle = 180 + (180 - angleDeg);
                else
                    Angle = angleDeg;
            }
        }
    }
}