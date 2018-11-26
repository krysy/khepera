namespace Remote.Peripherals
{
    public class Motors
    {

        public void SetMotors(int left, int right)
        {
            _remote.SendCommand(
                Opcodes.motorSetSpeed,
                new Registers.MotorSpeed(left, right)    
            );
        }
        
        public Motors(Remote remote)
        {
            _remote = remote;
        }

        private Remote _remote;
    }
}