using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Famoser.ETHZMensa.Business.Models;
using Famoser.ETHZMensa.Business.Models.Eth;
using Famoser.FrameworkEssentials.Logging;
using Famoser.FrameworkEssentials.Singleton;
using Newtonsoft.Json;

namespace Famoser.ETHZMensa.Business.Helpers
{
    public class HtmlParser : SingletonBase<HtmlParser>
    {
        public bool ParseEthHtml(string html, MensaModel mensa)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<RootObject>(html);
                mensa.Menus.Clear();
                foreach (var meal in obj.menu.meals)
                {
                    var menu = new MenuModel
                    {
                        MenuName = meal.label,
                        MenuType = meal.type,
                        Prices = meal.prices.student + " / " + meal.prices.staff + " / " + meal.prices.@extern,
                        Title = meal.description.FirstOrDefault()
                    };
                    if (meal.description.Count > 1)
                        menu.Description = string.Join("\n", meal.description.GetRange(1, meal.description.Count - 1));

                    mensa.Menus.Add(menu);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex);
            }
            return false;
        }

        public bool ParseEthAbendessenHtml(string html, MensaModel mensa)
        {
            try
            {
                if (!html.Contains("<table class=\"silvatable grid\""))
                    return false;

                html = html.Substring(html.IndexOf("<table class=\"silvatable grid\"", StringComparison.Ordinal) + 20);

                return ParseEthHtml(html, mensa);
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex);
            }
            return false;
        }

        public bool ParseUzhHtml(string html, MensaModel mensa)
        {
            try
            {
                if (!html.Contains("<div class=\"news\""))
                    return false;

                html = html.Substring(html.IndexOf("<div class=\"news\"", StringComparison.Ordinal));
                html = html.Substring(0, html.IndexOf("</div", StringComparison.Ordinal));

                var entries = html.Split(new[] { "<h3>" }, StringSplitOptions.None);
                mensa.Menus.Clear();


                for (int i = 1; i < entries.Length; i++)
                {
                    var menu = new MenuModel();
                    menu.MenuName = entries[i].Substring(0, entries[i].IndexOf("<span", StringComparison.Ordinal)).Trim();

                    if (entries[i].Contains("CHF "))
                    {
                        entries[i] = entries[i].Substring(entries[i].IndexOf("CHF ", StringComparison.Ordinal) + 4);
                        menu.Prices = entries[i].Substring(0, entries[i].IndexOf("<", StringComparison.Ordinal)).Trim('\n', ' ');
                    }
                    entries[i] = entries[i].Substring(entries[i].IndexOf("<p>", StringComparison.Ordinal) + 3);

                    if (entries[i].Contains("<br />"))
                    {
                        menu.Title = entries[i].Substring(0, entries[i].IndexOf("<br />", StringComparison.Ordinal)).Trim('\n', ' ');
                        entries[i] = entries[i].Substring(entries[i].IndexOf("<br />", StringComparison.Ordinal) + 6);
                        menu.Description = entries[i].Substring(0, entries[i].IndexOf("</p>", StringComparison.Ordinal)).Trim('\n', ' ');
                        menu.Description = menu.Description.Replace("<br />", "");
                        menu.Description = Regex.Replace(menu.Description, @" +", " ");
                        menu.Description = menu.Description.Replace("\n ", "\n");
                    }
                    else
                    {
                        menu.Title = entries[i].Substring(entries[i].IndexOf("<", StringComparison.Ordinal)).Trim('\n', ' ');
                    }

                    mensa.Menus.Add(menu);
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
