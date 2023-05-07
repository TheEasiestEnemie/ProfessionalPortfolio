# Overview

This first section of the Terraria Block Texture Picker is the database that holds all the textures and asssociated JSON information. This is a read only database that will be used to display blocks on the website.

The python files here each do their own things, and are meant to be run seperately. 

texture_setup initializes the json_tiles.json file.

The database_setup loads all the json files onto the database. 

The delete_images file goes through all the rest of the trash images that were added and deletes them, leaving only the images we want.

The retrieve_an_image file is a demonstration of how the database will be called and how it returns .png files.

Here's a link to a video demonstration of what the database looks like and how it works.

[Software Demo Video](https://youtu.be/4WTexTKFYoo)

# Cloud Database

The database I decided to use was Google's firebase Realtime database, it's a little overkill for a project like this, but I wanted to familiarize myself with something new.

The database merely holds the JSON data in the Realtime database, and the images in storage.

# Development Environment

Firebase was a great help with creating my project, but I will admit it was a steep learning curve for someone who's never connected to any database using python before.

I made use of the python language to communicate with the database, using the firebase_admin library to connect to firebase.

Here's a website that helped me get started using firebase_admin.

- [Free Code Camp - How to Get Started With Firebase Using Python](https://www.freecodecamp.org/news/how-to-get-started-with-firebase-using-python/)

# Future Work

A couple things that need to be worked on in the future:

- To start, I need to remove the firebaseconfig file as its the key to the database.
- I may need to add the item textures to the database to display to the website.