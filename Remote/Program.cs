using System;
using System.Threading;
using Perceptron;


namespace Remote
{
    static class Program
    {
        class FancyLedThread
        {
            private Khepera khepera;
            private Thread _thread;

            public void DoFancy()
            {
                while (true)
                {
                    for (byte i = 0; i < 63; i++)
                    {
                        khepera.Commands.Leds.Set(
                            new Registers.Leds(
                                new Registers.Led(i,0,0),
                                new Registers.Led(0,i,0),
                                new Registers.Led(0,0,i)
                            ));
                        Thread.Sleep(20);                    
                    }
                    for (byte i = 63; i > 0; i--)
                    {
                        khepera.Commands.Leds.Set(
                            new Registers.Leds(
                                new Registers.Led(i,0,0),
                                new Registers.Led(0,i,0),
                                new Registers.Led(0,0,i)
                            ));
                        Thread.Sleep(20);                    
                    }
                    
                    for (byte i = 0; i < 63; i++)
                    {
                        khepera.Commands.Leds.Set(
                            new Registers.Leds(
                                new Registers.Led(0,i,0),
                                new Registers.Led(0,0,i),
                                new Registers.Led(i,0,0)
                            ));
                        Thread.Sleep(20);                    
                    }
                    for (byte i = 63; i > 0; i--)
                    {
                        khepera.Commands.Leds.Set(
                            new Registers.Leds(
                                new Registers.Led(0,i,0),
                                new Registers.Led(0,0,i),
                                new Registers.Led(i,0,0)
                            ));
                        Thread.Sleep(20);                    
                    }
                    
                    for (byte i = 0; i < 63; i++)
                    {
                        khepera.Commands.Leds.Set(
                            new Registers.Leds(
                                new Registers.Led(0,0,i),
                                new Registers.Led(i,0,0),
                                new Registers.Led(0,i,0)
                            ));
                        Thread.Sleep(20);                    
                    }
                    for (byte i = 63; i > 0; i--)
                    {
                        khepera.Commands.Leds.Set(
                            new Registers.Leds(
                                new Registers.Led(0,0,i),
                                new Registers.Led(i,0,0),
                                new Registers.Led(0,i,0)
                            ));
                        Thread.Sleep(20);                    
                    }
                }
            }

            public void Start()
            {
                _thread.Start(); 
            }

            public void Stop()
            {
            }

            public FancyLedThread(ref Khepera khepera)
            {
                this.khepera = khepera;
                ThreadStart threadStart = DoFancy;
                _thread = new Thread(threadStart);
            }
        }

        static void Main(string[] args)
        {
            Khepera khepera = new Khepera("192.168.8.115",2543);
            
            var fancy = new FancyLedThread(ref khepera);
            //fancy.Start();

            var motor1Perry = new Perceptron.Perceptron(-2, 2, -1);
            var motor2Perry = new Perceptron.Perceptron(8, -5, -3);
            

            //khepera.Commands.Motors.SetMotors(100,2940);

            while (true)
            {
                //Console.WriteLine($"Swapped motors! {DateTime.Now}");
                var ttl = DateTime.Now.Millisecond;
                var ir = khepera.Commands.Infrared.GetProximity();
                
                
                var infraRight1 = ir.GroundFrontRight <= 500 ? 1 : 0;
                var infraLeft1 = ir.GroundFrontLeft <= 500 ? 1 : 0;
               // int infraRight2 = ir.GroundFrontRight <= 300 ? 1 : 0;
               // int infraLeft2 = ir.GroundFrontLeft <= 300 ? 1 : 0;

                int motorRightSpeed = motor2Perry.Evaluate(infraLeft1, infraRight1) == 1 ? 100 : 0;
                int motorLeftSpeed = motor1Perry.Evaluate(infraLeft1, infraRight1) == 1 ? 100 : 0;
                //motorRightSpeed = motor2Perry.Evaluate(infraLeft2, infraRight2) == 1 ? 100 : 20;
                //motorLeftSpeed = motor1Perry.Evaluate(infraLeft2, infraRight2) == 1 ? 100 : 20;

                khepera.Commands.Motors.SetMotors(motorLeftSpeed, motorRightSpeed);
                Console.WriteLine($"LOOP TIME: {DateTime.Now.Millisecond - ttl}ms");
                //Thread.Sleep(20);                
            }
            
            /*var proximity = khepera.Commands.Infrared.GetProximity();
            var ambient = khepera.Commands.Infrared.GetAmbient();
            var leds = khepera.Commands.Leds.Get();
            */
            Console.WriteLine();
        }
    }
}