/*
Hello, thank you for using kuNIX. kuNIX is open source and
free to use. Developed by Scott and YoPoster, along with help
from gOS and SharpOS for some code. kuNIX is based on the
CosmosOS kernel, drivers and bootloader.

For users:
Feel free to look through the source and edit whatever you like!

For developers:
You can use the kuNIX kernel however you like. kuNIX has a GPL 3.0
license, meaning the software is free and open source. To make a
graphical version of kuNIX, consider using CGS (Cosmos Graphic
Subsystem) for making a GUI. We highly recommend that you heavily
modify this kernel for graphical use. Rather than typing characters,
you'd be using it graphically, so it'd make more sense to remove
Console text and inputs all together and have the GUI send data
to the kernel in a syscall.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.HAL.Drivers.PCI.Video;
using Cosmos.System.FileSystem;
using Cosmos.Debug.Kernel;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Threading.Tasks;
using Cosmos.Core.IOGroup;
using Cosmos.System.Emulation;
using Cosmos.System.ExtendedASCII;

namespace kunix
{
    public class Kernel : Sys.Kernel {
        string current_directory = @"0:\\";

        Canvas canvas; // 32 Bit Depth

        private readonly Bitmap bitmap = new Bitmap(10, 10,
                new byte[] { 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0,
                    255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255,
                    0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255,
                    0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 23, 59, 88, 255,
                    23, 59, 88, 255, 0, 255, 243, 255, 0, 255, 243, 255, 23, 59, 88, 255, 23, 59, 88, 255, 0, 255, 243, 255, 0,
                    255, 243, 255, 0, 255, 243, 255, 23, 59, 88, 255, 153, 57, 12, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255,
                    243, 255, 0, 255, 243, 255, 153, 57, 12, 255, 23, 59, 88, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243,
                    255, 0, 255, 243, 255, 0, 255, 243, 255, 72, 72, 72, 255, 72, 72, 72, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0,
                    255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 72, 72,
                    72, 255, 72, 72, 72, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255,
                    10, 66, 148, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255,
                    243, 255, 10, 66, 148, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 10, 66, 148, 255, 10, 66, 148, 255,
                    10, 66, 148, 255, 10, 66, 148, 255, 10, 66, 148, 255, 10, 66, 148, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255,
                    243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 10, 66, 148, 255, 10, 66, 148, 255, 10, 66, 148, 255, 10, 66, 148,
                    255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255,
                    0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, 0, 255, 243, 255, }, ColorDepth.ColorDepth32);

        protected override void BeforeRun() {
            Console.WriteLine("Checking for kuNIX......................\n");  // checks if kuNIX is already installed
            if(Directory.Exists(@"0:\\kunix")) {
                bool installed = true;
            }
            else {
                bool installed = false;
            }
            if (installed == false) {
                Console.WriteLine("[FAILED]   kuNIX not installed......\n");
                Console.WriteLine("[OK]   Installing...................\n");
                Console.WriteLine("       ___________");
                Console.WriteLine("      |.---------.|");
                Console.WriteLine("      ||         ||");
                Console.WriteLine("      ||  kuNIX  ||");
                Console.WriteLine("      ||         ||");
                Console.WriteLine("      |'---------'|");
                Console.WriteLine("       `)__ ____('");
                Console.WriteLine("       [=== -- o ]   ");
                Console.WriteLine("     __'---------'__ ");
                Console.WriteLine("    [::::::::::: :::] ");
                System.Threading.Thread.Sleep(5000);
                Console.Clear();
                Console.Clear();
                Directory.CreateDirectory(@"0:\\kunix");
                current_directory = (@"0:\\kunix");
                var file = File.Create(current_directory + "host.txt");  // this is the file that declares the computer name
                Console.Write(current_directory + "[installer:host] > ");
                string hostname = Console.ReadLine();
                byte contents = byte.Parse(hostname);
                file.WriteByte(contents);
                file.Close();
                file = File.Create(current_directory + "bg.txt");  // console background color
                contents = byte.Parse("black");
                file.WriteByte(contents);
                file.Close();
                file = File.Create(current_directory + "fg.txt");  //  console foregroud color
                contents = byte.Parse("white");
                file.WriteByte(contents);
                file.Close();
                Directory.CreateDirectory(@"0:\\kunix\applications\");  // creates home directory
                Directory.CreateDirectory(@"0:\\home\");  // creates home directory
                current_directory = (@"0:\\home\");
                file = File.Create(current_directory + "manual.txt");  //  help manual
                contents = byte.Parse("Welcome to kuNIX! An operating system made by Scott and YoPoster.\nGetting Started:\nkuNIX is open source, meaning you are free to edit, distribute, share, and do whatever you want with the code. (pretty much)\nkuNIX has a GNU GPL 3.0 license, which grants these permissions to the user.\n\nInstalling:\nkuNIX automatically installs itself.\nThis installation includes:\nBackground configurations\nForeground configuration\nHome directory\nHostname/Computer name\nSettings\n\nCommands:\nhelp - reads help manual\nlscon - lists files and subdirectories in current directory\nshutdown - powers computer off\nreboot - reboots computer\necho - echoes phrase\nsto - writes to file\nappend - writes to file in a new line\nmkdir - creates directory\nbuild - creates file\nxpl - explore directories\nread - prints content of a file\nlsvol - lists volumes\nbg (color) - changes background color\ncolor (color) - changes foreground color\nclear - clears console\ncalc - does math\nbeep - beeps\nbday - plays a special tune\nunlink - deletes file\ntime - displays time\nuldir - deletes a directory\nsys - displays computer information\nver - shows kuNIX version\nrename - renames file\nrelocate - moves file\nrelocdir - moves directory\nepoch - shows seconds since 1/1/1970 0:00");
                file.WriteByte(contents);
                file.Close();
                Console.WriteLine("[OK]   Successfully installed kuNIX.\n");
            }
            FS = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(FS);
            FS.Initialize();  // allocates filesystem and stuff
            Console.WriteLine("[OK]   Scanning filesystems.........\n");
            Console.WriteLine("[OK]   Allocating memory............\n");
            Console.WriteLine("[OK]   Loading configurations.......\n");
            string bgfile = (@"0:\\kunix\bg.txt");
            string fgfile = (@"0:\\kunix\fg.txt");
            current_directory = (@"0:\\home");
            string bgconfig = File.ReadAllText(bgfile);
            string fgconfig = File.ReadAllText(fgfile);
            Console.BackgroundColor = ConsoleColor.bgconfig;  // loads background color configurations
            Console.ForegroundColor = ConsoleColor.fgconfig;  // loads foreground color configurations
            Console.Clear();
            Console.WriteLine("Welcome to kuNIX.");
        }
        public static bool graphical = false;
        public static bool debugText = false;
        public static float cpuUsage = 0;  // declares CPU usage as 0, pretty self-explanatory
        string hostfile = (@"0:\\kunix\host.txt");  // loads computer name
        public static string ComputerName = File.ReadAllText(hostfile);
        bool running = true;
        public Cosmos.System.FileSystem.CosmosVFS FS = null;
        protected override void Run()
        {
            while (running) {
                Console.Write("\n" + current_directory + "@" + ComputerName + " > ");
                string input = Console.ReadLine();  // takes input
                interpret(input);
            }
        }
        public void interpret(string input) {  // interprets command
            string[] args = input.Split(' ');
            if (input.StartsWith("shutdown")) {
                Console.Clear();
                Console.WriteLine("\n[OK]   Exiting kuNIX................\n");
                Console.WriteLine("[OK]   Shutting down................\n");
                Console.WriteLine("Thank you for choosing kuNIX.\n");
                running = false;
                Cosmos.Sys.Deboot.ShutDown();  // shutdown command
            }
            else if (input.StartsWith("reboot"))
            {
                Console.Clear();
                Console.WriteLine("\n[OK]   Exiting kuNIX................\n");
                Console.WriteLine("[OK]   Starting reboot sequence.....\n");
                Console.WriteLine("Thank you for choosing kuNIX.\n");
                Sys.Power.Reboot();  // reboot command
            }
            else if (input.StartsWith("echo "))
            {
                try
                {
                    Console.WriteLine(input.Remove(0, 5));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nECHO: " + ex.Message);
                }
            }
            else if (input.StartsWith("lscon"))  // lists files in current directory
            {
                Console.WriteLine("Type\tSize\tName");
                foreach (var dir in Directory.GetDirectories(current_directory))
                {
                    Console.WriteLine("<DIR>\t-\t" + dir);
                }
                foreach (var dir in Directory.GetFiles(current_directory))
                {
                    FileInfo finf = new FileInfo(dir);
                    Console.WriteLine(finf.Extension + "\t" + finf.Length + "\t" + dir);
                }
            }
            else if(input.StartsWith("sto"))  // writes to a file
            {
                Console.Write("\n" + current_directory + "[sto:file] > ");
                string writefile = Console.ReadLine();
                Console.Write("\n" + current_directory + "[sto:content] > ");
                string writecontent = Console.ReadLine();
                var file = writefile;
                byte contents = byte.Parse(writecontent);  // write content to file
                file.WriteByte(contents);
                file.Close();
            }
            else if(input.StartsWith("append"))  // appends to a file
            {
                Console.Write("\n" + current_directory + "[append:file] > ");
                string writefile = Console.ReadLine();
                string appendcontent = File.ReadAllText(writefile);
                Console.Write("\n" + current_directory + "[append:content] > ");
                string writecontent = Console.ReadLine();
                var file = writefile;
                byte contents = byte.Parse(appendcontent + "\n" + writecontent);  // append content to file
                file.WriteByte(contents);
                file.Close();
            }
            else if(input.StartsWith("mkdir "))
            {
                string dir = input.Remove(0, 6);
                Directory.CreateDirectory(current_directory + dir);  // creates directory
            }
            else if(input.StartsWith("build "))
            {
                string touchp = input.Remove(0, 6);
                File.Create(current_directory + touchp);  // creates file
            }
            else if(input.StartsWith("xpl "))
            {
                var newdir = input.Remove(0, 4);
                if(FS.GetDirectory(current_directory + newdir) != null)
                {
                    current_directory = current_directory + newdir + @"\\";  // changes current directory
                }
                else
                {
                    if(newdir == "..")
                    {
                        var dir = FS.GetDirectory(current_directory);
                        string p = dir.mParent.mName;
                        if (!string.IsNullOrEmpty(p))
                        {
                            current_directory = p;  // changes current directory to parent directory
                        }
                    }
                }
            }
            else if (input.StartsWith("read "))
            {
                string file = input.Remove(0, 5);
                    if (File.Exists(file)) {
                        Console.WriteLine(File.ReadAllText(file));  // prints file contents
                    } else {
                        Console.WriteLine("\nEXCEPTION: File doesn't exist.");
                    }
 
            }
            else if (input.StartsWith("lsvol"))
            {
                var vols = FS.GetVolumes();
                Console.WriteLine("\nName\tSize\tParent");
                foreach (var vol in vols)
                {
                    Console.WriteLine(vol.mName + "\t" + vol.mSize + "\t" + vol.mParent);  // lists volumes
                }
            }


            // for changing background and foreground colors


            else if (input.StartsWith("bg blue")) {
                var file = (@"0:\\kunix\bg.txt");
                byte contents = byte.Parse("blue");
                file.WriteByte(contents);
                file.Close();
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Clear();
            }
            else if (input.StartsWith("bg red")) {
                var file = (@"0:\\kunix\bg.txt");
                byte contents = byte.Parse("red");
                file.WriteByte(contents);
                file.Close();
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Clear();
            }
            else if (input.StartsWith("bg black")) {
                var file = (@"0:\\kunix\bg.txt");
                byte contents = byte.Parse("black");
                file.WriteByte(contents);
                file.Close();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
            }
            else if (input.StartsWith("bg green")) {
                var file = (@"0:\\kunix\bg.txt");
                byte contents = byte.Parse("green");
                file.WriteByte(contents);
                file.Close();
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Clear();
            }
            else if (input.StartsWith("bg white")) {
                var file = (@"0:\\kunix\bg.txt");
                byte contents = byte.Parse("white");
                file.WriteByte(contents);
                file.Close();
                Console.BackgroundColor = ConsoleColor.White;
                Console.Clear();
            }
            else if (input.StartsWith("bg yellow")) {
                var file = (@"0:\\kunix\bg.txt");
                byte contents = byte.Parse("yellow");
                file.WriteByte(contents);
                file.Close();
                Console.BackgroundColor = ConsoleColor.Yellow;
                Console.Clear();
            }
            else if (input.StartsWith("color white")) {
                var file = (@"0:\\kunix\fg.txt");
                byte contents = byte.Parse("white");
                file.WriteByte(contents);
                file.Close();
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
            }
            else if (input.StartsWith("color yellow")) {
                var file = (@"0:\\kunix\fg.txt");
                byte contents = byte.Parse("yellow");
                file.WriteByte(contents);
                file.Close();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Clear();
            }
            else if (input.StartsWith("color red")) {
                var file = (@"0:\\kunix\fg.txt");
                byte contents = byte.Parse("red");
                file.WriteByte(contents);
                file.Close();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Clear();
            }
            else if (input.StartsWith("color blue")) {
                var file = (@"0:\\kunix\fg.txt");
                byte contents = byte.Parse("blue");
                file.WriteByte(contents);
                file.Close();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Clear();
            }
            else if (input.StartsWith("color green")) {
                var file = (@"0:\\kunix\fg.txt");
                byte contents = byte.Parse("green");
                file.WriteByte(contents);
                file.Close();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Clear();
            }
            else if (input.StartsWith("color black")) {
                var file = (@"0:\\kunix\fg.txt");
                byte contents = byte.Parse("black");
                file.WriteByte(contents);
                file.Close();
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Clear();
            }
            else if (input.StartsWith("clear")) {
                Console.Clear();
            }
            else if (input.StartsWith("calc")) {
                double a, b;
                char c;
                Console.Write("\n" + current_directory + "[calc:operand1] > ");
                a = Double.Parse(Console.ReadLine());
                Console.Write("\n" + current_directory + "[calc:operand2] > ");
                b = Double.Parse(Console.ReadLine());
                Console.Write("\n" + current_directory + "[calc:operator] > ");
                c = Char.Parse(Console.ReadLine());
                switch (c)
                {
                    // does math
                    case '+':
                        Console.WriteLine("\n{0}+{1}={2}", a, b, a + b);
                        break;
                    case '-':
                        Console.WriteLine("\n{0}-{1}={2}", a, b, a - b);
                        break;
                    case '*':
                        Console.WriteLine("\n{0}*{1}={2}", a, b, a * b);
                        break;
                    case '/':
                        Console.WriteLine("\n{0}/{1}={2}", a, b, a / b);
                        break;
                    default:
                        Console.WriteLine("\nEXCEPTION: Unknown operator.");
                        break;
                }
            }
            else if (input.StartsWith("unlink ")) {
                string delpath = input.Remove(0, 7);
                File.Delete(delPath);  // deletes file
            }
            else if (input.StartsWith("time")) {
                DateTime current = DateTime.Now;
                Console.WriteLine("\n");
                Console.WriteLine($"{current}");  // displays time
            }
            else if (input.StartsWith("beep")) {  // beeps, WARNING: THIS WILL BE LOUD AS THE SOUNDS AREN'T LOADED FROM A SOUNDCARD AND RATHER THE CPU
                Console.Beep(800, 100);
            }
            else if (input.StartsWith("bday")) {  // plays birthday tune :D
                Console.Beep(264, 125);
                Console.Beep(264, 125);
                Console.Beep(297, 500);
                Console.Beep(264, 500);
                Console.Beep(352, 500);
                Console.Beep(330, 1000);
                Console.Beep(264, 125);
                Console.Beep(264, 125);
                Console.Beep(297, 500);
                Console.Beep(264, 500);
                Console.Beep(396, 500);
                Console.Beep(352, 1000);
                Console.Beep(264, 125);
                Console.Beep(264, 125);
                Console.Beep(2642, 500);
                Console.Beep(440, 500);
                Console.Beep(352, 250);
                Console.Beep(352, 125);
                Console.Beep(330, 500);
                Console.Beep(297, 1000);
                Console.Beep(466, 125);
                Console.Beep(466, 125);
                Console.Beep(440, 500);
                Console.Beep(352, 500);
                Console.Beep(396, 500);
                Console.Beep(352, 1000);
            }
            else if (input.StartsWith("uldir")) {
                string subPath = input.Remove(0, 6);
                try
                {
                    Directory.Delete(subPath);  // deletes directory
                    bool subDirectoryExists = Directory.Exists(subPath);
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nEXCEPTION: {0}.", e.Message);
                }
            }
            else if (input.StartsWith("sys")) {
                Console.WriteLine("\nRAM: " + Cosmos.Core.CPU.GetAmountOfRAM().ToString());  // system ram
                Console.WriteLine("\nFree Space: " + Sys.FileSystem.VFS.VFSManager.GetTotalFreeSpace("0") + " bytes");  // computer free space
                Console.WriteLine("\nRoot Drive Size: " + Sys.FileSystem.VFS.VFSManager.GetTotalSize("0") + " bytes");  // root drive space
            }
            else if (input.StartsWith("ver"))
            {
                Console.WriteLine("\n");
                Console.WriteLine("\nkuNIX 1.0");
                Console.WriteLine("\nDeveloped By:");
                Console.WriteLine("\nScott");
                Console.WriteLine("\nYoPoster");
                Console.WriteLine("\nSpecial thanks to:");
                Console.WriteLine("\nSharpOS");
                Console.WriteLine("\ngOS");
                Console.WriteLine("\nCosmos Team");
            }
            else if (input.StartsWith("epoch"))
            {
                TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                int secondsSinceEpoch = (int)t.TotalSeconds;
                Console.WriteLine("\n" + secondsSinceEpoch);  // displays time since unix epoch
            }
            else if (input.StartsWith("relocdir"))
            {
                Console.Write("\n" + current_directory + "[relocdir:path] > ");
                string sourceDirectory = "NULL";
                sourceDirectory = Console.ReadLine();
                Console.Write("\n" + current_directory + "[relocdir:destination] > ");
                string destinationDirectory = "NULL";
                destinationDirectory = Console.ReadLine();
                try
                {
                    Directory.Move(sourceDirectory, destinationDirectory);  // moves directory
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nEXCEPTION: " + e.Message);
                }
            }
            else if (input.StartsWith("help"))
            {
                string pfile = (@"0:\\home\manual.txt");
                if (File.Exists(pfile))
                {
                    Console.WriteLine(File.ReadAllText(pfile));  // reads manual
                }
                else
                {
                    Console.WriteLine("\nEXCEPTION: File doesn't exist...");
                }
            }
            else if (input.StartsWith("relocate"))
            {
                string relpath = @"0:\\home";
                string relpath2 = @"0:\\home";
                Console.WriteLine("\n" + current_directory + "[relocate:path] > ");
                relpath = Console.ReadLine();
                Console.WriteLine("\n" + current_directory + "[relocate:destination] > ");
                relpath2 = Console.ReadLine();
                try
                {
                    // moves file
                    if (!File.Exists(relpath))
                    {
                        using (FileStream fs = File.Create(relpath)) {}
                    }
                    if (File.Exists(relpath))	
                    File.Delete(relpath2);
                    File.Move(relpath, relpath2);
                    if (File.Exists(relpath))
                    {
                        Console.WriteLine("\nEXCEPTION: Original file could not be deleted...");
                    }		
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nEXCEPTION: {0}...", e.ToString());
                }
            }
            else if (input.StartsWith("rename"))
            {
                Console.WriteLine("\n" + current_directory + "[rename:file] > ");
                ren1 = Console.ReadLine();
                Console.WriteLine("\n" + current_directory + "[rename:destination] > ");
                ren2 = Console.ReadLine();
                try
                {
                    System.IO.File.Move(ren1, ren2);  // renames file
                }
                catch (Exception e)
                {
                    Console.WriteLine("\nEXCEPTION: Could not rename file...");
                }
            }
            if (input.StartsWith("caracal"))
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("                                              888 ");
                Console.WriteLine("                                              888 ");
                Console.WriteLine("                                              888 ");
                Console.WriteLine(".d8888b 8888b. 888d888 8888b.  .d8888b 8888b. 888 ");
                Console.WriteLine("d88P\"       \"88b888P\"      \"88bd88P\"      \"88b888 ");
                Console.WriteLine("888     .d888888888    .d888888888    .d888888888 ");
                Console.WriteLine("Y88b.   888  888888    888  888Y88b.  888  888888 ");
                Console.WriteLine("\"Y8888P\"Y888888888    \"Y888888 \"Y8888P\"Y888888888 \n");
                Console.WriteLine("Settings manager for kuNIX.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n_");
                string caracalcommand = "NULL";
                caracalcommand = Console.ReadLine();

                if (caracalcommand.StartsWith("hostname")) {
                    Console.WriteLine("\n> ");
                    string hostnameconfig = "NULL";
                    hostnameconfig = Console.ReadLine();
                    file = File.Create(@"0:\\kunix\host.txt");
                    contents = byte.Parse(hostnameconfig);
                    file.WriteByte(contents);
                    file.Close();
                    ComputerName = File.ReadAllText(@"0:\\kunix\host.txt");
                } else {
                    Console.WriteLine("\nClosing Caracal...");
                }
            }
            if (input.StartsWith("./"))
            {
                var execute = input.Remove(0, 2);
                if (execute) {
                    Process p = new Process();
                    p.StartInfo.FileName = execute;
                    p.StartInfo.UseShellExecute = false;
                    p.Start();
                } else {
                    Console.WriteLine("\nEXCEPTION: No executable specified.");
                }
            }
            else
            {
                Console.WriteLine("\nEXCEPTION: Invalid command.");  // if the input matches none of these commands, it will throw this exception
            }
        }
    }
}