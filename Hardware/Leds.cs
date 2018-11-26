namespace Hardware
{
    public static class Leds
    {
        public static void SetFrontLeft(byte red, byte green, byte blue)
        {
            if (red > 0x3F)
            {
                red = 0x3F;
            }

            if (green > 0x3F)
            {
                green = 0x3F;
            }

            if (blue > 0x3F)
            {
                blue = 0x3F;
            }

            I2C.WriteByte(I2C.Registers.Leds.FrontLeft.Red, red);
            I2C.WriteByte(I2C.Registers.Leds.FrontLeft.Green, green);
            I2C.WriteByte(I2C.Registers.Leds.FrontLeft.Blue, blue);
        }

        public static void SetFrontRight(byte red, byte green, byte blue)
        {
            if (red > 0x3F)
            {
                red = 0x3F;
            }

            if (green > 0x3F)
            {
                green = 0x3F;
            }

            if (blue > 0x3F)
            {
                blue = 0x3F;
            }

            I2C.WriteByte(I2C.Registers.Leds.FrontRight.Red, red);
            I2C.WriteByte(I2C.Registers.Leds.FrontRight.Green, green);
            I2C.WriteByte(I2C.Registers.Leds.FrontRight.Blue, blue);
        }

        public static void SetBack(byte red, byte green, byte blue)
        {
            if (red > 0x3F)
            {
                red = 0x3F;
            }

            if (green > 0x3F)
            {
                green = 0x3F;
            }

            if (blue > 0x3F)
            {
                blue = 0x3F;
            }

            I2C.WriteByte(I2C.Registers.Leds.Back.Red, red);
            I2C.WriteByte(I2C.Registers.Leds.Back.Green, green);
            I2C.WriteByte(I2C.Registers.Leds.Back.Blue, blue);
        }
    }
}