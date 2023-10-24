using AndreasReitberger.API.OctoPrint.Enum;
using AndreasReitberger.API.OctoPrint.Models;
using AndreasReitberger.API.OctoPrint.Structs;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AndreasReitberger.API.OctoPrint
{
    public partial class OctoPrintClient
    {
        #region Properties

        #endregion

        #region Methods

        public override async Task<ObservableCollection<IGcode>> GetFilesAsync()
        {
            ObservableCollection<OctoPrintModel> models = await GetAllFilesAsync(location: OctoPrintFileLocations.local.ToString()).ConfigureAwait(false);
            return new(models?.Select(gcode => gcode.File));
        }
        public async Task<ObservableCollection<OctoPrintModel>> GetAllFilesAsync(string location, string path = "", bool recursive = true)
        {
            try
            {
                OctoPrintFiles models = await GetFilesAsync(location, path, recursive).ConfigureAwait(false);
                if (models != null)
                {
                    //return IterateOctoPrintFileStack(models.Children ?? models.Files);
                    return IterateOctoPrintFileStack(models?.Children?.Count > 0 ? models?.Children : models?.Files);
                }
                else
                {
                    return new ObservableCollection<OctoPrintModel>();
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new ObservableCollection<OctoPrintModel>();
            }
        }

        public async Task RefreshFilesAsync()
        {
            try
            {
                ObservableCollection<OctoPrintModel> modelData = new();
                if (!IsReady || ActivePrinter == null)
                {
                    Models = modelData;
                    return;
                }
                Models = await GetAllFilesAsync(CurrentFileLocation.ToString()).ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                Models = new ObservableCollection<OctoPrintModel>();
            }
        }

        public async Task RefreshFilesAsync(string Location)
        {
            try
            {
                ObservableCollection<OctoPrintModel> modelData = new();
                if (!IsReady || ActivePrinter == null)
                {
                    Models = modelData;
                    return;
                }
                Models = await GetAllFilesAsync(Location).ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                Models = new ObservableCollection<OctoPrintModel>();
            }
        }
        public Task RefreshFilesAsync(OctoPrintFileLocations Location) => RefreshFilesAsync(Location.ToString());

        public async Task<OctoPrintFile> GetFileAsync(string location, string filename)
        {
            try
            {
                string command = string.Format("files/{0}/{1}", location, filename);

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Get, command)
                    .ConfigureAwait(false);
                */
                OctoPrintFile response = JsonConvert.DeserializeObject<OctoPrintFile>(result.Result);
                return response;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return new OctoPrintFile();
            }
        }

        public async Task<bool> SelectFileAsync(OctoPrintFile file, bool startPrint = false)
        {
            try
            {
                string command = string.Format("files/{0}/{1}", file.Origin, file.FilePath);
                object parameter = new { command = "select", print = startPrint ? "true" : "false" };

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                // no content for this result
                /*
                OctoPrintApiRequestResponse result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                return string.IsNullOrEmpty(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        /*
        public async Task<bool> SliceFileAsync(OctoPrintFile file, bool startPrint = false)
        {
            try
            {
                throw new NotImplementedException();
                
                string command = string.Format("files/{0}/{1}", file.Origin, file.Path);
                object parameter = new { command = "select", print = startPrint ? "true" : "false" };

                var result = await sendRestAPIRequestAsync(command, Method.Post, parameter);
                var response = JsonConvert.DeserializeObject<OctoPrintFileActionRespond>(result);
                return result.Succeeded;
                
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        */

        public async Task<bool> CopyFileAsync(OctoPrintFile file, string destination)
        {
            try
            {
                string command = string.Format("files/{0}/{1}", file.Origin, file.FilePath);
                object parameter = new
                {
                    command = "copy",
                    destination = destination
                };

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                OctoPrintApiRequestResponse result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Copy, command, parameter)
                    .ConfigureAwait(false);
                */
                OctoPrintFileActionRespond response = JsonConvert.DeserializeObject<OctoPrintFileActionRespond>(result.Result);
                return result.Succeeded;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> MoveFileAsync(OctoPrintFile file, string destination)
        {
            try
            {
                string command = string.Format("files/{0}/{1}", file.Origin, file.FilePath);
                object parameter = new
                {
                    command = "move",
                    destination = destination
                };

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: parameter,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                /*
                OctoPrintApiRequestResponse result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Post, command, parameter)
                    .ConfigureAwait(false);
                */
                OctoPrintFileActionRespond response = JsonConvert.DeserializeObject<OctoPrintFileActionRespond>(result.Result);
                return result.Succeeded;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> DeleteFileAsync(OctoPrintFile file)
        {
            try
            {
                string command = string.Format("files/{0}/{1}", file.Origin, file.FilePath);

                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone result = await SendRestApiRequestAsync(
                       requestTargetUri: targetUri,
                       method: Method.Post,
                       command: command,
                       jsonObject: null,
                       authHeaders: AuthHeaders,
                       //urlSegments: urlSegments,
                       cts: default
                       )
                    .ConfigureAwait(false);
                //no result
                /*
                OctoPrintApiRequestResponse result = await SendRestApiRequestAsync(OctoPrintCommandBase.api, Method.Delete, command)
                    .ConfigureAwait(false);
                */
                return string.IsNullOrEmpty(result.Result);
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> UploadFileAsync(OctoPrintFileLocation location, string target, string filePath, bool select = false, bool print = false)
        {
            try
            {
                IRestApiRequestRespone result =
                    await SendMultipartFormDataFileRestApiRequestAsync(filePath, location, target, select, print)
                    .ConfigureAwait(false);
                if (result != null)
                {
                    OctoPrintUploadFileResponse response = JsonConvert.DeserializeObject<OctoPrintUploadFileResponse>(result.Result);
                    return response.Done;
                }
                else return false;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public async Task<bool> UploadFileAsync(OctoPrintFileLocation location, string target, byte[] file, string fileName, bool select = false, bool print = false)
        {
            try
            {
                IRestApiRequestRespone result =
                    await SendMultipartFormDataFileRestApiRequestAsync(file, fileName, location, target, select, print)
                    .ConfigureAwait(false);
                if (result != null)
                {
                    OctoPrintUploadFileResponse response = JsonConvert.DeserializeObject<OctoPrintUploadFileResponse>(result.Result);
                    return response.Done;
                }
                else return false;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }
        public async Task<bool> CreateNewFolderAsync(string location, string path, string name)
        {
            try
            {
                IRestApiRequestRespone result = await SendMultipartFormDataFolderRestApiRequestAsync(name, location, path);
                if (result != null)
                    return result.Succeeded;
                else return false;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public Task<byte[]> DownloadFileAsync(string downloadUri, int timeout = 100000) => DownloadFileFromUriAsync(downloadUri, timeout);
        #endregion

    }
}
