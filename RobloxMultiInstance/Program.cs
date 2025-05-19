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
            try
            {
                Console.Title = "Roblox Multi Instance Enabler";
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                var robloxProcesses = Process.GetProcessesByName("RobloxPlayerBeta");
                if (robloxProcesses.Length > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[WARNING] Roblox instance detected.");
                    Console.WriteLine("A Roblox instance is already running.");
                    Console.Write("Do you want to close all instances? (Y/N): ");
                    Console.ResetColor();

                    string? input = Console.ReadLine()?.Trim().ToLower();

                    if (input == "y")
                    {
                        foreach (var proc in robloxProcesses)
                        {
                            try { proc.Kill(); } catch { }
                        }

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[INFO] All Roblox instances terminated.");
                        Console.ResetColor();

                        Thread.Sleep(2000);
                        Console.Clear();
                    }
                    else
                    {
                        Console.WriteLine("Operation cancelled. Exiting...");
                        return;
                    }
                }

                Console.WriteLine("\r\n  __  __       _ _   _   _____           _                       \r\n |  \\/  |     | | | (_) |_   _|         | |                      \r\n | \\  / |_   _| | |_ _    | |  _ __  ___| |_ __ _ _ __   ___ ___ \r\n | |\\/| | | | | | __| |   | | | '_ \\/ __| __/ _` | '_ \\ / __/ _ \\\r\n | |  | | |_| | | |_| |  _| |_| | | \\__ \\ || (_| | | | | (_|  __/\r\n |_|  |_|\\__,_|_|\\__|_| |_____|_| |_|___/\\__\\__,_|_| |_|\\___\\___|\r\n                                                                 \r\n                                                                 \r\n");
                Console.WriteLine("Multi Instance + 773 error code fix");
                Console.WriteLine("Made by evanovar :)\n");

                // multi instance
                robloxMutex = new Mutex(true, "ROBLOX_singletonEvent", out bool createdNew);

                if (createdNew)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[INFO] Multi-instance mode enabled.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[WARNING] Mutex already exists. Another instance might be running.");
                }
                Console.ResetColor();

                // 773 fix woah
                string cookiesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Roblox\LocalStorage\RobloxCookies.dat");

                if (File.Exists(cookiesPath))
                {
                    try
                    {
                        cookiesLockStream = new FileStream(cookiesPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[INFO] Error 773 fix applied.");
                    }
                    catch (IOException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[ERROR] Could not lock RobloxCookies.dat. It may already be locked.");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ERROR] Cookies file not found. 773 fix skipped.");
                }

                Console.ResetColor();
                Console.WriteLine("\nKeep this window open while launching Roblox instances.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERROR] Unexpected error: " + ex.Message);
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
