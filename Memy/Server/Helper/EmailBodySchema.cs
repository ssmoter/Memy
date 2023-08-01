using System.Text;

namespace Memy.Server.Helper
{
    internal static class EmailBodySchema
    {

        internal static string Register(string token)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div><p> Twój link do aktywacji konta");

            sb.AppendLine("<a href=\"");
            sb.Append(token);
            sb.Append("\">");
            sb.Append(token);
            sb.Append("</a></p>");
            sb.AppendLine("<p>Jeżeli to nie ty zakładałes konto zignoruj tego maila</p> </div>");

            return sb.ToString();
        }

    }
}
