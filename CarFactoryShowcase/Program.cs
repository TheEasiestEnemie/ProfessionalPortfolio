
using CarFactoryShowcase;
using System.Collections.Concurrent;
using System.Threading;

// If you wold like to change the settings for the showcase, navigate to 
// the Constants File. Change some variables and see what happens!
namespace CarFactoryShowcase {
    
    public class Program {


        static int factoryCount = Constants.factoryCount;
        static int dealerCount = Constants.dealerCount;
        static int shippingTruckSize = Constants.shippingTruckCapacity;
        static int shippingTruckCount = Constants.shippingTruckCount;

        static List<Thread> factoryThreads = new List<Thread>();
        static List<Thread> dealerThreads = new List<Thread>();
        static List<Thread> shippingThreads = new List<Thread>();
        static List<Shipping> trucks = new List<Shipping>();
        static Barrier dealerBarrier = new Barrier(dealerCount);
        static Barrier factoryBarrier = new Barrier(factoryCount);

        static void Main(string[] args) {

            
            ///////////////// START ALL THREADS ////////////////////

            Console.WriteLine("Starting the Showcase...");

            for(int i = 0; i < shippingTruckCount; i++) {
                ConcurrentQueue<Car> carQueue = new ConcurrentQueue<Car>();
                Semaphore carsSem = new Semaphore(0, shippingTruckSize);
                Semaphore spacesSem = new Semaphore(shippingTruckSize, shippingTruckSize);
                Console.WriteLine("Creating new Truck...");
                Shipping truck = new Shipping(carQueue, carsSem, spacesSem);
                trucks.Add(truck);

                Thread sThread = new Thread(() => truck.Run());
                Console.WriteLine("New Truck Created!");

                Console.WriteLine("Adding Truck thread to a list...");
                shippingThreads.Add(sThread);
                Console.WriteLine("Added Truck thread.");

                Console.WriteLine("starting Truck thread.");
                sThread.Start();
                Console.WriteLine("started Truck thread.");
            }

            int carsPerFactory = (int)Math.Floor((double)Constants.carsToBuild / (double)factoryCount);

            for(int i = 0; i < factoryCount; i++) {
                int id = i;
                int shippingIndex = i;
                while(shippingIndex >= trucks.Count) {
                    shippingIndex -= trucks.Count;
                }

                if (i == factoryCount - 1 && Constants.carsToBuild % factoryCount != 0) {
                    carsPerFactory += 1;
                }
                Console.WriteLine("Creating new factory...");
                Factory factory = new Factory(trucks[shippingIndex], id, carsPerFactory, factoryBarrier);
                trucks[shippingIndex].AddFactory(factory);
                Thread fThread = new Thread(() => factory.BuildCars());
                Console.WriteLine("Factory created!");

                Console.WriteLine("Adding Factory thread to a list...");
                factoryThreads.Add(fThread);
                Console.WriteLine("Added Factory thread.");

                Console.WriteLine("Starting factory thread...");
                fThread.Start();
                Console.WriteLine("Started factory thread.");
            }

            for(int i = 0; i < dealerCount; i++) {
                int id = i;
                int shippingIndex = i;
                while (shippingIndex >= trucks.Count) {
                    shippingIndex -= trucks.Count;
                }

                Console.WriteLine("Creating new Dealer...");
                Dealer dealer = new Dealer(trucks[shippingIndex], id, dealerBarrier);
                trucks[shippingIndex].AddDealer(dealer);
                Thread dThread = new Thread(() => dealer.SellCars());
                Console.WriteLine("Dealer created!");

                Console.WriteLine("Adding Dealer thread to a list...");
                dealerThreads.Add(dThread);
                Console.WriteLine("Added Dealer thread.");

                Console.WriteLine("Starting Dealer thread...");
                dThread.Start();
                Console.WriteLine("Started Dealer thread.");
            }

            /////////////////// JOIN ALL THREADS ////////////////////
            
            Console.WriteLine("Waiting for results...");

            for (int i = 0; i < dealerCount; i++) {
                dealerThreads[i].Join();
            }

            for (int i = 0; i < factoryCount; i++) {
                factoryThreads[i].Join();
            }

            for (int i = 0; i < shippingTruckCount; i++) {
                shippingThreads[i].Join();
            }

            Console.WriteLine("All threads complete. The showcase is finished!");
            string line = string.Format("Total Cars sold: {0}", Dealer.GetTotalCarsSold());
            Console.WriteLine(line);
        }
    }
}