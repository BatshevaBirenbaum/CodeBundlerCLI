# CodeBundlerCLI

File Bundler CLI Tool
A simple yet powerful C# command-line tool that bundles multiple source files from subdirectories into a single output file, with support for sorting, filtering, annotation, and customization options.

 Key Features
 Recursively scans subdirectories (excluding bin folders)

 Filters by file extension (e.g., txt, css, html, json, java, js)

 Adds file path notes before each file (--note)

 Removes empty lines from files (--remove)

 Sorts files by name or by extension (--sort)

 Inserts author name at the top of the output file (--author)

 Supports .rsp file generation for command reuse

 bundle Command
bash
Copy
Edit
fib bundle --output path/to/output.txt --language txt,css --note --sort --remove --author "Your Name"
🛠 Options
Option	Description
--output / -o	Path to the output file
--language / -l	Comma-separated list of file extensions, or use all
--note / -n	Adds a comment line with the file path before its content
--sort / -s	Sorts files by extension; if not specified, sorts alphabetically
--remove / -r	Removes empty lines from each file
--author / -a	Adds author's name at the top of the output file

 Example
bash
Copy
Edit
fib bundle -o result.txt -l html,css,js -n -s -r -a "John Doe"
This command will:

Include files ending with .html, .css, or .js

Add their paths as comments before their content

Sort them by extension

Remove any empty lines

Insert "John Doe" at the top of the file

 Creating an .rsp File (Shortcut Script)
You can run the interactive command to generate a reusable .rsp file:

bash
Copy
Edit
fib create-rsp
The app will prompt you for input and generate a file called RspFile.rsp.

 Project Structure
Program.cs — Contains the main CLI logic and file handling operations

RspFile.rsp — A saved command script for reuse

