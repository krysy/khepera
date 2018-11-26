
namespace Remote
{
    public class Khepera
    {
        public Commands Commands;
        
        
        public Khepera(string address, int port = 2542)
        {
            var remote = new Remote(address, port);
            Commands = new Commands(remote);
        }
    }
}