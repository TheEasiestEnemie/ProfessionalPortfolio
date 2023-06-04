using CarFactoryShowcase;

namespace CarFactoryShowcase {
    public class Factory {
        private int carsBuilt = 0;
        private int factoryID;
        private Shipping assignedTruck;
        private Semaphore carsSem;
        private Semaphore spacesSem;
        private static Object lockObj = new Object();
        private static int carsToBuild = Constants.carsToBuild;
        private static int numberOfDealers = Constants.dealerCount;
        private static Object finalLockObj = new Object();
        private static int finalCarsToBuild = numberOfDealers;
        private static Barrier factorybarrier;

        public Factory(Shipping shipping, int id, int carsToBuild, Barrier barrier) {
            factoryID = id;
            assignedTruck = shipping;
            carsSem = shipping.GetSemaphore(Constants.IN_QUE_INDEX);
            spacesSem = shipping.GetSemaphore(Constants.SPACES_INDEX);
            factorybarrier = barrier;
        }

        public void BuildCar(Random rand, bool isLastCar=false) {
            // Builds a Car

            Car newCar = new Car(isLastCar);

            string carDetails = newCar.Display();
            string buildLine = string.Format("Building a {0}...", carDetails);
            Console.WriteLine(buildLine);

            // Sleeps a random amount of time
            Thread.Sleep(rand.Next(Constants.avgTimeToBuildCar - 200,
                Constants.avgTimeToBuildCar + 200));
            
            string builtLine = string.Format("Built a {0}!", carDetails);
            Console.WriteLine(builtLine);

            // Adds a new car to the trucks queue
            assignedTruck.Enqueue(newCar);
            if (newCar.IsLastCar()) {
                Console.WriteLine("Built a Final Car!");
            }
            carsSem.Release();
        }

        public void BuildCars() {
            // Thread process that builds cars until carsToBuild == 0
            Random rand = new Random();
            bool lastCars = false;
            while (carsToBuild > 0) {
                lock(lockObj) { // needed to make sure two threads don't access carsToBuild at the same time.
                    if (carsToBuild <= 0 && finalCarsToBuild <= 0) {
                        break; // wait for all final cars to be built
                    }
                    if (carsToBuild <= numberOfDealers) {
                        string builtFinalCar = string.Format("Factory {0} is building a final car", factoryID);
                        Console.WriteLine(builtFinalCar);
                        lastCars = true; // start the final stage of production
                    }
                    carsToBuild -= 1;
                }
                spacesSem.WaitOne();
                BuildCar(rand, lastCars);

                if (lastCars) { // detect if we're in the final stages of production
                    lock (finalLockObj) {
                        finalCarsToBuild -= 1; // acts like carsToBuild, but for final cars
                    }
                }

                carsBuilt += 1; // Record how many cars each Factory builds.
            }

            factorybarrier.SignalAndWait();
            string builtAllLine = string.Format("Built {0} cars from Factory {1}",
            carsBuilt, factoryID);
            Console.WriteLine(builtAllLine);
        }
        
        public int GetID() {
            return factoryID;
        }

        public static int GetCarsToBuild() {
            return carsToBuild;
        }

        public static int GetFinalCarsToBuild() {
            return finalCarsToBuild;
        }
    }
}