using System;
using System.Linq;
using System.Threading;
using System.Transactions;
using System;
using System.Net.Http.Headers;

namespace Perceptron{

    public class Perceptron{
        public int Weight1{ get; set; }
        public int Weight2{  get; set; }
        public int Threshold{  get; set; }
        
        public int Evaluate(int input1, int input2){
            return ((input1 * Weight1) + (input2 * Weight2)) > 1 * Threshold ? 1 : 0;
        }
        
        public void AdjustWeights(int weight1, int weight2, int threshold){
            Weight1 = weight1;
            Weight2 = weight2;
            Threshold = threshold;
        }

        public Perceptron(int weight1 =0, int weight2=0, int threshold=0){
            Weight1 = weight1;
            Weight2 = weight2;
            Threshold = threshold;
        }
    }

    public class Network{
        
    }

    class Program
    {
        private static bool mainThreadRunning = true;
        
        static void TrainModel(int[][] desiredValues, ref Perceptron perry){
            int weight1, weight2, threshold;
            //var iteration = 0;
            Random rand = new Random();
            foreach (var value in desiredValues){
                while (perry.Evaluate(value[0], value[1]) != value[2]){
                    weight1 = rand.Next(-10, 10);
                    weight2 = rand.Next(-10, 10);
                    threshold = rand.Next(-10, 10);
                    perry.AdjustWeights(weight1, weight2, threshold);
                    //iteration++;
                }
            }
        }

        static int TrainPerceptron(int[][] desiredValues, ref Perceptron perry ){
            int[][] result = {
                new[]{0, 0, 0},
                new[]{0, 1, 0},
                new[]{1, 0, 0},
                new[]{1, 1, 0}
            };

            int totalIterations = 0;
            int i;
            bool eq1, eq2, eq3, eq4;
            
            while (true){
                TrainModel(desiredValues, ref perry);
                totalIterations++;
                //Console.WriteLine();

                i = 0;
                foreach (var val in desiredValues){
                  //  Console.WriteLine($"{val[0]}|{val[1]}|{perry.Evaluate(val[0], val[1])}");
                    //construct result
                    var tmp = new int[]{val[0], val[1], perry.Evaluate(val[0], val[1])};
                    result[i] = tmp;
                    i++;
                }

                eq1 = desiredValues[0][2] == result[0][2];
                eq2 = desiredValues[1][2] == result[1][2];
                eq3 = desiredValues[2][2] == result[2][2];
                eq4 = desiredValues[3][2] == result[3][2];


                if (eq1 && eq2 && eq3 && eq4){
                    break;
                }

                if (totalIterations >= 1023){
                    throw new Exception("Probably impossible to complete!");
                }
            }

            return totalIterations;
        }

        static void ConsoleAbortThread(){
            while (true){
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.Q)
                {
                    mainThreadRunning = false;
                    Thread.Sleep(100);
                    Environment.Exit(1);
                }
            }
        }

        static void Main(string[] args){
            var andPerry = new Perceptron();
            var nandPerry = new Perceptron();
            var orPerry = new Perceptron();
            var norPerry = new Perceptron();
            var dunnoPerry = new Perceptron();
            
            var motor1Perry = new Perceptron(-2, 2, -1);
            var motor2Perry = new Perceptron(8, -5, -3);


            var net0 = new[]
            {
                new Perceptron(),
                new Perceptron(),
                new Perceptron(),
                new Perceptron()
            };

            var net1 = new[]
            {
                new Perceptron(),
                new Perceptron(),
                new Perceptron(),
                new Perceptron(),
            };
            throw new Exception("Fuck this");
            
        /*    
            var totalIterations = 0;

            ThreadStart threadStart = ConsoleAbortThread;
            Thread thread = new Thread(threadStart);
            thread.Start();
            #region logicGates

            int[][] andGate = {
                new[]{0, 0, 0},
                new[]{0, 1, 0},
                new[]{1, 0, 0},
                new[]{1, 1, 1}
            };
            int[][] nandGate = {
                new[]{0, 0, 1},
                new[]{0, 1, 1},
                new[]{1, 0, 1},
                new[]{1, 1, 0}
            };
            
            int[][] orGate = {
                new[]{0, 0, 0},
                new[]{0, 1, 1},
                new[]{1, 0, 1},
                new[]{1, 1, 1}
            };
            int[][] norGate = {
                new[]{0, 0, 1},
                new[]{0, 1, 0},
                new[]{1, 0, 0},
                new[]{1, 1, 0}
            };
            int[][] dunnoGate = {
                new[]{0, 0, 1},
                new[]{0, 1, 1},
                new[]{1, 0, 1},
                new[]{1, 1, 1}
            };
            

            #endregion
            //[2] = output, [0,1] = input values
            int[][] motor1Gate = {
                new[]{0, 0, 1},
                new[]{0, 1, 1},
                new[]{1, 0, 0},
                new[]{1, 1, 1}
            };
            int[][] motor2Gate = {
                new[]{0, 0, 1},
                new[]{0, 1, 0},
                new[]{1, 0, 1},
                new[]{1, 1, 1}
            };
            
            
            
            //train perceptrons here
            //totalIterations += TrainPerceptron(motor1Gate, ref motor1Perry);
            //totalIterations += TrainPerceptron(motor2Gate, ref motor2Perry);
           // Console.WriteLine($"Trained in {totalIterations} iterations!");

            var resultTable = $@"
 ~M1~   ~M2~
------ ------
0|0|{motor1Perry.Evaluate(0,0)}| 0|0|{motor2Perry.Evaluate(0,0)}|
0|1|{motor1Perry.Evaluate(0,1)}| 0|1|{motor2Perry.Evaluate(0,1)}|
1|0|{motor1Perry.Evaluate(1,0)}| 1|0|{motor2Perry.Evaluate(1,0)}| 
1|1|{motor1Perry.Evaluate(1,1)}| 1|1|{motor2Perry.Evaluate(1,1)}|
";
            Console.WriteLine(resultTable);

            I2C.openBus("/dev/i2c-2");
            while (mainThreadRunning){
                Infrared.Proximity.ReloadValues();
                int infraRight1 = Infrared.Proximity.GroundFrontRight <= 500 ? 1 : 0;
                int infraLeft1 = Infrared.Proximity.GroundFrontLeft <= 500 ? 1 : 0;
                int infraRight2 = Infrared.Proximity.GroundFrontRight <= 300 ? 1 : 0;
                int infraLeft2 = Infrared.Proximity.GroundFrontLeft <= 300 ? 1 : 0;

                int motorRightSpeed = motor2Perry.Evaluate(infraLeft1, infraRight1) == 1 ? 250 : 0;
                int motorLeftSpeed = motor1Perry.Evaluate(infraLeft1, infraRight1) == 1 ? 250 : 0;
                motorRightSpeed = motor2Perry.Evaluate(infraLeft2, infraRight2) == 1 ? 350 : 0;
                motorLeftSpeed = motor1Perry.Evaluate(infraLeft2, infraRight2) == 1 ? 350 : 0;

                Console.WriteLine($"infraRightVal: {Infrared.Proximity.GroundFrontRight} infraLeftVal: {Infrared.Proximity.GroundFrontLeft}");
                Console.WriteLine($"infraRight: {infraRight1} infraLeft: {infraLeft1}");
                Motors.SetSpeed(motorLeftSpeed, motorRightSpeed);
                Thread.Sleep(10);
            }
            Motors.SetSpeed(0,0);

            /*Console.WriteLine();
            totalIterations += TrainPerceptron(andGate, ref andPerry);
            totalIterations += TrainPerceptron(nandGate, ref nandPerry);
            totalIterations += TrainPerceptron(orGate, ref orPerry);
            totalIterations += TrainPerceptron(norGate, ref norPerry);
            totalIterations += TrainPerceptron(dunnoGate, ref dunnoPerry);
            int y = 0;
            switch (y){
                case 0:
                    while (y != 8){
                        Console.WriteLine("Is 0!");
                        y++;
                    }
                    break;
                case 8:
                    Console.WriteLine("Is 8!");
                    break;
                default:
                    Console.WriteLine("Is !8||!0");
                    break;
            }

            throw new Exception("fuck this");

            var resultTable = $@"
~AND~  ~NAND~  ~OR~   ~NOR~ ~DUNNO~
------ ------ ------ ------ -------
0|0|{andPerry.Evaluate(0,0)}| 0|0|{nandPerry.Evaluate(0,0)}| 0|0|{orPerry.Evaluate(0,0)}| 0|0|{norPerry.Evaluate(0,0)}| 0|0|{dunnoPerry.Evaluate(0,0)}| 
0|1|{andPerry.Evaluate(0,1)}| 0|1|{nandPerry.Evaluate(0,1)}| 0|1|{orPerry.Evaluate(0,1)}| 0|1|{norPerry.Evaluate(0,1)}| 0|1|{dunnoPerry.Evaluate(0,1)}| 
1|0|{andPerry.Evaluate(1,0)}| 1|0|{nandPerry.Evaluate(1,0)}| 1|0|{orPerry.Evaluate(1,0)}| 1|0|{norPerry.Evaluate(1,0)}| 1|0|{dunnoPerry.Evaluate(1,0)}| 
1|1|{andPerry.Evaluate(1,1)}| 1|1|{nandPerry.Evaluate(1,1)}| 1|1|{orPerry.Evaluate(1,1)}| 1|1|{norPerry.Evaluate(1,1)}| 1|1|{dunnoPerry.Evaluate(1,1)}| 
";
            Console.WriteLine(resultTable);
            Console.WriteLine($"That took {totalIterations} iterations");
            */

        }
    }
}