using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Sys = Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.HAL.Drivers.PCI.Video;
using Cosmos.HAL;
using Cosmos.System.FileSystem;
using Cosmos.Debug.Kernel;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Threading.Tasks;
using Cosmos.Core.IOGroup;
using Cosmos.System.ExtendedASCII;
using System.Security.Cryptography;

namespace kuNIX {
  public class Kernel: Sys.Kernel {

      string current_directory = (@"0:\\");
      public static string ComputerName = "kuNIX";
      public static float cpuUsage = 0;

      Canvas canvas;

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
        FS = new Sys.FileSystem.CosmosVFS();
        Sys.FileSystem.VFS.VFSManager.RegisterVFS(FS);
        FS.Initialize();
        Console.WriteLine("[OK]       Scanning Filesystems...");
        Console.Clear();
        Console.WriteLine("[OK]       Checking for kuNIX.....");
        if (Directory.Exists(@"0:\\kunix")) {
          bool installed = true;
        } else {
          Console.WriteLine("[FAILED]   kuNIX not installed....");
          Console.WriteLine("[OK]       Installing.............");
          Console.WriteLine("[installer:hostname] > ");
          Directory.CreateDirectory(@"0:\\kunix");
          File.Create(@"0:\\kunix\host.txt");
          File.Create(@"0:\\kunix\bash.txt");
          File.Create(@"0:\\home\help1.txt");
          File.Create(@"0:\\home\help2.txt");
          string hostname = "kuNIX";
          hostname = Console.ReadLine();
          Console.WriteLine("\n");
          Console.WriteLine("[OK]       Writing host.txt.......");
          File.WriteAllText(@"0:\\kunix\host.txt", hostname);
          File.WriteAllText(@"0:\\home\help1.txt", "--- BEFORE WE GET STARTED ---\n\n\nWith all this freedom, you sure can do a lot to your computer.\nkuNIX provides no limitations, but, with that said, you probably\ndo not want to delete the /kunix/ directory. And you DEFINITELY\ndo not want to delete the /home/ directory. But, there are no\nlimitations, so you can do it if you want, but we warned you.");
          File.WriteAllText(@"0:\\home\help2.txt", "\n\n\n--- COMMANDS ---\n\n\nGeneral Commands:\n\necho - prints text\nbeep - plays a beep sound\nbday - wishes you a happy birthday\nclear - clears the console\ncalc - does math\ntime - displays time\nepoch - displays seconds since unix epoch\n\n\nFile Commands:\n\nlscon - lists files in current directory\nxpl - explores through directories\nread - prints a file's contents\nlsvol - lists volumes\nunlink - deletes file\nuldir - unlinks directory\nmkdir - creates directory\nbuild - creates file\nsto - writes to file\nappend - appends new line to file\nredir - renames/relocates directory\n\n\nSystem Commands:\n\nshutdown - shuts down computer\nreboot - reboots computer\nsys - shows system info\nhostname - allows you to change your hostname");
          File.WriteAllText(@"0:\\kunix\bash.txt", "Welcome to kuNIX.\nMade by Scott and YoPoster.\nPlease check out http://kunix.rf.gd for documentation.\nUse 'help' to display the help menu.");
          Console.WriteLine("[OK]       Creating home..........");
          Directory.CreateDirectory(@"0:\\home\");
            Console.Clear();
          }
          Console.WriteLine("[OK]       Setting up kuNIX.......");
          current_directory = (@"0:\\home\");
            ComputerName = File.ReadAllText(@"0:\\kunix\host.txt"); string bash = File.ReadAllText(@"0:\\kunix\bash.txt"); Console.Clear(); Console.WriteLine(bash);
          }

          public Cosmos.System.FileSystem.CosmosVFS FS = null;
          protected override void Run() {
            Console.Write("[" + current_directory + " @ " + ComputerName + "] > ");
            string input = Console.ReadLine();
            interpret(input);
          }

          public void interpret(string input) {
            if (input.StartsWith("echo ")) {
              try {
                Console.WriteLine(input.Remove(0, 5));
              } catch (Exception ex) {
                Console.WriteLine("EXCEPTION: " + ex.Message);
              }
            } else if (input.StartsWith("help1")) {
              string readfile = (@"0:\\home\help1.txt");
              if (File.Exists(readfile)) {
                Console.WriteLine(File.ReadAllText(readfile));
              } else {
                Console.WriteLine("EXCEPTION: File doesn't exist.");
              }
            } else if (input.StartsWith("help2")) {
              string readfile = (@"0:\\home\help2.txt");
              if (File.Exists(readfile)) {
                Console.WriteLine(File.ReadAllText(readfile));
              } else {
                Console.WriteLine("EXCEPTION: File doesn't exist.");
              }
            } else if (input.StartsWith("help")) {
              Console.WriteLine("To display helpful notes, use 'help1'.");
              Console.WriteLine("To display commands, use 'help2'.");
            } else if (input.StartsWith("shutdown")) {
              Console.WriteLine("Thank you for choosing kuNIX.");
              Console.WriteLine("[OK]       Shutting down..........");
              Sys.Power.Shutdown();
            } else if (input.StartsWith("reboot")) {
              Console.WriteLine("Thank you for choosing kuNIX.");
              Console.WriteLine("[OK]       Rebooting..............");
              Sys.Power.Reboot();
            } else if (input.StartsWith("clear")) {
              Console.Clear();
            } else if (input.StartsWith("bday")) {
              Console.WriteLine("If you are unable to hear any sounds, that could be due to your PC not having built in speakers...");
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
              Console.WriteLine();
              Console.WriteLine("       iiiiiiiiii");
              Console.WriteLine("      |:H:a:p:p:y:|");
              Console.WriteLine("    __|___________|__");
              Console.WriteLine("   |^^^^^^^^^^^^^^^^^|");
              Console.WriteLine("   |:B:i:r:t:h:d:a:y:|");
              Console.WriteLine("   |                 |");
              Console.WriteLine("   ~~~~~~~~~~~~~~~~~~~");
            } else if (input.StartsWith("beep")) {
              Console.Beep();
            } else if (input.StartsWith("lscon")) {
              try {
                Console.WriteLine("Type\tSize\tName");
                foreach(var dir in Directory.GetDirectories(current_directory)) {
                  Console.WriteLine("<DIR>\t-\t" + dir);
                }
                foreach(var dir in Directory.GetFiles(current_directory)) {
                  FileInfo finf = new FileInfo(dir);
                  Console.WriteLine(finf.Extension + "\t" + finf.Length + "\t" + dir);
                }
              } catch (Exception e) {
                Console.WriteLine("EXCEPTION: Could not list...");
              }
            } else if (input.StartsWith("xpl ")) {
              var newdir = input.Remove(0, 4);
              if (FS.GetDirectory(current_directory + newdir) != null) {
                current_directory = current_directory + newdir + @"\";
              } else {
                if (newdir == "..") {
                  var dir = FS.GetDirectory(current_directory);
                  string p = dir.mParent.mName;
                  if (!string.IsNullOrEmpty(p)) {
                    current_directory = (@"0:\\" + p);
                  }
                }
              }
            } else if (input.StartsWith("read ")) {
              string readfile = input.Remove(0, 5);
              if (File.Exists(readfile)) {
                Console.WriteLine(File.ReadAllText(readfile));
              } else {
                Console.WriteLine("EXCEPTION: File doesn't exist.");
              }
            } else if (input.StartsWith("lsvol")) {
              var vols = FS.GetVolumes();
              Console.WriteLine("\nName\tSize\tParent");
              foreach(var vol in vols) {
                Console.WriteLine(vol.mName + "\t" + vol.mSize + "\t" + vol.mParent); // lists volumes
              }
            } else if (input.StartsWith("calc")) {
              double a, b;
              char c;
              Console.Write("[calc:operand1] > ");
              a = Double.Parse(Console.ReadLine());
              Console.Write("[calc:operand2] > ");
              b = Double.Parse(Console.ReadLine());
              Console.Write("[calc:operator] > ");
              c = Char.Parse(Console.ReadLine());
              switch (c) {
              case '+':
                Console.WriteLine("{0}+{1}={2}", a, b, a + b);
                break;
              case '-':
                Console.WriteLine("{0}-{1}={2}", a, b, a - b);
                break;
              case '*':
                Console.WriteLine("{0}*{1}={2}", a, b, a * b);
                break;
              case '/':
                Console.WriteLine("{0}/{1}={2}", a, b, a / b);
                break;
              default:
                Console.WriteLine("EXCEPTION: Unknown operator.");
                break;
              }
            } else if (input.StartsWith("uldir ")) {
              string subPath = input.Remove(0, 6);
              if (input.Contains("-f ")) {
                Directory.Delete(subPath);
              } else {
                try {
                  Directory.Delete(subPath);
                  bool subDirectoryExists = Directory.Exists(subPath);
                } catch (Exception e) {
                  Console.WriteLine("EXCEPTION: {0}.", e.Message);
                }
              }
            } else if (input.StartsWith("unlink ")) {
              string delPath = input.Remove(0, 7);
              try {
                File.Delete(delPath);
              } catch (Exception e) {
                Console.WriteLine("EXCEPTION: {0}.", e.Message);
              }
            } else if (input.StartsWith("time")) {
              DateTime current = DateTime.Now;
              Console.WriteLine($"{current}");
            } else if (input.StartsWith("epoch")) {
              TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
              int secondsSinceEpoch = (int) t.TotalSeconds;
              Console.WriteLine("\n" + secondsSinceEpoch);
            } else if (input.StartsWith("sys")) {
              Console.WriteLine("RAM: " + Cosmos.Core.CPU.GetAmountOfRAM().ToString());
              Console.WriteLine("Free Space: " + Sys.FileSystem.VFS.VFSManager.GetTotalFreeSpace("0") + " bytes");
              Console.WriteLine("Root Drive Size: " + Sys.FileSystem.VFS.VFSManager.GetTotalSize("0") + " bytes");
            } else if (input.StartsWith("mkdir ")) {
              string dir = input.Remove(0, 6);
              Directory.CreateDirectory(current_directory + dir);
            } else if (input.StartsWith("build ")) {
              try {
                string touchp = input.Remove(0, 6);
                File.Create(current_directory + touchp);
              } catch (Exception e) {
                Console.WriteLine("EXCEPTION: Could not build file...");
              }
            } else if (input.StartsWith("sto")) {
              Console.Write("[sto:file] > ");
              string writefile = Console.ReadLine();
              Console.Write("[sto:content] > ");
              string writecontent = Console.ReadLine();
              if (File.Exists(writefile)) {
                File.WriteAllText(writefile, writecontent);
              }
            } else if (input.StartsWith("append")) {
              Console.Write("[append:file] > ");
              string writefile = Console.ReadLine();
              string appendcontent = File.ReadAllText(writefile);
              Console.Write("[append:content] > ");
              string writecontent = Console.ReadLine();
              string contents = (appendcontent + "\n" + writecontent);
              if (File.Exists(writefile)) {
                File.WriteAllText(writefile, contents);
              }
            } else if (input.StartsWith("hostname")) {
              Console.Write("[hostname:content] > ");
              string writecontent = Console.ReadLine();
              string writefile = (@"0:\\kunix\host.txt");
              if (File.Exists(writefile)) {
                File.WriteAllText(writefile, writecontent);
              }
              ComputerName = File.ReadAllText(@"0:\\kunix\host.txt");
            } else if (input.StartsWith("redir")) {
              Console.Write("[redir:path1] > ");
              string sourceDirectory = Console.ReadLine();
              Console.Write("[redir:path2] > ");
              string destinationDirectory = Console.ReadLine();
              try {
                Directory.Move(sourceDirectory, destinationDirectory);
              } catch (Exception e) {
                Console.WriteLine(e.Message);
              }
            } else if (input.StartsWith("matrix")) {
              Console.ForegroundColor = ConsoleColor.Green;
              Console.BackgroundColor = ConsoleColor.Black;
              Console.Clear();
            } else if (input.StartsWith("ice")) {
              Console.ForegroundColor = ConsoleColor.White;
              Console.BackgroundColor = ConsoleColor.Blue;
              Console.Clear();
            } else {
              Console.WriteLine("EXCEPTION: Invalid command...");
            }
          }
        }
      }
