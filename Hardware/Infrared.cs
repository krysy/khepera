namespace Hardware
{
    public static class Infrared
    {
        public static class Ambient
        {
            public static ushort GetBackLeft()
            {
                byte lowByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.BackLeft.Low);
                byte highByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.BackLeft.High);
                return (ushort) ((highByte << 8) | lowByte);
            }

            public static ushort GetLeft()
            {
                byte lowByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.Left.Low);
                byte highByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.Left.High);
                return (ushort) ((highByte << 8) | lowByte);
            }

            public static ushort GetFrontLeft()
            {
                byte lowByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.FrontLeft.Low);
                byte highByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.FrontLeft.High);
                return (ushort) ((highByte << 8) | lowByte);
            }

            public static ushort GetFront()
            {
                byte lowByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.Front.Low);
                byte highByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.Front.High);
                return (ushort) ((highByte << 8) | lowByte);
            }

            public static ushort GetFrontRight()
            {
                byte lowByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.FrontRight.Low);
                byte highByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.FrontRight.High);
                return (ushort) ((highByte << 8) | lowByte);
            }

            public static ushort GetRight()
            {
                byte lowByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.Right.Low);
                byte highByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.Right.High);
                return (ushort) ((highByte << 8) | lowByte);
            }

            public static ushort GetBackRight()
            {
                byte lowByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.BackRight.Low);
                byte highByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.BackRight.High);
                return (ushort) ((highByte << 8) | lowByte);
            }

            public static ushort GetBack()
            {
                byte lowByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.Back.Low);
                byte highByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.Back.High);
                return (ushort) ((highByte << 8) | lowByte);
            }

            public static ushort GetGroundLeft()
            {
                byte lowByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.GroundLeft.Low);
                byte highByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.GroundLeft.High);
                return (ushort) ((highByte << 8) | lowByte);
            }

            public static ushort GetGroundFrontLeft()
            {
                byte lowByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.GroundFrontLeft.Low);
                byte highByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.GroundFrontLeft.High);
                return (ushort) ((highByte << 8) | lowByte);
            }

            public static ushort GetGroundFrontRight()
            {
                byte lowByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.GroundFrontRight.Low);
                byte highByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.GroundFrontRight.High);
                return (ushort) ((highByte << 8) | lowByte);
            }

            public static ushort GetGroundRight()
            {
                byte lowByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.GroundRight.Low);
                byte highByte = I2C.ReadByte(I2C.Registers.Infrared.Ambient.GroundRight.High);
                return (ushort) ((highByte << 8) | lowByte);
            }
        }

        public static class Proximity
        {
            public static int BackLeft;
            public static int Left;
            public static int FrontLeft;
            public static int Front;
            public static int FrontRight;
            public static int Right;
            public static int BackRight;
            public static int Back;
            public static int GroundLeft;
            public static int GroundFrontLeft;
            public static int GroundFrontRight;
            public static int GroundRight;

            public static void ReloadValues()
            {
                var values = I2C.ReadMultipleBytes(I2C.Registers.Infrared.Proximity.BackLeft.Low,
                    I2C.Registers.Infrared.SensorCount * 2);
                //anyone else hungry for spaghetti?
                BackLeft = (values[1] << 8) | values[0];
                Left = (values[3] << 8) | values[2];
                FrontLeft = (values[5] << 8) | values[4];
                Front = (values[7] << 8) | values[6];
                FrontRight = (values[9] << 8) | values[8];
                Right = (values[11] << 8) | values[10];
                BackRight = (values[13] << 8) | values[12];
                Back = (values[15] << 8) | values[14];

                GroundLeft = (values[17] << 8) | values[16];
                GroundFrontLeft = (values[19] << 8) | values[18];
                GroundRight = (values[23] << 8) | values[22];
                GroundFrontRight = (values[21] << 8) | values[20];
            }
        }
    }
}