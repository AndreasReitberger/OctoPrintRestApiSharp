using AndreasReitberger.API.OctoPrint.Enum;
using AndreasReitberger.API.OctoPrint.Models;
using AndreasReitberger.API.OctoPrint.Structs;
using AndreasReitberger.API.Print3dServer.Core;
using AndreasReitberger.API.Print3dServer.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndreasReitberger.API.OctoPrint
{
    public partial class OctoPrintClient
    {
        #region Properties

        #endregion

        #region Methods

        public override async Task<List<IGcodeGroup>> GetModelGroupsAsync(string path = "")
        {
            //List<IGcode> files = await GetAllFilesAsync().ConfigureAwait(false);
            List<IGcode> files = await GetFilesAsync().ConfigureAwait(false);
            IEnumerable<OctoPrintGroup> directories = files
                .Select(gc => gc.Group)
                .Distinct()
                .Select(groupName => new OctoPrintGroup() { Name = groupName, DirectoryName = groupName, Path = groupName });
            return [.. directories];
        }

        public override async Task<List<IGcode>> GetFilesAsync()
        {
            List<OctoPrintModel> models = await GetAllFilesAsync(location: OctoPrintFileLocations.local.ToString()).ConfigureAwait(false);
            return [.. models?.Select(gcode => gcode.File)];
        }
        public async Task<List<OctoPrintModel>> GetAllFilesAsync(string location, string path = "", bool recursive = true)
        {
            try
            {
                OctoPrintFiles? models = await GetFilesAsync(location, path, recursive).ConfigureAwait(false);
                if (models is not null)
                {
                    //return IterateOctoPrintFileStack(models.Children ?? models.Files);
                    return IterateOctoPrintFileStack(models?.Children?.Count > 0 ? models?.Children : models?.Files);
                }
                else
                {
                    return [];
                }
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return [];
            }
        }

        public Task RefreshFilesAsync() => RefreshFilesAsync(CurrentFileLocation.ToString());
        /*
        {
            try
            {
                List<OctoPrintModel> modelData = [];
                if (!IsReady || ActivePrinter is null)
                {
                    Files = [];
                    Groups = [];
                    return;
                }
                modelData = await GetAllFilesAsync(CurrentFileLocation.ToString()).ConfigureAwait(false);
                
                IEnumerable<OctoPrintModel> files = modelData.Where(md => !md.IsFolder && md.File is not null);
                Files = [.. files.Select(md => md.File)];

                IEnumerable<OctoPrintModel> folders = modelData.Where(md => md.IsFolder);
                Groups = [.. folders.Select(f => new OctoPrintGroup() { Name = f.Name, DirectoryName = f.Name, Path = f.Path, Root = f.Location)];
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                Files = [];
                Groups = [];
            }
        }*/

        public async Task RefreshFilesAsync(string location)
        {
            try
            {
                List<OctoPrintModel> modelData = [];
                if (!IsReady || ActivePrinter is null)
                {
                    Files = [];
                    Groups = [];
                    return;
                }
                modelData = await GetAllFilesAsync(location).ConfigureAwait(false);

                IEnumerable<OctoPrintModel> files = modelData.Where(md => !md.IsFolder && md.File is not null);
                Files = [.. files.Select(md => md.File)];

                IEnumerable<OctoPrintModel> folders = modelData.Where(md => md.IsFolder);
                Groups = [.. folders.Select(f => new OctoPrintGroup() { Name = f.Name, DirectoryName = f.Name, Path = f.Path, Root = f.Location })];
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                Files = [];
                Groups = [];
            }
        }
        public Task RefreshFilesAsync(OctoPrintFileLocations Location) => RefreshFilesAsync(Location.ToString());

        public async Task<OctoPrintFile?> GetFileAsync(string location, string filename)
        {
            try
            {
                string command = string.Format("files/{0}/{1}", location, filename);
                string targetUri = $"{OctoPrintCommands.Api}";
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                OctoPrintFile? response = GetObjectFromJson<OctoPrintFile>(result?.Result, NewtonsoftJsonSerializerSettings);
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return string.IsNullOrEmpty(result?.Result);
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
                var response = GetObjectFromJson<OctoPrintFileActionRespond>(result);
                return result?.Succeeded ?? false;
                
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                OctoPrintFileActionRespond? response = GetObjectFromJson<OctoPrintFileActionRespond>(result?.Result, NewtonsoftJsonSerializerSettings);
                return result?.Succeeded ?? false;
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                OctoPrintFileActionRespond? response = GetObjectFromJson<OctoPrintFileActionRespond>(result?.Result, NewtonsoftJsonSerializerSettings);
                return result?.Succeeded ?? false;
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
                IRestApiRequestRespone? result = await SendRestApiRequestAsync(
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
                return string.IsNullOrEmpty(result?.Result);
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
                string targetRequestUri = $"/api/files/{location}";
                Dictionary<string, string> parameters = new()
                {
                    { "select", select ? "true" : "false" },
                    { "print", print ? "true" : "false" },
                    { "path", target },
                };
                IRestApiRequestRespone? result =
                    await SendMultipartFormDataFileRestApiRequestAsync(
                        requestTargetUri: targetRequestUri, authHeaders: AuthHeaders, localFilePath: filePath)
                    .ConfigureAwait(false);
                /*
                result =
                    await SendMultipartFormDataFileRestApiRequestAsyncOld(filePath, location, target, select, print)
                    .ConfigureAwait(false);
                */
                if (result is not null)
                {
                    OctoPrintUploadFileResponse? response = GetObjectFromJson<OctoPrintUploadFileResponse>(result?.Result, NewtonsoftJsonSerializerSettings);
                    return response?.Done ?? false;
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
                string targetRequestUri = $"/api/files/{location}";
                Dictionary<string, string> parameters = new()
                {
                    { "select", select ? "true" : "false" },
                    { "print", print ? "true" : "false" },
                    { "path", target },
                };
                IRestApiRequestRespone? result =
                    await SendMultipartFormDataFileRestApiRequestAsync(
                        requestTargetUri: targetRequestUri, authHeaders: AuthHeaders, fileName: fileName, file: file)
                    .ConfigureAwait(false);
                /*
                result =
                    await SendMultipartFormDataFileRestApiRequestAsyncOld(file, fileName, location, target, select, print)
                    .ConfigureAwait(false);
                */
                if (result is not null)
                {
                    OctoPrintUploadFileResponse? response = GetObjectFromJson<OctoPrintUploadFileResponse>(result?.Result, NewtonsoftJsonSerializerSettings);
                    return response?.Done ?? false;
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
                string targetRequestUri = $"/api/files/{location}";
                Dictionary<string, string> parameters = new()
                {
                    { "foldername", name },
                    { "path", path },
                };
                IRestApiRequestRespone? result = 
                    await SendMultipartFormDataFileRestApiRequestAsync(requestTargetUri: targetRequestUri, authHeaders: AuthHeaders, parameters: parameters);
                //result = await SendMultipartFormDataFolderRestApiRequestAsync(name, location, path);
                if (result is not null)
                    return result?.Succeeded ?? false;
                else return false;
            }
            catch (Exception exc)
            {
                OnError(new UnhandledExceptionEventArgs(exc, false));
                return false;
            }
        }

        public override Task<byte[]?> DownloadFileAsync(string downloadUri) 
            => DownloadFileFromUriAsync(path: downloadUri, authHeaders: AuthHeaders, timeout: 100000);
        public Task<byte[]?> DownloadFileAsync(string downloadUri, int timeout = 100000) 
            => DownloadFileFromUriAsync(path: downloadUri, authHeaders: AuthHeaders, timeout: timeout);
        #endregion

    }
}
