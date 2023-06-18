use core::time;
use std::io::{self, BufRead, Write};
use std::path::Path;
use rand::seq::SliceRandom;
use std::fs;

fn main() {
    menu();
}

fn menu() {
    // The hub of the program. Repeats until the user quits the menu.
    let mut not_done = true;
    while not_done {
        let mut choice = String::new();

        println!("\nCHOICE SELECTOR MENU: \n");
        println!("1) Add a choice");
        println!("2) Make a choice");
        println!("3) Clear choices");
        println!("4) See current choices");
        println!("5) Quit \n");

        println!("Enter an option: ");
        
        // Grabs a menu option from the user
        io::stdin()
            .read_line(&mut choice)
            .expect("Failed to read input");

        let choice: u32 = match choice.trim().parse() {
            Ok(num) => num, // returns the choice
            Err(_) => {
                println!("Please enter a number 1-5.");
                continue
            } // Restarts the loop
        };

        println!();
        
        // Interpreting the choice of the user
        if choice == 1 {
            add_choice();
        }
        else if choice == 2 {
            make_choice();
        }
        else if choice == 3 {
            clear_choices();
        }
        else if choice == 4 {
            see_choices();
        }
        else if choice == 5 {
            not_done = false;
            println!("Goodbye!");
        }
    }
}

fn add_choice() {
    // Get a choice from the user. ADD ERROR HANDLING
    let mut choice = String::new();
    loop {
        println!("Enter a possible choice to make: ");
        match io::stdin().read_line(&mut choice) {
            Ok(_) => break,
            Err(_) => println!("Please enter a choice.")
        }
    }
    
    // Write the choice to a file here
    let data_file = fs::OpenOptions::new()
    .append(true)
    .open("choices.txt");

    // verifies that the data_file is a valid File
    let file = match data_file {
        Ok(dat_file) => dat_file, // returns the data_file
        Err(_) => fs::File::create("choices.txt")
            .expect("How did we get here?") // creates a new file
    };

    // the means to write to the file, redefining the file
    let mut file = io::LineWriter::new(file);

    // Write the Choice to the file
    match file.write(choice.as_bytes()) {
        Ok(_) => println!("Added the choice \"{}\"", choice.trim()),
        Err(_) => println!("Couldn't write your choice, try again.")
    }

    std::thread::sleep(time::Duration::from_secs(1))

}

fn make_choice() {
    
    // File choices.txt 
    if let Ok(lines) = read_lines("choices.txt") {
        let mut choices: Vec<String> = vec!["0".to_string()];
        // Consumes the iterator, maps the choices to the array
        let mut i: usize = 0;
        for line in lines {
            if let Ok(ip) = line {
                if i == 0 {
                    choices[i] = ip;
                }
                else {
                    choices.push(ip)
                }
                i += 1;
            }
        };
        
        // grabs a random item in the array
        match choices.choose(&mut rand::thread_rng()) {
            None => println!("No choices to make!"), // there were no choices
            Some(choice) => println!("Your choice was: {}\n", choice)
        }
        let mut enter_line = String::new();
        
        println!("Press Enter to Continue:");
        
        match io::stdin().read_line(&mut enter_line) {
            Ok(_) => return,
            Err(_) => return
        };
    }
    else {
        println!("No file to read. Please add a choice before making one.");
        std::thread::sleep(time::Duration::from_secs(1))
    }
}

fn clear_choices() {
    // Deletes the file
    println!("Clearing All Choices...");
    fs::remove_file("choices.txt")
        .expect("No file to remove.");
    println!("Choices cleared!");

    std::thread::sleep(time::Duration::from_secs(1));
}

fn see_choices() {
    
    // File choices.txt must exist in under the choice-selector directory
    if let Ok(lines) = read_lines("choices.txt") {
        // Consumes the iterator, prints the choices
        for line in lines {
            if let Ok(ip) = line {
                println!("{}", ip);
            }
        };
        
        println!();
        
        let mut enter_line = String::new();
        println!("Press Enter to Continue:");
        
        match io::stdin().read_line(&mut enter_line) {
            Ok(_) => return,
            Err(_) => return
        }; // Don't care what it returns, used for a clearer console.
    }
    else {
        println!("No file to read. Please add choices before trying to see them.");
        std::thread::sleep(time::Duration::from_secs(1));
    }
}

// The output is wrapped in a Result to allow matching on errors
// Returns an Iterator to the Reader of the lines of the file.
fn read_lines<P>(filename: P) -> io::Result<io::Lines<io::BufReader<fs::File>>>
where P: AsRef<Path>, {
    let file = fs::File::open(filename)?;
    Ok(io::BufReader::new(file).lines())
}