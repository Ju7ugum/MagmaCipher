## Magma Cipher in C#
This repository contains a C# implementation of the Magma encryption algorithm. The Magma algorithm is part of a family of block ciphers used in Russia for encryption purposes. The project implements encryption and decryption functions, including feedback gamming modes.
## Peculiarities
- **Encryption**: Magma encryption in CBC (Cipher Block Chaining) mode and with feedback is implemented.
- **Decryption**: Supports decryption of text encrypted using the Magma algorithm.
- **16-byte block support**: All encryption/decryption operations use 128-bit blocks.
- **GOST 34.12-2015**: The GOST cryptographic standard for the Magma cipher is implemented.

## Usage
After starting the program, you can select one of the menu items and test the program. The program was developed and tested according to GOST 32.12-2015, which will also be attached.

### How to build the project
Clone the repository:
```bash
git clone https://github.com/Ju7ugum/MagmaCipher.git

Open the solution in Visual Studio 2022.
Build the project to compile the executable.

Files  
Magma.cs: Contains the main logic for encryption and decryption.
Program.cs: The main entry point for running the application.
.gitignore: The default ignore file for Git.
MagmaS.sln: The solution file for Visual Studio.


Developed by Ju7ugum.
