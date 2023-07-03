# Overview

This program acts like a recipe book. You input all your ingredients and then ask the program what recipes you can make with the current ingredients that you have. It will then return a list of recipe names that you can make. It's by no means a polished or complete program. This is a proof of concept to show what I understand about functional programming.

The purpose of this software was to demonstrate I know the basics of functional programming using the clojure language. The program concept is fairly simple. Just iterate through a couple lists and see if you have the ingredients for each recipe. Functional programming complicates this though since we can't iterate through lists and append to others the way we normally would.

Here's a quick demonstration of how it works down below:

[Recipe Book Demo Video](https://youtu.be/J0gXnTP8CLM)

# Development Environment

I built this project using cursive and clojure 1.11.1. I used the Jetbrains IntelliJ IDEA IDE and VS Code for easier reading to view the code. 

To run this project, you need to have at least clojure installed, and open up a REPL by typing clj into a console. Then navigate to the directory with the clojure file and type (load-file "recipeBook.clj").

# Future Work

Here are some things that need to be added to the program:
* I need to add a function that combines ingredient amounts if you add a duplicate of an ingredient to your ingredients list
* I need a way to subtract an entire recipe's worth of ingredients from your ingredients list, when you decide to make that recipe
* I need a way to look up recipes in case you don't know what ingredients are in a recipe