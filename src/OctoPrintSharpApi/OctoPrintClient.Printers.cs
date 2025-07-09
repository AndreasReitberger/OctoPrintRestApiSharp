using AndreasReitberger.API.OctoPrint.Models;
using AndreasReitberger.API.OctoPrint.Structs;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using AndreasReitberger.API.REST.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndreasReitberger.API.OctoPrint
{
    public partial class OctoPrintClient
    {
        #region Methods
        async Task<OctoPrintPrinterProfiles?> GetPrinterProfilesAsync()
        {
            try
            {
                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Get,
                       command: "printerprofiles",
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                IRestApiRequestRespone? result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, "printerprofiles")
                    .ConfigureAwait(false);
                */
                OctoPrintPrinterProfiles? list = GetObjectFromJson<OctoPrintPrinterProfiles>(result?.Result, NewtonsoftJsonSerializerSettings);
                return list;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintPrinterProfiles() { Profiles = new Dictionary<string, OctoPrintPrinter>() };
            }
        }

        public override async Task SetPrinterActiveAsync(int Index = -1, bool RefreshPrinterList = true)
        {
            try
            {
                if (RefreshPrinterList)
                    await RefreshPrinterListAsync().ConfigureAwait(false);
                if (Printers.Count > Index && Index >= 0)
                    ActivePrinter = Printers[Index];
                else
                {
                    // If no index is provided, or it's out of bound, the first online printer is used
                    ActivePrinter = Printers.FirstOrDefault(printer => printer.IsOnline);
                    // If no online printers is found, however there is at least one printer configured, use this one
                    if (ActivePrinter is null && Printers.Count > 0)
                        ActivePrinter = Printers[0];
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        public override async Task SetPrinterActiveAsync(string Id, bool RefreshPrinterList = true)
        {
            try
            {
                if (RefreshPrinterList)
                    await RefreshPrinterListAsync().ConfigureAwait(false);
                IPrinter3d? printer = Printers.FirstOrDefault(prt => prt.Slug == Id);
                if (printer is not null && ActivePrinter != printer)
                {
                    ActivePrinter = printer;
                    //Disconnect
                    //Connect
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        public async Task RefreshPrinterListAsync()
        {
            try
            {
                List<IPrinter3d> printers = await GetPrintersAsync().ConfigureAwait(false);
                Printers = [.. printers];
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }
        public async Task<List<IPrinter3d>> GetAllPrinterProfilesAsync()
        {
            try
            {
                OctoPrintPrinterProfiles? result = await GetPrinterProfilesAsync().ConfigureAwait(false);
                IEnumerable<OctoPrintPrinter>? profiles = result?.Profiles?.Select(pair => pair.Value);
                if (profiles is not null)
                {
                    List<IPrinter3d> profile = [.. profiles];
                    return profile;
                }
                else return [];
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return [];
            }
        }

        public override Task<List<IPrinter3d>> GetPrintersAsync() => GetAllPrinterProfilesAsync();

        public async Task<IPrinter3d?> GetPrinterProfileAsync(string slug)
        {
            try
            {
                OctoPrintPrinterProfiles? result = await GetPrinterProfilesAsync().ConfigureAwait(false);
                IPrinter3d? profile = new ObservableCollection<IPrinter3d>(result?.Profiles?.Select(pair => pair.Value) ?? []).FirstOrDefault(prof => prof.Slug == slug);
                return profile;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintPrinter();
            }
        }

        public async Task<OctoPrintPrinterState?> GetCurrentPrinterStateAsync(bool includeHistory, int limit = 0, string[]? excludes = null)
        {
            try
            {
                string command = "printer";
                Dictionary<string, string> urlSegments = new()
                {
                    { "history", includeHistory ? "true" : "false" }
                };
                if (limit > 0)
                    urlSegments.Add("limit", limit.ToString());
                if (excludes is not null && excludes.Length > 0)
                {
                    StringBuilder sb = new();
                    for (int i = 0; i < excludes.Length; i++)
                    {
                        sb.Append(excludes[i]);
                        if (i < excludes.Length - 1)
                            sb.Append(",");
                    }
                    urlSegments.Add("exclude", sb.ToString());
                }

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                OctoPrintApiRequestResponse result =
                    await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, command, jsonObject: null, cts: default, urlSegments)
                    .ConfigureAwait(false);
                */
                OctoPrintPrinterState? response = GetObjectFromJson<OctoPrintPrinterState>(result?.Result, NewtonsoftJsonSerializerSettings);
                return response;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintPrinterState();
            }
        }

        public async Task RefreshPrinterStateAsync(bool IncludeHistory = false)
        {
            try
            {
                if (!IsReady)
                {
                    return;
                }
                State = await GetCurrentPrinterStateAsync(IncludeHistory).ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
            }
        }

        #endregion

    }
}
