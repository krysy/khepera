using System;

namespace Hardware
{
    public static class Motors
    {
        private static class ControlModes
        {
            public static byte idle = 0;
            public static byte speed = 1;
            public static byte speedProfile = 2;
            public static byte position = 3;
            public static byte openPosition = 4;
        }

        //comment, lol
        public static void SetSpeedConsign(int leftSpeed, int rightSpeed)
        {
            if (leftSpeed > 2940 || leftSpeed < -2940 || rightSpeed > 2940 || rightSpeed < -2940)
            {
                throw new Exception("Invalid motor speed value! Range is from -2940 to 2940");
            }

            I2C.WriteByte(I2C.Registers.Motors.ControlType, ControlModes.speedProfile);

            byte leftLowByte = (byte) (leftSpeed & 0xFF);
            byte leftHighByte = (byte) ((leftSpeed >> 8) & 0xFF);
            byte rightLowByte = (byte) (rightSpeed & 0xFF);
            byte rightHighByte = (byte) ((rightSpeed >> 8) & 0xFF);

            I2C.WriteByte(I2C.Registers.Motors.Left.SpeedConsign.Low, leftLowByte);
            I2C.WriteByte(I2C.Registers.Motors.Left.SpeedConsign.High, leftHighByte);
            I2C.WriteByte(I2C.Registers.Motors.Right.SpeedConsign.Low, rightLowByte);
            I2C.WriteByte(I2C.Registers.Motors.Right.SpeedConsign.High, rightHighByte);
        }

        public static void SetSpeed(int leftSpeed, int rightSpeed)
        {
            if (leftSpeed > 2940 || leftSpeed < -2940 || rightSpeed > 2940 || rightSpeed < -2940)
            {
                throw new Exception("Invalid motor speed value! Range is from -2940 to 2940");
            }

            I2C.WriteByte(I2C.Registers.Motors.ControlType, ControlModes.speed);

            byte leftLowByte = (byte) (leftSpeed & 0xFF);
            byte leftHighByte = (byte) ((leftSpeed >> 8) & 0xFF);
            byte rightLowByte = (byte) (rightSpeed & 0xFF);
            byte rightHighByte = (byte) ((rightSpeed >> 8) & 0xFF);

            I2C.WriteByte(I2C.Registers.Motors.Left.SpeedConsign.Low, leftLowByte);
            I2C.WriteByte(I2C.Registers.Motors.Left.SpeedConsign.High, leftHighByte);
            I2C.WriteByte(I2C.Registers.Motors.Right.SpeedConsign.Low, rightLowByte);
            I2C.WriteByte(I2C.Registers.Motors.Right.SpeedConsign.High, rightHighByte);
        }
    }
}