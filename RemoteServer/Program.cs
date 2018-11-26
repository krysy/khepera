using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace RemoteServer
{
    class Program
    {

        public class Watchdog
        {
            private int _counter = 0;

            public void Feed()
            {
                _counter = 0;
            }

            public void Watch()
            {
                while (true)
                {

                    if (_counter >= 500)
                    {
                        Hardware.Motors.SetSpeed(0,0);
                        _counter = 0;
                    }
                
                    _counter++;
                    Thread.Sleep(1);
                }
            }

            public Watchdog()
            {
                var thread = new Thread(() =>
                {
                    while (true)
                    {

                        if (_counter >= 500)
                        {
                            Hardware.Motors.SetSpeed(0,0);
                            _counter = 0;
                        }
                
                        _counter++;
                        Thread.Sleep(1);
                    }
                });
                
                thread.Start();
            }
        }

        private class Packet
        {
            public int Opcode;
            public Object Data;

            public Packet(int opcode, Object data)
            {
                Opcode = opcode;
                Data = data;

            }
        }
        
        static void Main(string[] args)
        {
            Hardware.I2C.OpenBus("/dev/i2c-2");
            
           // var dog = new Watchdog();
            //ThreadStart doggy = dog.Watch;
            //hread watchdog = new Thread(doggy);
            //watchdog.Start();
            var dog = new Watchdog();
            dog.Feed();
            
            
            
            TcpListener server = new TcpListener(IPAddress.Any,2543);
            server.Start();
            Console.WriteLine("Server started!");
            
            Hardware.Leds.SetFrontRight(
               63,0,0
            );
            Hardware.Leds.SetFrontLeft(
                0,63,0
            );
            Hardware.Leds.SetBack(
                0,0,63
            );

            while (true)
            {
                var client = server.AcceptTcpClient();

                var stream = client.GetStream();

                //var buffer = new byte[1000];
                while (client.Connected)
                {
                    var buffer = new byte[1024];
                    stream.Read(buffer, 0, buffer.Length);
                    
                    //decode packet
                    var packet = JsonConvert.DeserializeObject<Packet>(Encoding.ASCII.GetString(buffer));
                   // Console.WriteLine($"Opcode: {packet.Opcode}");
                    stream.Flush();

                  
                    switch (packet.Opcode)
                    {
                        case Opcodes.MotorSetSpeed:
                            var motorSpeed = JsonConvert.DeserializeObject<Registers.MotorSpeed>(JsonConvert.SerializeObject(packet.Data));
                            /*Console.WriteLine("--------------------");
                            Console.WriteLine("Setting motor speeds");
                            Console.WriteLine($"Left: {motorSpeed.Left}");
                            Console.WriteLine($"Right: {motorSpeed.Right}");*/
                            Hardware.Motors.SetSpeed(motorSpeed.Left,motorSpeed.Right);
                            dog.Feed();
                            break;
                        
                        case Opcodes.SetLeds:
                            var leds = JsonConvert.DeserializeObject<Registers.Leds>(JsonConvert.SerializeObject(packet.Data));
                            /*Console.WriteLine("------------");
                            Console.WriteLine("Setting leds");
                            Console.WriteLine($"Back R:{leds.Back.Red} G:{leds.Back.Green} B:{leds.Back.Red}");
                            Console.WriteLine($"FrontR R:{leds.FrontLeft.Red} G:{leds.FrontLeft.Green} B:{leds.FrontLeft.Blue}");
                            Console.WriteLine($"FrontR R:{leds.FrontRight.Red} G:{leds.FrontRight.Green} B:{leds.FrontRight.Red}");
                            */
                            Hardware.Leds.SetFrontRight(
                                leds.FrontRight.Red,
                                leds.FrontRight.Green,
                                leds.FrontRight.Blue
                            );
                            Hardware.Leds.SetFrontLeft(
                                leds.FrontLeft.Red,
                                leds.FrontLeft.Green,
                                leds.FrontLeft.Blue
                            );
                            Hardware.Leds.SetBack(
                                leds.Back.Red,
                                leds.Back.Green,
                                leds.Back.Blue
                            );
                            break;
                        
                        case Opcodes.IrProximity:
                            Hardware.Infrared.Proximity.ReloadValues();
                            var json = JsonConvert.SerializeObject(
                                new Registers.Infrared(
                                   Hardware.Infrared.Proximity.BackLeft,
                                   Hardware.Infrared.Proximity.Left,
                                   Hardware.Infrared.Proximity.FrontLeft,
                                   Hardware.Infrared.Proximity.Front,
                                   Hardware.Infrared.Proximity.FrontRight,
                                   Hardware.Infrared.Proximity.Right,
                                   Hardware.Infrared.Proximity.BackRight,
                                   Hardware.Infrared.Proximity.Back,
                                   Hardware.Infrared.Proximity.GroundLeft,
                                   Hardware.Infrared.Proximity.GroundFrontLeft,
                                   Hardware.Infrared.Proximity.GroundFrontRight,
                                   Hardware.Infrared.Proximity.GroundRight
                            ));
                            stream.Write(Encoding.ASCII.GetBytes(json),0,json.Length);
                            break;
                        
                    }
                    
                    client.Close();
                }
            }
        }
    }
}