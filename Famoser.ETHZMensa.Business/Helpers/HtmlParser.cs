using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.ETHZMensa.Business.Models;
using Famoser.FrameworkEssentials.Logging;
using Famoser.FrameworkEssentials.Singleton;

namespace Famoser.ETHZMensa.Business.Helpers
{
    public class HtmlParser : SingletonBase<HtmlParser>
    {
        public bool ParseHtml(string html, MensaModel mensa)
        {
            try
            {
                if (!html.Contains("<table class=\"silvatable grid\""))
                    return false;

                html = html.Substring(html.IndexOf("<table class=\"silvatable grid\"", StringComparison.Ordinal));
                html = html.Substring(0, html.IndexOf("</table>", StringComparison.Ordinal));

                var entries = html.Split(new[] { "<tr>" }, StringSplitOptions.None);
                mensa.MenuMittags.Clear();
                for (int i = 2; i < entries.Length; i++)
                {
                    var columns = entries[i].Split(new[] { "<td>" }, StringSplitOptions.None);

                    var menu = new MenuModel();
                    menu.MenuName = columns[1].Substring(3);
                    menu.MenuName = menu.MenuName.Substring(0, menu.MenuName.IndexOf("</b>", StringComparison.Ordinal)).Trim();

                    if (columns[2].Contains("<br/>"))
                    {
                        menu.Title =
                            columns[2].Substring(0, columns[2].IndexOf("<br/>", StringComparison.Ordinal)).Trim();

                        columns[2] = columns[2].Substring(columns[2].IndexOf("<br/>", StringComparison.Ordinal) + 5);
                        menu.Description = columns[2].Substring(0, columns[2].IndexOf("</td>", StringComparison.Ordinal));
                        menu.Description = menu.Description.Replace("<br/>", "\n");
                    }
                    else
                    {
                        menu.Title = columns[2].Substring(0, columns[2].IndexOf("</td>", StringComparison.Ordinal));
                    }
                    if (columns[3].Contains("<br />"))
                    {
                        menu.Prices = columns[3].Substring(0, columns[3].IndexOf("<br />", StringComparison.Ordinal));
                    }

                    mensa.MenuMittags.Add(menu);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex);
            }
            return false;
        }
    }
}
