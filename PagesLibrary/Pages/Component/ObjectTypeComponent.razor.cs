using Memy.Shared.Helper;

namespace PagesLibrary.Pages.Component
{
    public partial class ObjectTypeComponent : IDisposable
    {
        private int _descriptionLength = 300;
        private string _descriptionFirst;
        private string _descriptionLast;
        private string _descriptionSmall;
        protected override void OnInitialized()
        {
            SetDescription();
        }
        protected override void OnParametersSet()
        {
            SetDescription();
        }

        private void SetDescription()
        {
            if (fileModel.ObjTyp == (int)MyEnums.FileTyp.text)
            {
                _descriptionFirst = GetFirstSegment(fileModel.ObjName).ToString();
                _descriptionLast = GetRestSegment(fileModel.ObjName).ToString();
                _descriptionSmall = GetSmall(fileModel.ObjName).ToString();
            }
        }
        private ReadOnlySpan<char> GetSmall(ReadOnlySpan<char> value)
        {
            if (value.Length > 10)
            {
                return value.Slice(0, 10);
            }
            else
            {
                return value.Slice(0, value.Length);
            }
        }
        private ReadOnlySpan<char> GetFirstSegment(ReadOnlySpan<char> value)
        {
            if (value.Length > _descriptionLength)
            {
                return value.Slice(0, _descriptionLength);
            }
            else
            {
                return value.Slice(0, value.Length);
            }
        }
        private ReadOnlySpan<char> GetRestSegment(ReadOnlySpan<char> value)
        {
            if (value.Length > _descriptionLength)
            {
                return value.Slice(_descriptionLength, value.Length - _descriptionLength);
            }
            else
            {
                return null;
            }
        }



        public void Dispose()
        {
            fileModel = null;
        }
    }
}
