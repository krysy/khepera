using System;

namespace Remote
{
    public class Registers
    {
        public class MotorSpeed
        {
            public int Left { get; private set; }
            public int Right { get; private set; }

            public MotorSpeed(int left, int right)
            {
                Left = left;
                Right = right;
            }
        }

        public class Led
        {
            public byte Red { get; private set; }
            public byte Green { get; private set; }
            public byte Blue { get; private set; }

            public Led(byte red, byte green, byte blue)
            {
                Red = red;
                Green = green;
                Blue = blue;
            } 
        }

        public class Leds
        {
            public Led Back { get; private set; }
            public Led FrontLeft { get; private set; }
            public Led FrontRight { get; private set; }

            public Leds(Led back, Led frontLeft, Led frontRight)
            {
                Back = back;
                FrontLeft = frontLeft;
                FrontRight = frontRight;
            }
        }

        public class Infrared
        {
            public int BackLeft { get; set; }
            public int Left { get; set; }
            public int FrontLeft { get; set; }
            public int Front { get; set; }
            public int FrontRight { get; set; }
            public int Right { get; set; }
            public int BackRight { get; set; }
            public int Back { get; set; }
            public int GroundLeft { get; set; }
            public int GroundFrontLeft { get; set; }
            public int GroundFrontRight { get; set; }
            public int GroundRight { get; set; }
        }
    }
}