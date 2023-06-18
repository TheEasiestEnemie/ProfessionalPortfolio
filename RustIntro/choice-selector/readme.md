# Overview

The Choice-Selector was asked by a couple friends to be made to help the make a choice. It's a fairly simple program that stores choices to a .txt file that persists beyond the life of the program. It makes a random choice as well as clears previous choices.

The purpose of this small project was to demonstrate what I could create with a few hours and a language I've never touched before. I learned fairly quickly the error-handling that is integral to building Rust programs. For a first program in Rust, let me know how I did!

Take a look at this video below for a quick explanation of the program and how it works.

[Choice-Selector Demo](http://youtube.link.goes.here)

# Development Environment

-VS Code with Rust plugins
-Rust
-Libraries used:
    core::time
    std::io::{self, BufRead, Write}
    std::path::Path
    rand::seq::SliceRandom
    std::fs

# Useful Websites

- [rust-lang.org](https://doc.rust-lang.org/book/)
- [rust-lang.org/std/](https://doc.rust-lang.org/std/)

# Future Work

- The program can't remove individual choices, needs to be added
- Add permanent prebuilt lists, like for fast food