using Memy.Shared.Helper;
using Memy.Shared.Model;

using Microsoft.AspNetCore.Components.Forms;

namespace PagesLibrary.Data.File
{
    public static class CheckingFile
    {
        public static async Task<FileUploadStatus?> CorrectData(IBrowserFile file, FileUploadStatus? status)
        {
            try
            {
                using (Stream fileStream = file.OpenReadStream(FileRequirements.MaxSizeOfFile))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await fileStream.CopyToAsync(ms);
                        status.Data = new byte[ms.Length];
                        status.Data = ms.ToArray();
                        status.ImgUrl = $"data:image/{file.Name};base64,{Convert.ToBase64String(status.Data)}";
                    }
                }
                return status;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static (FileUploadStatus?, string?) GetStatus(IBrowserFile file)
        {
            try
            {
                string error = "";
                FileUploadStatus? status = new FileUploadStatus
                {
                    Name = GetName(file.Name).ToString(),
                    Typ = GetType(file.Name).ToString()
                };
                if (file.Size >= FileRequirements.MaxSizeOfFile)
                {
                    error = "Za duży rozmiar pliku";
                    return (status, error);
                }
                if (!FileRequirements.FileTypAccess.Any(x => x == status.Typ))
                {
                    error = "Nie poprawne rozszerzenie pliku";
                    return (status, error);
                }
                return (status, error);

            }
            catch (Exception)
            {
                throw;
            }
        }



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

    }
}
