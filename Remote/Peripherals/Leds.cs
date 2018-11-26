namespace Remote.Peripherals
{
    public class Leds
    {
        public Registers.Leds Get()
        {
            return _remote.ReadCommand<Registers.Leds>(new Opcode(Opcodes.getLeds));
        }

        public void Set(Registers.Leds leds)
        {
            _remote.SendCommand(
                Opcodes.setLeds,
                leds
            );
        }

        public Leds(Remote remote)
        {
            _remote = remote;
        }

        private Remote _remote;
    }
}