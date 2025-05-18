using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace RobloxMultiInstance773Fix
{
    internal class Program
    {
        static Mutex? robloxMutex = null;
        static FileStream? cookiesLockStream = null;

        static void Main(string[] args)
        {
            Console.Title = "Roblox Multi Instance Enabler";
            Console.WriteLine("\r\n  __  __       _ _   _   _____           _                       \r\n |  \\/  |     | | | (_) |_   _|         | |                      \r\n | \\  / |_   _| | |_ _    | |  _ __  ___| |_ __ _ _ __   ___ ___ \r\n | |\\/| | | | | | __| |   | | | '_ \\/ __| __/ _` | '_ \\ / __/ _ \\\r\n | |  | | |_| | | |_| |  _| |_| | | \\__ \\ || (_| | | | | (_|  __/\r\n |_|  |_|\\__,_|_|\\__|_| |_____|_| |_|___/\\__\\__,_|_| |_|\\___\\___|\r\n                                                                 \r\n                                                                 \r\n");
            Console.WriteLine("Multi Instance with TeleportService support");
            Console.WriteLine("Made by evanovar :)\n");

            try
            {
                robloxMutex = new Mutex(true, "ROBLOX_singletonEvent", out bool createdNew);

                if (createdNew)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[INFO] Multi-instance mode enabled.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ERROR] Mutex already exists. Another instance might be running.");
                }
                Console.ResetColor();

                string cookiesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Roblox\LocalStorage\RobloxCookies.dat");
                string noFixFile = Path.Combine(Environment.CurrentDirectory, "no773fix.txt");

                bool apply773Fix = File.Exists(cookiesPath) && !File.Exists(noFixFile);

                if (!apply773Fix)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[ERROR] Not applying 773 fix: Cookies file exists = {File.Exists(cookiesPath)}, 'no773fix.txt' exists = {File.Exists(noFixFile)}");
                    Console.ResetColor();
                }
                else
                {
                    try
                    {
                        cookiesLockStream = new FileStream(cookiesPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[INFO] Error 773 has been fixed successfully.");
                        Console.ResetColor();
                    }
                    catch (IOException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[ERROR] Could not lock RobloxCookies.dat (file might be in use by another instance).");
                        Console.ResetColor();
                    }
                }

                Console.WriteLine("\nKeep this window open while launching Roblox instances.");
                Console.WriteLine("Press any key to exit and disable multi instance...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Unexpected error: " + ex.Message);
                Console.ResetColor();
            }
            finally
            {
                robloxMutex?.ReleaseMutex();
                robloxMutex?.Dispose();
                cookiesLockStream?.Close();
                cookiesLockStream?.Dispose();
            }
        }
    }
}
