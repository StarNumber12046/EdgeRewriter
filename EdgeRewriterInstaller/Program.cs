using System;
using System.Diagnostics;
using System.Security.Principal;

static void OpenUri(string uri)
{
    ProcessStartInfo launcher = new ProcessStartInfo(uri)
    {
        UseShellExecute = true
    };
    Process.Start(launcher);
}

static bool IsElevated()
{
            return WindowsIdentity.GetCurrent().Owner
          .IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);
}

if (!IsElevated())
{
    Console.WriteLine("[+] Not running as administrator. Cannot proceed.");
    Environment.Exit(1);
}

string cRewriterPath = "EdgeRewriter.exe";
if (!File.Exists("EdgeRewriter.exe"))
{
    Console.WriteLine("[*] Rewriter binary is missing. Please type in the full path.");
    cRewriterPath = Console.ReadLine();
    if (!File.Exists(cRewriterPath))
    {
        Console.WriteLine("[!] Path does not exist. Exiting...");
        Environment.Exit(1);
    }
}

if (File.Exists("C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\original-msedge.exe"))
{
    Console.WriteLine("[*] Found existing original-msedge.exe. Did you already install this before?");
    ProcessStartInfo launcher = new ProcessStartInfo("taskkill")
    {
        Arguments = "/f /im original-msedge.exe",
        UseShellExecute = true
    };
    Process.Start(launcher);
    File.Delete("C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe");
    File.Move("C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\original-msedge.exe", "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe");
    Console.WriteLine("[*] File deleted.");
}

File.Move("C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe", "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\original-msedge.exe");
Console.WriteLine("[+] Successfully renamed msedge file");

File.Copy(cRewriterPath, "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe");
Console.WriteLine("[+] Successfully installed EdgeRewriter.");
Console.WriteLine("[+] Press any key to test the installation");
Console.ReadKey();
OpenUri("microsoft-edge://https://www.daniel.priv.no/tools/edgedeflector/post-install.html");
