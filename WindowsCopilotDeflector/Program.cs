using System.Collections.Specialized;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Web;

// The following code is taken from the EdgeDeflector repo
static bool IsUri(string uristring)
{
    try
    {
        Uri uri = new Uri(uristring);
        return true;
    }
    catch (UriFormatException)
    {
    }
    catch (ArgumentNullException)
    {
    }
    return false;
}

static bool IsHttpUri(string uri)
{
    return uri.StartsWith("HTTPS://", StringComparison.OrdinalIgnoreCase) || uri.StartsWith("HTTP://", StringComparison.OrdinalIgnoreCase);
}

static bool IsMsEdgeUri(string uri)
{
    return uri.StartsWith("MICROSOFT-EDGE:", StringComparison.OrdinalIgnoreCase) && !uri.Contains(" ");
}

static bool IsNonAuthoritativeWithUrlQueryParameter(string uri)
{
    return uri.Contains("microsoft-edge:?") && uri.Contains("&url=");
}

static string GetURIFromCortanaLink(string uri)
{
    NameValueCollection queryCollection = HttpUtility.ParseQueryString(uri);
    return queryCollection["url"];
}

static string RewriteMsEdgeUriSchema(string uri)
{
    string msedge_protocol_pattern = "^microsoft-edge:/*";

    Regex rgx = new Regex(msedge_protocol_pattern);
    string new_uri = rgx.Replace(uri, string.Empty);

    if (IsHttpUri(new_uri))
    {
        return new_uri;
    }

    // May be new-style Cortana URI - try and split out
    if (IsNonAuthoritativeWithUrlQueryParameter(uri))
    {
        string cortanaUri = GetURIFromCortanaLink(uri);
        if (IsHttpUri(cortanaUri))
        {
            // Correctly form the new URI before returning
            return cortanaUri;
        }
    }

    // defer fallback to web browser
    return "http://" + new_uri;
}

static void OpenUri(string uri)
{
    if (!IsUri(uri) || !IsHttpUri(uri))
    {
        Environment.Exit(1);
    }

    ProcessStartInfo launcher = new ProcessStartInfo(uri)
    {
        UseShellExecute = true
    };
    Process.Start(launcher);
}

// The code following is by me :)

Console.WriteLine("[*] The process has been launched with the following arguments: ");
foreach (string arg in args)
{
    Console.WriteLine("[+] Argument: " + arg);
}

// Find if the program has been launched as a protocol

bool bAsProtocol = false;

foreach (string arg in args)
{
    if (arg.StartsWith("microsoft-edge://"))
    {
        Console.WriteLine("[*] We have been launched as a protocol");
        bAsProtocol = true;
    }
}


if (!bAsProtocol)
{
    Console.WriteLine("[+] The microsoft edge launch was \"intentional\". Launching original edge...");
    ProcessStartInfo psi = new ProcessStartInfo();
    psi.FileName = "original-msedge.exe";
    psi.Arguments = String.Join(" ", args);
    Process.Start(psi);
//    Environment.Exit(0);
}

foreach(string arg in args)
{
    Console.WriteLine("[*] Processing argument " + arg);
    Console.WriteLine("[*] IsMsEdgeUri(arg): " + IsMsEdgeUri(arg));
    if (IsMsEdgeUri(arg))
    {
        Console.WriteLine("[+] RewriteMsEdgeUriSchema(arg): " + RewriteMsEdgeUriSchema(arg));
        OpenUri(RewriteMsEdgeUriSchema(arg));
//        Environment.Exit(0);
    }
}

Console.ReadKey();
