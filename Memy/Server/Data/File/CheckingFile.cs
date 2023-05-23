using Memy.Shared.Helper;
using Memy.Shared.Model;

namespace Memy.Server.Data.File
{
    internal static class CheckingFile
    {
        internal static async Task<FileUploadStatus?> CorrectData(FileUploadStatus file, IWebHostEnvironment webHost)
        {
            try
            {
                var status = new FileUploadStatus()
                {
                    Name = file.Name,
                    Typ = file.Typ
                };
                using (MemoryStream ms = new MemoryStream(file.Data))
                {
                    //spawdzanie czy plik nie jest za duży
                    if (ms.Length >= FileRequirements.MaxSizeOfFile)
                    {
                        //status.Error = "Za duży rozmiar pliku";
                        return status;
                    }
                    //sprawdzanie czy rozszerzenia pliku
                    if (!FileRequirements.FileTypAccess.Any(x => x == status.Typ))
                    {
                        //status.Error = "Nie poprawne rozszerzenie pliku";
                        return status;
                    }
                    //bezpieczna nazwa
                    var trustedFileName = Guid.NewGuid().ToString();
                    //ścieżka do folderu
                    var path = Path.Combine(webHost.ContentRootPath, webHost.EnvironmentName, FileRequirements.PatchFolderName);
                    //tworzenie folderu
                    CreateFolder(path);
                    //ścieżka do pliku
                    path = Path.Combine(path, trustedFileName);
                    path = Path.ChangeExtension(path, status.Typ);
                    //zapisanie pliku
                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        await ms.CopyToAsync(fs);
                    }
                    status.Name = trustedFileName;
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //pobranie tylko nazwy
        public static ReadOnlySpan<char> GetName(ReadOnlySpan<char> value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == '.')
                {
                    return value.Slice(0, i);
                }
            }
            return null;
        }
        //pobranie tylko typu
        public static ReadOnlySpan<char> GetType(ReadOnlySpan<char> value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == '.')
                {
                    return value.Slice(i + 1);
                }
            }
            return null;
        }
        //tworzenie folderu gdzie zapisywane są pliki
        private static void CreateFolder(string patch)
        {
            if (!System.IO.File.Exists(patch))
            {
                Directory.CreateDirectory(patch);
            }
        }

    }
}
