using CompomentsLibrary.Helper;

using Memy.Shared.Model;

using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace PagesLibrary.Pages
{
    public partial class AddPages : IDisposable
    {

        #region Preview
        private async Task Preview(InputFileChangeEventArgs inputFile)
        {
            try
            {
                _popUp.ShowToats("Trwa wczytywanie plików", "Pliki", PopupLevel.Level.None);
#if DEBUG
                _logger.LogInformation("Initialized Preview");
#endif
                if (inputFile.FileCount > Memy.Shared.Helper.FileRequirements.MaxNumberOfFiles)
                {
                    _error.Add($"Przekroczyłeś dopuszczalną liczbę plików {Environment.NewLine}Dopuszczalna liczba = {Memy.Shared.Helper.FileRequirements.MaxNumberOfFiles}");
                    _logger.LogError("Too many images max {0}", Memy.Shared.Helper.FileRequirements.MaxNumberOfFiles);
                }
                for (int i = 0; i < inputFile.FileCount; i++)
                {
                    if (_fileUploadStatuses.Count >= Memy.Shared.Helper.FileRequirements.MaxNumberOfFiles)
                        break;

                    var status = Data.File.CheckingFile.GetStatus(inputFile.GetMultipleFiles()[i]);
                    if (!string.IsNullOrWhiteSpace(status.Item2))
                    {
                        _error.Add(status.Item2);
                        _logger.LogError("Error: {0}", status.Item2);
                        continue;
                    }
                    _fileUploadStatuses.Add(status.Item1);
                    StateHasChanged();

                    var result = await Data.File.CheckingFile.CorrectData(inputFile.GetMultipleFiles()[i], status.Item1);
                    if (_fileUploadStatuses[i] != null)
                    {
                        _fileUploadStatuses[_fileUploadStatuses.Count - 1] = result;
                    }
                    _popUp.ShowToats($"{_fileUploadStatuses[i].Name} został dodany do listy", "Nowy plik", PopupLevel.Level.Info);
                    _logger.LogInformation("Show new image {0}", _fileUploadStatuses[i].Name);
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        private void RemoveAt(int index)
        {
            try
            {
                if (_fileUploadStatuses != null)
                {
                    var name = _fileUploadStatuses[index].Name;
                    _fileUploadStatuses.RemoveAt(index);
                    StateHasChanged();
                    _popUp.ShowToats($"{name} został usunięty", "Usunięty", PopupLevel.Level.Info);
                    _logger.LogInformation("Remove item at {0}", index);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private void RemoveTagAt(int index)
        {
            if (index >= 0)
            {
                _fileAdd.Tag.RemoveAt(index);
                _logger.LogInformation("Remove tag");
            }
        }
        #endregion

        protected override void OnInitialized()
        {
            _editContext = new EditContext(_fileAdd);
            _editContext.OnFieldChanged += HandleFieldChanged;

            _fileUploadStatuses = new List<FileUploadStatus?>();
            _error = new List<string?>();
            cts = new CancellationTokenSource();
            _fileAdd.Categories = CategoriesTable[0];
#if DEBUG
            _logger.LogInformation("Initialized page");
#endif
        }
        protected override async Task OnInitializedAsync()
        {
            var taglist = await _iFileManager.GetTagAsync();
            if (taglist.IsSuccessStatusCode)
            {
                TagTable = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(await taglist.Content.ReadAsStringAsync());
            }
            else
            {
                _logger.LogError("Tag Error:{0}", await taglist.Content.ReadAsStringAsync());
            }
#if DEBUG
            _logger.LogInformation("Initialized async page");
#endif
        }
        public void HandleFieldChanged(object sender, FieldChangedEventArgs e)
        {
            if (_editContext != null)
            {
                _formInvalid = !_editContext.Validate();
                _error.Clear();
            }
            StateHasChanged();
        }

        #region Send
        private async Task HandleValidSubmit()
        {
            _logger.LogInformation("Valid Submit");
            if (_editContext != null)
            {
                try
                {
                    if (!_editContext.Validate())
                    {
                        return;
                    }
                    if (!(await _modal.ShowPopup("Przesyłanie", "Czy na pewno chcesz przesłać pliki?", "Tak", "Nie")))
                    {
                        return;
                    }
                    _popUp.ShowToats("Rozpoczęto przestłanie pliku", "Przesyłanie", PopupLevel.Level.Info);
                    var request = new FileUploadModel();
                    request.Title = _fileAdd.Title;
                    request.Description = _fileAdd.Description;
                    request.Categories = _fileAdd.Categories.Item1;
                    request.Tag = new string[_fileAdd.Tag.Count];
                    for (int i = 0; i < _fileAdd.Tag.Count; i++)
                    {
                        request.Tag[i] = _fileAdd.Tag[i];
                    }

                    request.FileUploadStatuses = new FileUploadStatus[_fileUploadStatuses.Count];

                    for (int i = 0; i < _fileUploadStatuses.Count; i++)
                    {
                        request.FileUploadStatuses[i] = new FileUploadStatus();
                        request.FileUploadStatuses[i].Name = _fileUploadStatuses[i].Name;
                        request.FileUploadStatuses[i].Typ = _fileUploadStatuses[i].Typ;
                        request.FileUploadStatuses[i].Data = new byte[_fileUploadStatuses[i].Data.Length];
                        request.FileUploadStatuses[i].Data = _fileUploadStatuses[i].Data;
                    }

                    var result = await _iFileManager.PostFileAsync(request);

                    if (result.IsSuccessStatusCode)
                    {
                        _popUp.ShowToats("Plik został udostępniony", "Przesyłanie", PopupLevel.Level.Success);
                        await Task.Run(async () =>
                        {
                            await Task.Delay(2 * 1000);
                            _error.Clear();
                            _fileAdd = new FileAdd();
                            _fileUploadStatuses.Clear();
                            StateHasChanged();

                        });
                    }
                    else
                    {
                        _logger.LogError(await result.Content.ReadAsStringAsync());
                        _popUp.ShowToats("Nie udało się przesłać pliku", "Przesyłanie", PopupLevel.Level.Error);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }
        #endregion


        public void Dispose()
        {
#if DEBUG
            _logger.LogInformation("Dispose");
#endif
            if (_editContext != null)
            {
                _editContext.OnFieldChanged -= HandleFieldChanged;
            }
            _fileUploadStatuses = null;
            _error = null;
        }

        class FileAdd
        {
            [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Tytuł jest wymagany")]
            [System.ComponentModel.DataAnnotations.MinLength(length: 3, ErrorMessage = "Tytuł jest za krótki")]
            [System.ComponentModel.DataAnnotations.MaxLength(length: 100, ErrorMessage = "Tytuł jest za długi")]
            public string? Title { get; set; }
            [System.ComponentModel.DataAnnotations.MaxLength(length: 5000, ErrorMessage = "Opis jest za długi")]
            public string? Description { get; set; }
            public List<string> Tag { get; set; }
            [System.ComponentModel.DataAnnotations.MaxLength(length: 20, ErrorMessage = "Tag jest za długi")]
            private string? _SimpleTag;
            public string? SimpleTag
            {
                get => _SimpleTag;
                set
                {
                    if (_SimpleTag != value)
                    {
                        _SimpleTag = value;
                        if (string.IsNullOrWhiteSpace(_SimpleTag))
                        {
                            return;
                        }
                        if (_SimpleTag[SimpleTag.Length - 1] == ' ')
                        {
                            ReadOnlySpan<char> chars = _SimpleTag;
                            Tag.Add(chars.Slice(0, chars.Length - 1).ToString());
                            _SimpleTag = string.Empty;
                        }
                    }
                }
            }


            public (string, string) Categories { get; set; }
            public FileAdd()
            {
                Tag = new List<string>();
            }
        }
    }
}
