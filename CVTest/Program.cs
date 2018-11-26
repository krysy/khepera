using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.Linq;
using System.Threading;
using OpenCvSharp;
using Point = OpenCvSharp.Point;

namespace CVTest
{
    class Program
    {
        private static string MainWindowName = "Nice fucking model!";
        private static int _frameWidth = 0;

        private static class Mathz
        {
            public static Point2f GetCenterPoint(Point2f firstPoint, Point2f secondPoint)
            {
                var centerX = (firstPoint.X + secondPoint.X) / 2;
                var centerY = (firstPoint.Y + secondPoint.Y) / 2;
                return new Point2f(centerX, centerY);
            }

            public static Point2f GetVectorDefinition(Point2f firstPoint, Point2f secondPoint)
            {
                var u1 = Math.Abs(firstPoint.X - secondPoint.X);
                var u2 = Math.Abs(firstPoint.Y - secondPoint.Y);
                
                return new Point2f(u1, u2);
            }
        }

        
        
        

        static void Main(string[] args)
        {
            //Mat picture = new Mat("/home/krysy/Pictures/khepera1.png");
            var khepvid = new VideoCapture("/home/krysy/Pictures/longKhepvid.mp4");
            //var khepvid = new VideoCapture("http://192.168.8.112:8080/video");
            var frame = new Mat();

            _frameWidth = khepvid.FrameWidth;
            //window.ShowImage(picture);
            //Cv2.WaitKey();
            var image = new Mat();
            var triangleFactory = new TriangleFactory(_frameWidth);
            
            while (khepvid.IsOpened())
            {
                khepvid.Read(frame);
                
                //Cv2.CvtColor(frame, frame, ColorConversionCodes.BGR2HSV);
                //Cv2.CvtColor(frame, image, ColorConversionCodes.BGR2Lab);
                
               // Cv2.InRange(frame, new Scalar(255,255,120), new Scalar(255,255,200) , image);
                
               // Cv2.InRange(frame, new Scalar(0,0,120), new Scalar(255,255,250), frame);
                
                
                Cv2.CvtColor(frame, image, ColorConversionCodes.BGR2GRAY);
                Cv2.Threshold(image, image, 220, 255, ThresholdTypes.Binary);
                Cv2.Erode(image, image, null, iterations: 2);
                Cv2.Dilate(image, image, null, iterations: 4);
                
                var circles = Cv2.HoughCircles(
                    image,
                    HoughMethods.Gradient,
                    1,
                    15,
                    param1: 2,
                    param2: 5,
                    minRadius: 5,
                    maxRadius: 40
                );

                try
                {
                    var triangle = triangleFactory.Produce(circles[0].Center, circles[1].Center, circles[2].Center);
                    Cv2.Line(frame, (Point) triangle.PointB, (Point) triangle.PointC, Scalar.Blue, 8);
                    Cv2.Line(frame, (Point) triangle.PointC, (Point) triangle.PointA, Scalar.Blue, 8);
                    Cv2.Line(frame, (Point) triangle.PointA, (Point) triangle.PointB, Scalar.Gold, 10);
                    
                    Cv2.PutText(frame, "X",(Point) triangle.PointA,HersheyFonts.Italic, 2, Scalar.Red, 4);
                    Cv2.PutText(frame, "X",(Point) triangle.PointB,HersheyFonts.Italic, 2, Scalar.Red, 4);
                    Cv2.PutText(frame, "C",(Point) triangle.PointC,HersheyFonts.Italic, 2, Scalar.Red, 4);

                    Cv2.PutText(frame, $"DEG: {triangle.Angle}", new Point(20, 100), HersheyFonts.Italic, 2, Scalar.Red, thickness: 3);
 
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    continue;
                }
                    
                    /*
                    circles.ToList().ForEach(c =>
                    {
                        //Console.WriteLine($"X: {c.Center.X} Y: {c.Center.Y}");
                        Cv2.Circle(frame, (int) c.Center.X, (int) c.Center.Y, (int) c.Radius, new Scalar(255,0,0), 8);
                    });
                    /*
                    var triangle = new Triangle(circles[0].Center,circles[1].Center,circles[2].Center);
                    
                    Cv2.Line(frame, (Point) triangle.Dumb[0], (Point) triangle.Dumb[1], Scalar.Beige, 10);
                    /*Cv2.Line(frame, (Point) triangle.PointA, (Point) triangle.PointB, Scalar.Gold, 10);
                    Cv2.Line(frame, (Point) triangle.PointB, (Point) triangle.PointC, Scalar.Blue, 8);
                    Cv2.Line(frame, (Point) triangle.PointC, (Point) triangle.PointA, Scalar.Blue, 8);
                    //Cv2.Line(frame, anglePtB, anglePtC, (0, 255, 0), 8)
                    //Cv2.Line(frame, anglePtC, anglePtA, (0, 255, 0), 8)
                    // LINQ, because why the hell not
                    
    /*            
                    foreach (var circle in circles)
                    {
                        Console.WriteLine($"X: {circle.Center.X} Y: {circle.Center.Y}");
                        Cv2.Circle(frame, (int) circle.Center.X, (int) circle.Center.Y, (int) circle.Radius, new Scalar(255,0,0), 8);
                    }*/
                //break;
                /*
                foreach (var circle in circles)
                {
                    Console.WriteLine($"X: {circle.Center.X} Y: {circle.Center.Y}");
                    Cv2.Circle(frame, (int) circle.Center.X, (int) circle.Center.Y, (int) circle.Radius, new Scalar(255,0,0), 8);
                }*/
                
                Cv2.NamedWindow(MainWindowName);
                Cv2.ResizeWindow(MainWindowName, 768, 432);
                Cv2.ImShow(MainWindowName, frame);
                Cv2.WaitKey(1);
            }

        }
    }
}