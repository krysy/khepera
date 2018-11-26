namespace Remote.Peripherals
{
    public class Infrared
    {

        public Registers.Infrared GetAmbient()
        {
            return _remote.ReadCommand<Registers.Infrared>(new Opcode(Opcodes.irAmbient));
        }
        
        public Registers.Infrared GetProximity()
        {
            return _remote.ReadCommand<Registers.Infrared>(new Opcode(Opcodes.irProximity));
        }
        
        public Infrared(Remote remote)
        {
            _remote = remote;
        }

        private Remote _remote;
    }
}