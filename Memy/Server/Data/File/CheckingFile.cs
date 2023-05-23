using Memy.Shared.Helper;
using Memy.Shared.Model;

using System.Buffers.Text;
using System.Runtime.InteropServices;

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
                    var trustedFileName = ToStringFromGuid(Guid.NewGuid());
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
        //usuwanie plikow
        public static void DeleteFile(string name, string typ, IWebHostEnvironment webHost)
        {
            var path = Path.Combine(webHost.ContentRootPath, webHost.EnvironmentName, FileRequirements.PatchFolderName, name);
            path = Path.ChangeExtension(path, typ);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }


        private const byte SlashByte = (byte)'/';
        private const char HyphenChar = '-';

        private const char plusChar = '+';
        private const byte plusByte = (byte)'+';
        private const char Underscore = '_';

        //parse guid to string
        private static string ToStringFromGuid(Guid value)
        {
            Span<byte> idBytes = stackalloc byte[16];
            Span<byte> base64CBytes = stackalloc byte[24];

            MemoryMarshal.TryWrite(idBytes, ref value);
            Base64.EncodeToUtf8(idBytes, base64CBytes, out _, out _);

            Span<char> finalChars = stackalloc char[22];

            for (int i = 0; i < 22; i++)
            {
                finalChars[i] = base64CBytes[i] switch
                {
                    SlashByte => HyphenChar,
                    plusByte => Underscore,
                    _ => (char)base64CBytes[i],
                };
            }

            return new string(finalChars);
        }

    }
}
