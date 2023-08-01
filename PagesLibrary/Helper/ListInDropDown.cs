using CompomentsLibrary.Model;

using Memy.Shared.Helper;

namespace PagesLibrary.Helper
{
    public static class ListInDropDown
    {
        public static ValueInDropDownList[] OrderTable = new ValueInDropDownList[]
        {
            new ValueInDropDownList(((int)MyEnums.OrderTyp.DateDESC).ToString(),"Najnowsze"),
            new ValueInDropDownList(((int)MyEnums.OrderTyp.DateASC).ToString(),"Najstarsze"),
            new ValueInDropDownList(((int)MyEnums.OrderTyp.ReactionDESC).ToString(),"Popularne"),
            new ValueInDropDownList(((int)MyEnums.OrderTyp.ReactionASC).ToString(),"Niepopularne"),
        };

        private static string _dateFormat = "yyyy-MM-dd HH:mm:ss";
        public static ValueInDropDownList[] DateTable = new ValueInDropDownList[]
        {
            new ValueInDropDownList("empty","brak"),
            new ValueInDropDownList(DateTimeOffset.Now.AddHours(-12).ToString(_dateFormat),"12 Godziń"),
            new ValueInDropDownList(DateTimeOffset.Now.AddDays(-1).ToString(_dateFormat),"1 Dzień"),
            new ValueInDropDownList(DateTimeOffset.Now.AddDays(-7).ToString(_dateFormat),"Tydzień"),
            new ValueInDropDownList(DateTimeOffset.Now.AddMonths(-1).ToString(_dateFormat),"Miesiąc"),
            new ValueInDropDownList(DateTimeOffset.Now.AddYears(-1).ToString(_dateFormat),"Rok"),
        };

        public static ValueInDropDownList[] CategoriesTable = new ValueInDropDownList[]
        {
            new ValueInDropDownList(Memy.Shared.Helper.Categories.Waiting,"Brak"),
        };
        public static ValueInDropDownList[] CategoriesTablePlusMain = new ValueInDropDownList[]
        {
            new ValueInDropDownList(Memy.Shared.Helper.Categories.Main,"Główna"),
            new ValueInDropDownList(Memy.Shared.Helper.Categories.Waiting,"Brak"),
        };
    }
}
