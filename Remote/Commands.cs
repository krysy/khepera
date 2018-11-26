using Remote.Peripherals;

namespace Remote
{
    public class Commands
    {
        public Motors Motors;
        public Infrared Infrared;
        public Leds Leds;
        
        public Commands(Remote remote)
        {
            Motors = new Motors(remote);
            Infrared = new Infrared(remote);
            Leds = new Leds(remote);
        }

    }
}