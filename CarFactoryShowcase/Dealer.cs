using System.Threading;
using System.Collections.Concurrent;
using CarFactoryShowcase;

namespace CarFactoryShowcase {

    public class Dealer {
        private int dealerID;
        private int carsSold;
        private static Object lockObj = new Object();
        private static int totalCarsSold = 0;
        private Shipping assignedTruck;
        private Semaphore carsInQueueSem;
        private Semaphore spacesAvailableSem;
        private static Barrier dealerBarrier;
        public Dealer(Shipping shipping, int id, Barrier barrier) {
            carsSold = 0;
            assignedTruck = shipping;
            carsInQueueSem = shipping.GetSemaphore(Constants.IN_QUE_INDEX);
            spacesAvailableSem = shipping.GetSemaphore(Constants.SPACES_INDEX);
            dealerID = id;
            dealerBarrier = barrier;
        }

        private void SellCar(Random rand, Car car) {

            // Sell a car

            string carDetails = car.Display();
            string sellLine = string.Format("Selling a {0}...", carDetails);
            Console.WriteLine(sellLine);
            // Sleep a random amount 
            Thread.Sleep(rand.Next(Constants.avgTimeToSellCar - 200, 
                Constants.avgTimeToSellCar + 200));

            string soldLine = string.Format("Sold a {0}!", carDetails);
            Console.WriteLine(soldLine);
            lock(lockObj) {
                totalCarsSold += 1;
            }
            carsSold += 1;
        }

        public void SellCars() {
            Random rand = new Random();
            while(true) {
                Car? car;
                carsInQueueSem.WaitOne();
                car = assignedTruck.TryDequeue(); // Grab a car off the truck queue
                spacesAvailableSem.Release();
                if (car == null) { // error handling,
                    string errorLine = string.Format("No cars found in truck by Dealer {0}", dealerID);
                    Console.WriteLine(errorLine);
                    break;
                }
                else if (car.IsLastCar()) { // sell that car and then shut down the dealer,
                    SellCar(rand, car);
                    string finalCarLine = string.Format("Dealer {0} sold the last car and is shutting down.", dealerID);
                    Console.WriteLine(finalCarLine);
                    break;
                }
                else { // otherwise, just sell that car
                    SellCar(rand, car);
                }
            }
            dealerBarrier.SignalAndWait(); // Wait until all dealers are finished
            string finishedLine = string.Format("{0} cars sold from Dealer {1}!", carsSold, dealerID);
            Console.WriteLine(finishedLine);
        }

        public int GetID() {
            return dealerID;
        }

        public static int GetTotalCarsSold() {
            return totalCarsSold;
        }
    }
}