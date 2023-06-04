using System.Threading;
using System.Collections.Concurrent;


namespace CarFactoryShowcase {
    public class Shipping {
        private ConcurrentQueue<Car> carQueue;
        private Semaphore carsSem;
        private Semaphore spacesSem;
        private List<Factory> factories = new List<Factory>();
        private List<Dealer> dealers = new List<Dealer>();
        private bool pickingUp = true;
        public Shipping(ConcurrentQueue<Car> carQueue, Semaphore carsSem, Semaphore spacesSem) {
            this.carQueue = carQueue;
            this.carsSem = carsSem;
            this.spacesSem = spacesSem;
        }

        public void Run() {
            while (true) {
                pickingUp = true;
                if (Factory.GetCarsToBuild() == 0 && Factory.GetFinalCarsToBuild() <= 0) {
                    pickingUp = false;
                    break;
                }

                // Wait until the truck is full, and is no longer dropping off
                while (carQueue.Count < Constants.shippingTruckCapacity && pickingUp == true) {
                    Thread.Sleep(250); // change the loop to a slow poll
                    if (Factory.GetCarsToBuild() == 0) {
                        break;
                    }
                }
                pickingUp = false;

                // Wait until the truck is empty and that it isn't picking up
                while (carQueue.Count > 0 && pickingUp == false) {
                    Thread.Sleep(250); // change the loop to a slow poll
                }
            }
            Console.WriteLine("Truck route complete!");
        }

        public Car TryDequeue() {
            Car? car;
            // Wait until truck is at the dealer
            while (pickingUp) {
                Thread.Sleep(250);
            }
            lock (carQueue){
                // only pull a car if you KNOW there's a car in queue
                carQueue.TryDequeue(out car);
            }

            // Catch here if there's a inconsistency with the semaphores. DO
            // NOT allow this to continue happening if it reaches this point.
            while (car == null) {
                Console.WriteLine("Failed to find a car on the truck.");
                carsSem.WaitOne();
                Console.WriteLine("Trying to find a car on the truck...");
                carQueue.TryDequeue(out car);
            }
            return car;
        }

        public void Enqueue(Car car) {
            // Wait until truck is at the factory
            while (!pickingUp) {
                Thread.Sleep(250);
            }

            lock(carQueue) {
                // only load a car onto the truck when there are SPACES available
                carQueue.Enqueue(car);
            }
        }

        public Semaphore GetSemaphore(int semIndex) {
            if(semIndex == 0) {
                return carsSem;
            }
            else if(semIndex == 1) {
                return spacesSem;
            }
            else {
                Console.WriteLine("Invalid Semaphore Index. Returning Semaphore at index 0.");
                return carsSem;
            }
        }

        public void AddFactory(Factory factory) {
            factories.Add(factory);
        }

        public void AddDealer(Dealer dealer) {
            dealers.Add(dealer);
        }
    }
}