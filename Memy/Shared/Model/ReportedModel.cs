namespace Memy.Shared.Model
{
    public class ReportedModel
    {
        public int Id { get; set; }
        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    if (_value > 0)                    
                        IsChecked = true;                  
                    else
                        IsChecked = false;
                }
            }
        }
        public int ValueSum { get; set; }
        public bool IsChecked { get; set; }

    }
}
