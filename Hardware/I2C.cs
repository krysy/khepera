using System;
using System.Runtime.InteropServices;

namespace Hardware
{
    public class I2C
    {
        private static class SystemImports
        {
            [DllImport("libc.so.6", EntryPoint = "open")]
            internal static extern int Open(string fileName, int mode);

            [DllImport("libc.so.6", EntryPoint = "ioctl", SetLastError = true)]
            internal extern static int Ioctl(int fd, int request, int data);

            [DllImport("libc.so.6", EntryPoint = "read", SetLastError = true)]
            internal static extern int Read(int handle, byte[] data, int length);

            [DllImport("libc.so.6", EntryPoint = "write", SetLastError = true)]
            internal static extern int Write(int handle, byte[] data, int length);
        };

        private static int I2C_BUS_HANDLE;
        private static int I2C_OPEN_READ_WRITE = 2;
        private static int I2C_SLAVE = 0x0703; //not sure why this is needed, stole it from kernel drivers
        private static int I2C_SLAVE_ADDRESS = 0x30;

        public static void OpenBus(string busname)
        {
            I2C_BUS_HANDLE = SystemImports.Open(busname, I2C_OPEN_READ_WRITE);
            if (I2C_BUS_HANDLE < 0)
            {
                throw new Exception("Cannot open I2C bus!");
            }

            //set I2C address to PIC
            SystemImports.Ioctl(I2C_BUS_HANDLE, I2C_SLAVE, I2C_SLAVE_ADDRESS);
        }

        public static void WriteByte(byte register, byte value)
        {
            byte[] regAddr = new byte[] {register, value};
            int ret = SystemImports.Write(I2C_BUS_HANDLE, regAddr, regAddr.Length);
            if (ret < 0)
            {
                throw new Exception("Error writing to the I2C bus!");
            }
        }

        public static byte ReadByte(byte register)
        {
            byte[] regAddr = new byte[] {register};
            int ret = SystemImports.Write(I2C_BUS_HANDLE, regAddr, regAddr.Length);
            if (ret < 0)
            {
                throw new Exception("Error writing to the I2C bus!");
            }

            byte[] returnDat = new byte[1];
            int count = SystemImports.Read(I2C_BUS_HANDLE, returnDat, returnDat.Length);

            return returnDat[0];
        }

        //DO NOT USE, DOESN'T WORK, YET!
        public static void writeMultipleBytes(byte register, byte[] values)
        {
            byte[] regAddr = new byte[values.Length + 1];
            regAddr[0] = register;

            for (int i = 1; i < values.Length; i++)
            {
                regAddr[i] = values[i - 1];
            }

            int ret = SystemImports.Write(I2C_BUS_HANDLE, regAddr, regAddr.Length);
            if (ret < 0)
            {
                throw new Exception("Error writing to the I2C bus!");
            }
        }

        public static byte[] ReadMultipleBytes(byte register, int length)
        {
            byte[] regAddr = new byte[] {register};
            int ret = SystemImports.Write(I2C_BUS_HANDLE, regAddr, regAddr.Length);
            if (ret < 0)
            {
                throw new Exception("Error writing to the I2C bus!");
            }

            byte[] returnDat = new byte[length];
            int count = SystemImports.Read(I2C_BUS_HANDLE, returnDat, returnDat.Length);

            return returnDat;
        }

        public static class Registers
        {
            public static byte REVISION = 0x00;

            public static class Leds
            {
                public static byte control = 0x01; //control LED

                public class FrontLeft
                {
                    public static byte Red = 0x02;
                    public static byte Green = 0x03;
                    public static byte Blue = 0x04;
                }

                public class FrontRight
                {
                    public static byte Red = 0x05;
                    public static byte Green = 0x06;
                    public static byte Blue = 0x07;
                }

                public class Back
                {
                    public static byte Red = 0x08;
                    public static byte Green = 0x09;
                    public static byte Blue = 0x0A;
                }
            }

            public static class Motors
            {
                public static byte MOT_KP = 0x0B;
                public static byte MOT_KI = 0x0C;
                public static byte MOT_KD = 0x0D;

                public static byte MOT_ACC_INC = 0x0E;
                public static byte MOT_ACC_DIV = 0x0F;

                public static byte MOT_ACC_ACC = 0x10;
                public static byte MOT_ACC_DEC = 0x11;

                public static byte PositionReset = 0x16;

                public static byte ControlType = 0x17;

                public static byte OnTarget = 0x2C;

                class PositionMargin
                {
                    public static byte Low = 0x12;
                    public static byte High = 0x13;
                }

                class SpeedPosition
                {
                    public static byte Low = 0x14;
                    public static byte High = 0x15;
                }

                public class Left
                {
                    public class SpeedConsign
                    {
                        public const byte Low = 0x18;
                        public const byte High = 0x19;
                    }

                    public class PWMConsign
                    {
                        public const byte Low = 0x1C;
                        public const byte High = 0x1D;
                    }

                    public class PositionConsign
                    {
                        public const byte LowLow = 0x20;
                        public const byte LowHigh = 0x21;
                        public const byte HighLow = 0x22;
                        public const byte HighHigh = 0x23;
                    }

                    public class Speed
                    {
                        public const byte Low = 0x28;
                        public const byte High = 0x29;
                    }

                    public class Position
                    {
                        public const byte LowLow = 0x2D;
                        public const byte LowHigh = 0x2E;
                        public const byte HighLow = 0x2F;
                        public const byte HighHigh = 0x30;
                    }
                }

                public class Right
                {
                    public class SpeedConsign
                    {
                        public const byte Low = 0x1A;
                        public const byte High = 0x1B;
                    }

                    public class PWMConsign
                    {
                        public const byte Low = 0x1E;
                        public const byte High = 0x1F;
                    }

                    public class PositionConsign
                    {
                        public const byte LowLow = 0x24;
                        public const byte LowHigh = 0x25;
                        public const byte HighLow = 0x26;
                        public const byte HighHigh = 0x27;
                    }

                    public class Speed
                    {
                        public const byte Low = 0x2A;
                        public const byte High = 0x2B;
                    }

                    public class Position
                    {
                        public const byte LowLow = 0x31;
                        public const byte LowHigh = 0x32;
                        public const byte HighLow = 0x33;
                        public const byte HighHigh = 0x34;
                    }
                }


                /*public static byte MOT_LEFT_SPEED_CONSIGN_L = 0x18;
                public static byte MOT_LEFT_SPEED_CONSIGN_H = 0x19;
    
                public static byte MOT_RIGHT_SPEED_CONSIGN_L = 0x1A;
                public static byte MOT_RIGHT_SPEED_CONSIGN_H = 0x1B;
                    
                public static byte MOT_LEFT_PWM_CONSIGN_L = 0x1C;
                public static byte MOT_LEFT_PWM_CONSIGN_H = 0x1D;
    
                public static byte MOT_RIGHT_PWM_CONSIGN_L = 0x1E;
                public static byte MOT_RIGHT_PWM_CONSIGN_H = 0x1F;
                    
                public static byte MOT_LEFT_POS_CONSIGN_LL = 0x20;
                public static byte MOT_LEFT_POS_CONSIGN_LH = 0x21;
                public static byte MOT_LEFT_POS_CONSIGN_HL = 0x22;
                public static byte MOT_LEFT_POS_CONSIGN_HH = 0x23;
    
                public static byte MOT_RIGHT_POS_CONSIGN_LL = 0x24;
                public static byte MOT_RIGHT_POS_CONSIGN_LH = 0x25;
                public static byte MOT_RIGHT_POS_CONSIGN_HL = 0x26;
                public static byte MOT_RIGHT_POS_CONSIGN_HH = 0x27;
    
                public static byte MOT_LEFT_SPEED_L = 0x28;
                public static byte MOT_LEFT_SPEED_H = 0x29;
    
                public static byte MOT_RIGHT_SPEED_L = 0x2A;
                public static byte MOT_RIGHT_SPEED_H = 0x2B;
                
                public static byte MOT_LEFT_POS_LL = 0x2D;
                public static byte MOT_LEFT_POS_LH = 0x2E;
                public static byte MOT_LEFT_POS_HL = 0x2F;
                public static byte MOT_LEFT_POS_HH = 0x30;
                
                public static byte MOT_RIGHT_POS_LL = 0x31;
                public static byte MOT_RIGHT_POS_LH = 0x32;
                public static byte MOT_RIGHT_POS_HL = 0x33;
                public static byte MOT_RIGHT_POS_HH = 0x34;*/
            }

            public static class Infrared
            {
                public const int SensorCount = 12;

                public static class Ambient
                {
                    public static class BackLeft
                    {
                        public const byte Low = 0x40;
                        public const byte High = 0x41;
                    }

                    public static class Left
                    {
                        public const byte Low = 0x42;
                        public const byte High = 0x43;
                    }

                    public static class FrontLeft
                    {
                        public const byte Low = 0x44;
                        public const byte High = 0x45;
                    }

                    public static class Front
                    {
                        public const byte Low = 0x46;
                        public const byte High = 0x47;
                    }

                    public static class FrontRight
                    {
                        public const byte Low = 0x48;
                        public const byte High = 0x49;
                    }

                    public static class Right
                    {
                        public const byte Low = 0x4A;
                        public const byte High = 0x4B;
                    }

                    public static class BackRight
                    {
                        public const byte Low = 0x4C;
                        public const byte High = 0x4D;
                    }

                    public static class Back
                    {
                        public const byte Low = 0x4E;
                        public const byte High = 0x4F;
                    }

                    public static class GroundLeft
                    {
                        public const byte Low = 0x50;
                        public const byte High = 0x51;
                    }

                    public static class GroundFrontLeft
                    {
                        public const byte Low = 0x52;
                        public const byte High = 0x53;
                    }

                    public static class GroundFrontRight
                    {
                        public const byte Low = 0x54;
                        public const byte High = 0x55;
                    }

                    public static class GroundRight
                    {
                        public const byte Low = 0x56;
                        public const byte High = 0x57;
                    }
                }

                public static class Proximity
                {
                    public static class BackLeft
                    {
                        public const byte Low = 0x58;
                        public const byte High = 0x59;
                    }

                    public static class Left
                    {
                        public const byte Low = 0x5A;
                        public const byte High = 0x5B;
                    }

                    public static class FrontLeft
                    {
                        public const byte Low = 0x5C;
                        public const byte High = 0x5D;
                    }

                    public static class Front
                    {
                        public const byte Low = 0x5E;
                        public const byte High = 0x5F;
                    }

                    public static class FrontRight
                    {
                        public const byte Low = 0x60;
                        public const byte High = 0x61;
                    }

                    public static class Right
                    {
                        public const byte Low = 0x62;
                        public const byte High = 0x63;
                    }

                    public static class BackRight
                    {
                        public const byte Low = 0x64;
                        public const byte High = 0x65;
                    }

                    public static class Back
                    {
                        public const byte Low = 0x66;
                        public const byte High = 0x67;
                    }

                    public static class GroundLeft
                    {
                        public const byte Low = 0x68;
                        public const byte High = 0x69;
                    }

                    public static class GroundFrontLeft
                    {
                        public const byte Low = 0x6A;
                        public const byte High = 0x6B;
                    }

                    public static class GroundFrontRight
                    {
                        public const byte Low = 0x6C;
                        public const byte High = 0x6D;
                    }

                    public static class GroundRight
                    {
                        public const byte Low = 0x6E;
                        public const byte High = 0x6F;
                    }
                }
            }
        }
    }
}