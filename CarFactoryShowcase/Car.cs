using System.Threading;

namespace CarFactoryShowcase {
    public class Car {
        private static List<string> makes = new List<string>(){
            "Toyota", "Ford", "Chevrolet", "Honda", "Nissan", "Hyundai", "Dodge"
        };

        private static readonly Dictionary<string, List<string>> models = 
            new Dictionary<string, List<string>>() {
            {
                "Toyota", new List<string>(){"Corolla", "Highlander", "RAV4", 
                "Prius", "Camry", "4Runner", "Yaris", "Supra", "Sequoia"}
            },
            {
                "Ford", new List<string>(){"Edge", "F-150", "F-250", "Flex",
                "EcoSport", "Transit", "Mustang", "Escape", "Taurus"}
            },
            {
                "Chevrolet", new List<string>(){"Suburban", "Tahoe", "Silverado",
                "Traverse", "Trailblazer", "Trax", "Blazer", "Express", "Camaro"}
            },
            {
                "Honda", new List<string>(){"Civic", "Accord", "Ridgeline",
                "Clarity", "Element", "Pilot", "Insight", "Odyssey", "HR-V"}
            },
            {
                "Nissan", new List<string>(){"Altima", "Rogue", "Sentra", "Juke",
                "Kicks", "Armada", "Navara", "Murano", "Maxima", "Titan"}
            },
            {
                "Hyundai", new List<string>(){"Accent", "Aura", "Celesta",
                "Elantra", "Azera", "Lafesta", "Mistra", "Sonata"}
            },
            {
                "Dodge", new List<string>(){"Ram 1500", "Ram 2500", "Attitude",
                "Challenger", "Charger", "Durango", "Hornet", "Journey"}
            }
        };
        
        private static List<string> colors = new List<string>(){
            "Black", "White", "Red", "Light Gray", "Gray", "Brown", "Dark Blue",
            "Silver", "Green"
        };

        private string make;
        private string model;
        private int year;
        private string color;
        private bool lastCar;

        public Car(bool lastCar=false) {
            Random rand = new Random();
            
            make = makes[rand.Next(0, makes.Count)];
            model = models[make][rand.Next(0, models[make].Count)];
            color = colors[rand.Next(0, colors.Count)];
            year = rand.Next(2005, DateTime.Now.Year + 1);
            this.lastCar = lastCar;
        }

        public string Display() {
            string carDetails = string.Format("{0} {1} {2} {3}", color, year, make, model);
            return carDetails;
        }

        public bool IsLastCar() {
            return lastCar;
        }
    }
}