using Newtonsoft.Json;

namespace GlobalIPDotnet
{
  /// <summary>
  /// Global IP looks up an IP address and returns location and network details
  /// for it, such as city, region, country, postal code, latitude/longitude, time
  /// zone, ISP and domain name, connection type, and proxy information.
  ///
  /// <para>High-level flow of this sample:</para>
  /// <list type="number">
  ///   <item><description>ARGS    - ParseArguments reads any --flag values off the command line.</description></item>
  ///   <item><description>INPUT   - CallAPI fills in whatever wasn't supplied via interactive prompts.</description></item>
  ///   <item><description>REQUEST - CallAPI builds the REST query string (license + IP address).</description></item>
  ///   <item><description>CALL    - GetContents issues the GET request and pretty-prints the JSON response.</description></item>
  /// </list>
  ///
  /// <para>This sample is a thin HTTP client: it builds a query string, sends a GET
  /// request to the Global IP Cloud API, and prints the JSON response.</para>
  ///
  /// <para>Reference:</para>
  /// <list type="bullet">
  ///   <item><description>Documentation: https://docs.melissa.com/cloud-api/global-ip/global-ip-index.html</description></item>
  ///   <item><description>Release notes: https://releasenotes.melissa.com/cloud-api/global-ip/</description></item>
  ///   <item><description>Result codes: https://docs.melissa.com/melissa/result-codes/result-codes-index.html</description></item>
  /// </list>
  /// </summary>
  static class Program
  {
    /// <summary>
    /// Entry point. Reads the optional command-line arguments, then hands control to
    /// CallAPI, which performs the actual request/response cycle.
    /// </summary>
    /// <param name="args">The raw command-line arguments.</param>
    static void Main(string[] args)
    {
      string baseServiceUrl = @"https://globalip.melissadata.net/";
      string serviceEndpoint = @"v4/web/iplocation/doiplocation"; //please see https://www.melissa.com/developer/ip-locator for more endpoints
      string license = "";
      string ip = "";

      // Populate any values passed on the command line, then run the lookup.
      ParseArguments(ref license, ref ip, args);
      CallAPI(baseServiceUrl, serviceEndpoint, license, ip);
    }

    /// <summary>
    /// Reads the supported command-line options and writes each recognized value into
    /// its matching by-ref parameter. Any parameter left unset here falls back to an
    /// interactive prompt later in <see cref="CallAPI"/>.
    ///
    /// <para>Recognized flags (each followed by its value, e.g. "--ip 12.203.219.6"):
    /// --license/-l, --ip.</para>
    /// </summary>
    /// <param name="license">Receives the Melissa license string, if supplied.</param>
    /// <param name="ip">Receives the IP address to look up, if supplied.</param>
    /// <param name="args">The raw command-line arguments to parse.</param>
    static void ParseArguments(ref string license, ref string ip, string[] args)
    {
      for (int i = 0; i < args.Length; i++)
      {
        if (args[i].Equals("--license") || args[i].Equals("-l"))
        {
          if (args[i + 1] != null)
          {
            license = args[i + 1];
          }
        }
        if (args[i].Equals("--ip"))
        {
          if (args[i + 1] != null)
          {
            ip = args[i + 1];
          }
        }
      }
    }

    /// <summary>
    /// Issues the GET request against the Global IP endpoint and
    /// pretty-prints the API call and the JSON response to the console.
    /// </summary>
    /// <param name="baseServiceUrl">The Global IP Cloud API base URL.</param>
    /// <param name="requestQuery">The endpoint path plus query string built by <see cref="CallAPI"/>.</param>
    public static async Task GetContents(string baseServiceUrl, string requestQuery)
    {
      HttpClient client = new HttpClient();
      client.BaseAddress = new Uri(baseServiceUrl);
      HttpResponseMessage response = await client.GetAsync(requestQuery);

      string text = await response.Content.ReadAsStringAsync();

      // Re-serialize with indentation so the raw response is easier to read.
      var obj = JsonConvert.DeserializeObject(text);
      var prettyResponse = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);

      // Print output
      Console.WriteLine("\n=============================== OUTPUT ===============================\n");

      Console.WriteLine("API Call: ");
      string APICall = Path.Combine(baseServiceUrl, requestQuery);
      for (int i = 0; i < APICall.Length; i += 70)
      {
        if (i + 70 < APICall.Length)
        {
          Console.WriteLine(APICall.Substring(i, 70));
        }
        else
        {
          Console.WriteLine(APICall.Substring(i, APICall.Length - i));
        }
      }

      Console.WriteLine("\nAPI Response:");
      Console.WriteLine(prettyResponse);
    }

    /// <summary>
    /// Drives the interactive/CLI loop: gathers the required IP field, builds and
    /// submits the REST query, prints the result, and optionally repeats for another record.
    ///
    /// <para>In interactive mode (no IP argument supplied) it loops, asking for a new record each pass
    /// until the user answers "N". In one-shot mode (IP argument supplied) it runs a single
    /// pass and exits.</para>
    /// </summary>
    /// <param name="baseServiceUrl">The Global IP Cloud API base URL.</param>
    /// <param name="serviceEndPoint">The specific Global IP endpoint path to call.</param>
    /// <param name="license">The Melissa license string sent with every request.</param>
    /// <param name="ip">An IP address to look up in one-shot mode; if empty, the program prompts interactively.</param>
    static void CallAPI(string baseServiceUrl, string serviceEndPoint, string license, string ip)
    {
      Console.WriteLine("\n=============== WELCOME TO MELISSA GLOBAL IP CLOUD API ===============\n");
      
      bool shouldContinueRunning = true;

      while (shouldContinueRunning)
      {
        string inputIp = "";

        // No IP was supplied via command line, so prompt for it.
        if (string.IsNullOrEmpty(ip))
        {
          Console.WriteLine("\nFill in each value to see results");
          Console.Write("IP: ");
          inputIp = Console.ReadLine();
        }
        else
        {
          // An IP was supplied via command line; use it as-is.
          inputIp = ip;
        }

        // Keep prompting until a non-empty IP is entered.
        while (string.IsNullOrEmpty(inputIp))
        {
          Console.WriteLine("\nFill in missing required parameters");

          Console.Write("IP: ");
          inputIp = Console.ReadLine();
        }

        // Map the input field to the API's expected query parameter name.
        // (No format parameter is sent; the response is still parsed as JSON.)
        Dictionary<string, string> inputs = new Dictionary<string, string>()
        {
            { "ip", inputIp}
        };

        Console.WriteLine("\n=============================== INPUTS ===============================\n");
        Console.WriteLine($"\t   Base Service Url: {baseServiceUrl}");
        Console.WriteLine($"\t  Service End Point: {serviceEndPoint}");
        Console.WriteLine($"\t                 IP: {inputIp}");

        // Create Service Call
        // Set the License String in the Request
        string RESTRequest = "";

        RESTRequest += @"&id=" + Uri.EscapeDataString(license);

        // Set the Input Parameters
        foreach (KeyValuePair<string, string> kvp in inputs)
          RESTRequest += @"&" + kvp.Key + "=" + Uri.EscapeDataString(kvp.Value);

        // Build the final REST String Query
        RESTRequest = serviceEndPoint + @"?" + RESTRequest;

        // Submit to the Web Service. 
        bool success = false;
        int retryCounter = 0;

        do
        {
          try //retry just in case of network failure
          {
            GetContents(baseServiceUrl, $"{RESTRequest}").Wait();
            Console.WriteLine();
            success = true;
          }
          catch (Exception ex)
          {
            retryCounter++;
            Console.WriteLine(ex.ToString());
            return;
          }
        } while ((success != true) && (retryCounter < 5));

        // If the IP came from the command line, treat this as a one-shot
        // run rather than looping for additional records.
        bool isValid = false;
        if (!string.IsNullOrEmpty(ip))
        {
          isValid = true;
          shouldContinueRunning = false;
        }

        // Otherwise ask whether to test another record. Keep prompting until we get a
        // valid Y/N. "N" ends the program; "Y" falls through to another pass.
        while (!isValid)
        {
          Console.WriteLine("\nTest another record? (Y/N)");
          string testAnotherResponse = Console.ReadLine();

          if (!string.IsNullOrEmpty(testAnotherResponse))
          {
            testAnotherResponse = testAnotherResponse.ToLower();
            if (testAnotherResponse == "y")
            {
              isValid = true;
            }
            else if (testAnotherResponse == "n")
            {
              isValid = true;
              shouldContinueRunning = false;
            }
            else
            {
              Console.Write("Invalid Response, please respond 'Y' or 'N'");
            }
          }
        }
      }
      
      Console.WriteLine("\n================ THANK YOU FOR USING MELISSA CLOUD API ===============\n");
    }
  }
}
