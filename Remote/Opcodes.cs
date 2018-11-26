namespace Remote
{
    public static class Opcodes
    {
        public const int getLeds = 2048;
        public const int irAmbient = 2049;
        public const int irProximity = 2050;

        public const int motorSetSpeed = 101;
        public const int setLeds = 100;
    }

    public class Opcode
    {
        public int opcode { get; set; }

        public Opcode(int Opcode)
        {
            opcode = Opcode;
            //opcode = Opcode;
        }
    }
}