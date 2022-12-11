using AndreasReitberger.API.OctoPrint;
using AndreasReitberger.API.OctoPrint.Models;
using AndreasReitberger.Core.Utilities;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace OctoPrintSharpApi.NUnitTest
{
    public class Tests
    {
        // https://docs.microsoft.com/en-us/dotnet/core/tutorials/testing-library-with-visual-studio

        private static string _api = "A1FE2CC8FCCE4BD59910D8865ABBDAE5";
        private static string _host = "192.168.10.44";
        private static int _port = 80;
        private static bool _ssl = false;

        private bool _skipWebSocketTests = true;
        private bool _skipOnlineTests = false;
        private bool _skipPrinterActionTests = true;

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SerializeJsonTest()
        {
            var dir = @"TestResults\Serialization\";
            Directory.CreateDirectory(dir);
            string serverConfig = Path.Combine(dir, "server.xml");
            if (File.Exists(serverConfig)) File.Delete(serverConfig);
            try
            {

                OctoPrintClient.Instance = new OctoPrintClient(_host, _api, _port, _ssl)
                {
                    FreeDiskSpace = 1523165212,
                    TotalDiskSpace = 65621361616161,
                    ServerName = "My OctoPrint Server"
                };
                OctoPrintClient.Instance.SetProxy(true, "https://testproxy.de", 447, "User", SecureStringHelper.ConvertToSecureString("my_awesome_pwd"), true);

                var serializedString = System.Text.Json.JsonSerializer.Serialize(OctoPrintClient.Instance);
                var serializedObject = System.Text.Json.JsonSerializer.Deserialize<OctoPrintClient>(serializedString);
                Assert.IsTrue(serializedObject is OctoPrintClient server && server != null);

            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public void SerializeJsonNewtonSoftTest()
        {
            var dir = @"TestResults\Serialization\";
            Directory.CreateDirectory(dir);
            string serverConfig = Path.Combine(dir, "server.xml");
            if (File.Exists(serverConfig)) File.Delete(serverConfig);
            try
            {

                OctoPrintClient.Instance = new OctoPrintClient(_host, _api, _port, _ssl)
                {
                    FreeDiskSpace = 1523165212,
                    TotalDiskSpace = 65621361616161,
                    ServerName = "My OctoPrint Server"
                };
                OctoPrintClient.Instance.SetProxy(true, "https://testproxy.de", 447, "User", SecureStringHelper.ConvertToSecureString("my_awesome_pwd"), true);

                var serializedString = JsonConvert.SerializeObject(OctoPrintClient.Instance);
                var serializedObject = JsonConvert.DeserializeObject<OctoPrintClient>(serializedString);
                Assert.IsTrue(serializedObject is OctoPrintClient server && server != null);

            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public void SerializeXmlTest()
        {

            var dir = @"TestResults\Serialization\";
            Directory.CreateDirectory(dir);
            string serverConfig = Path.Combine(dir, "server.xml");
            if (File.Exists(serverConfig)) File.Delete(serverConfig);
            try
            {
                var xmlSerializer = new XmlSerializer(typeof(OctoPrintClient));
                using (var fileStream = new FileStream(serverConfig, FileMode.Create))
                {
                    OctoPrintClient.Instance = new OctoPrintClient(_host, _api, _port, _ssl)
                    {
                        FreeDiskSpace = 1523165212,
                        TotalDiskSpace = 65621361616161,
                        ServerName = "My OctoPrint Server"
                    };
                    OctoPrintClient.Instance.SetProxy(true, "https://testproxy.de", 447, "User", SecureStringHelper.ConvertToSecureString("my_awesome_pwd"), true);

                    xmlSerializer.Serialize(fileStream, OctoPrintClient.Instance);
                    Assert.IsTrue(File.Exists(Path.Combine(dir, "server.xml")));
                }

                xmlSerializer = new XmlSerializer(typeof(OctoPrintClient));
                using (var fileStream = new FileStream(serverConfig, FileMode.Open))
                {
                    var instance = (OctoPrintClient)xmlSerializer.Deserialize(fileStream);
                }

            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task ServerInitTest()
        {
            try
            {
                OctoPrintClient _server = new OctoPrintClient(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    if (_server.ActivePrinter == null)
                        await _server.SetPrinterActiveAsync(0, true);

                    await _server.RefreshAllAsync();
                    Assert.IsTrue(_server.InitialDataFetched);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task FetchPrinterProfilesTest()
        {
            try
            {
                OctoPrintClient _server = new OctoPrintClient(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    if (_server.ActivePrinter == null)
                        await _server.SetPrinterActiveAsync(0, true);

                    ObservableCollection<OctoPrintPrinter> printers = await _server.GetAllPrinterProfilesAsync();
                    Assert.IsTrue(printers != null && printers.Count > 0);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task FetchPrintModelsTest()
        {
            try
            {
                OctoPrintClient _server = new OctoPrintClient(_host, _api, _port, _ssl);
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    if (_server.ActivePrinter == null)
                        await _server.SetPrinterActiveAsync(0, true);

                    var models = await _server.GetAllFilesAsync("local");
                    Assert.IsTrue(models != null && models.Count > 0);
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }
        /**/
        [Test]
        public async Task OnlineTest()
        {
            if (_skipOnlineTests) return;
            try
            {
                OctoPrintClient _server = new OctoPrintClient(_host, _api, _port, _ssl);
                _server.Error += (o, args) =>
                {
                    Assert.Fail(args.ToString());
                };
                _server.ServerWentOffline += (o, args) =>
                {
                    Assert.Fail(args.ToString());
                };
                // Wait 10 minutes
                CancellationTokenSource cts = new CancellationTokenSource(new TimeSpan(0, 10, 0));
                do
                {
                    await Task.Delay(5000);
                    await _server.CheckOnlineAsync();
                } while (_server.IsOnline && !cts.IsCancellationRequested);
                Assert.IsTrue(cts.IsCancellationRequested);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task DownloadModelTest()
        {
            string _modelPath = "http://192.168.10.44/downloads/files/local/babygroot_0.6n_0.15mm_PLA_MK3S_1d2h57m.gcode";
            try
            {
                OctoPrintClient _server = new OctoPrintClient(_host, _api, _port, _ssl);
                _server.Error += (o, args) =>
                {
                    Assert.Fail(args.ToString());
                };
                _server.ServerWentOffline += (o, args) =>
                {
                    Assert.Fail(args.ToString());
                };
                byte[] file = await _server.DownloadFileFromUriAsync(_modelPath);
                Assert.IsNotNull(file);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task WebsocketTest()
        {
            try
            {
                if (_skipWebSocketTests) return;

                Dictionary<DateTime, string> websocketMessages = new Dictionary<DateTime, string>();
                OctoPrintClient _server = new OctoPrintClient(_host, _api, _port, _ssl);
                await _server.SetPrinterActiveAsync(0);
                _server.StartListening();

                _server.Error += (o, args) =>
                {
                    Assert.Fail(args.ToString());
                };
                _server.ServerWentOffline += (o, args) =>
                {
                    Assert.Fail(args.ToString());
                };

                _server.WebSocketDataReceived += (o, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Message))
                    {
                        websocketMessages.Add(DateTime.Now, args.Message);
                        Debug.WriteLine($"WebSocket Data: {args.Message} (Total: {websocketMessages.Count})");
                    }
                };

                _server.WebSocketError += (o, args) =>
                {
                    Assert.Fail($"Websocket closed due to an error: {args}");
                };

                // Wait 10 minutes
                CancellationTokenSource cts = new CancellationTokenSource(new TimeSpan(0, 10, 0));
                _server.WebSocketDisconnected += (o, args) =>
                {
                    if (!cts.IsCancellationRequested)
                        Assert.Fail($"Websocket unexpectly closed: {args}");
                };

                do
                {
                    await Task.Delay(10000);
                    await _server.CheckOnlineAsync();
                } while (_server.IsOnline && !cts.IsCancellationRequested);
                _server.StopListening();


                Assert.IsTrue(cts.IsCancellationRequested);
            }
            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task SetHeatedbedTest()
        {
            //if (_skipPrinterActionTests) return;
            try
            {
                OctoPrintClient _server = new OctoPrintClient(_host, _api, _port, _ssl);
                _server.Error += (s, e) =>
                {
                    Assert.Fail($"Error occured: {e?.ToString()}");
                };
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    if (_server.ActivePrinter == null)
                        await _server.SetPrinterActiveAsync(1, true);

                    bool result = await _server.SetBedTemperatureAsync(25);
                    // Set timeout to 5 minutes
                    var cts = new CancellationTokenSource(new TimeSpan(0, 5, 0));

                    if (result)
                    {
                        double? temp = 0;
                        // Wait till temp rises
                        while (temp < 23)
                        {
                            var state = await _server.GetCurrentBedStateAsync(true);
                            if (state != null && state.Bed != null)
                            {
                                var bed = state.Bed;
                                if (bed == null)
                                {
                                    Assert.Fail("No heated bed found");
                                    break;
                                }
                                temp = bed.Actual;
                            }
                        }
                        Assert.IsTrue(temp >= 23);
                        // Turn off bed
                        result = await _server.SetBedTemperatureAsync(0);
                        // Set timeout to 5 minutes
                        cts = new CancellationTokenSource(new TimeSpan(0, 5, 0));
                        if (result)
                        {

                            while (temp > 23)
                            {
                                var state = await _server.GetCurrentBedStateAsync(true);
                                if (state != null && state.Bed != null)
                                {
                                    var bed = state.Bed;
                                    if (bed == null)
                                    {
                                        Assert.Fail("No heated bed found");
                                        break;
                                    }
                                    temp = bed.Actual;
                                }
                            }
                            Assert.IsTrue(temp <= 23);
                        }
                        else
                            Assert.Fail("Command failed to be sent.");
                    }
                    else
                        Assert.Fail("Command failed to be sent.");
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
            }
            catch (TaskCanceledException texc)
            {
                Assert.Fail(texc.Message);
            }

            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task SetExtruderTest()
        {
            //if (_skipPrinterActionTests) return;
            try
            {
                OctoPrintClient _server = new OctoPrintClient(_host, _api, _port, _ssl);
                _server.Error += (s, e) =>
                {
                    Assert.Fail($"Error occured: {e?.ToString()}");
                };
                await _server.CheckOnlineAsync();
                if (_server.IsOnline)
                {
                    if (_server.ActivePrinter == null)
                        await _server.SetPrinterActiveAsync(1, true);

                    bool result = await _server.SetToolTemperatureAsync(30);
                    // Set timeout to 3 minutes
                    var cts = new CancellationTokenSource(new TimeSpan(0, 3, 0));

                    if (result)
                    {
                        double? extruderTemp = 0;
                        // Wait till temp rises
                        while (extruderTemp < 28)
                        {
                            var state = await _server.GetCurrentToolStateAsync(true);
                            if (state != null && state.Tool0 != null)
                            {
                                var extruder = state.Tool0;
                                if (extruder == null)
                                {
                                    Assert.Fail("No extrudes available");
                                    break;
                                }
                                extruderTemp = extruder.Actual;
                            }
                        }
                        Assert.IsTrue(extruderTemp >= 28);
                        // Turn off extruder
                        result = await _server.SetToolTemperatureAsync(0);
                        // Set timeout to 3 minutes
                        cts = new CancellationTokenSource(new TimeSpan(0, 3, 0));
                        if (result)
                        {

                            while (extruderTemp > 28)
                            {
                                var state = await _server.GetCurrentToolStateAsync(true);
                                if (state != null && state.Tool0 != null)
                                {
                                    var extruder = state.Tool0;
                                    if (extruder == null)
                                    {
                                        Assert.Fail("No extrudes available");
                                        break;
                                    }
                                    extruderTemp = extruder.Actual;
                                }
                            }
                            Assert.IsTrue(extruderTemp <= 28);
                        }
                        else
                            Assert.Fail("Command failed to be sent.");
                    }
                    else
                        Assert.Fail("Command failed to be sent.");
                }
                else
                    Assert.Fail($"Server {_server.FullWebAddress} is offline.");
            }
            catch (TaskCanceledException texc)
            {
                Assert.Fail(texc.Message);
            }

            catch (Exception exc)
            {
                Assert.Fail(exc.Message);
            }
        }

        [Test]
        public async Task ConnectionBuilderTest()
        {
            string host = "192.168.10.112";
            string api = "_yourkey";

            using OctoPrintClient client = new OctoPrintClient.OctoPrintConnectionBuilder()
                .WithServerAddress(host, 3344, false)
                .WithApiKey(api)
                .Build();
            await client.CheckOnlineAsync();
            Assert.IsTrue(client?.IsOnline ?? false);
        }
    }
}