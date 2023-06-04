namespace CarFactoryShowcase {
    public class Constants {
        public static int factoryCount = 2;
        public static int dealerCount = 5;
        public static int shippingTruckCapacity = 10;
        public static int avgTimeToSellCar = 500; // measured in milliseconds
        public static int avgTimeToBuildCar = 500; // measured in milliseconds
        public static int carsToBuild = 200;

        // CHANGE WITH CAUTION! Unfortunately, the program will hang in some 
        // circumstances if this is increased. Sorry!
        public static int shippingTruckCount = 1;


        
        // DO NOT CHANGE!
        public static int IN_QUE_INDEX = 0;
        // DO NOT CHANGE!
        public static int SPACES_INDEX = 1;
    }
}