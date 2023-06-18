namespace CarFactoryShowcase {
    public class Constants {
        public static int factoryCount = 20;
        public static int dealerCount = 5;
        public static int shippingTruckCapacity = 100;
        public static int avgTimeToSellCar = 10000; // measured in milliseconds
        public static int avgTimeToBuildCar = 500; // measured in milliseconds
        public static int carsToBuild = 100;

        // CHANGE WITH CAUTION! Unfortunately, the program will hang in some 
        // circumstances if this is increased. Sorry!
        public static int shippingTruckCount = 1;


        
        // DO NOT CHANGE!
        public static int IN_QUE_INDEX = 0;
        // DO NOT CHANGE!
        public static int SPACES_INDEX = 1;
    }
}