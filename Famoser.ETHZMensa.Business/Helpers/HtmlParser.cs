using System;
using System.Linq;
using System.Text.RegularExpressions;
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
                //go to right area
                if (!html.Contains("<div class=\"mod mod-newslist\" id=\"box-1\">"))
                    return false;
                html = html.Substring(html.IndexOf("<div class=\"mod mod-newslist\" id=\"box-1\">", StringComparison.Ordinal) + 5);

                //go to specific menu area
                if (!html.Contains("<div class=\"text-basics\">"))
                    return false;
                html = html.Substring(html.IndexOf("<div class=\"text-basics\">", StringComparison.Ordinal));

                //cut at end
                html = html.Substring(0, html.IndexOf("</div", StringComparison.Ordinal));

                var entries = html.Split(new[] { "<h3>" }, StringSplitOptions.None);
                mensa.Menus.Clear();


                for (int i = 1; i < entries.Length; i++)
                {
                    var localHtml = entries[i];
                    var menu = new MenuModel
                    {
                        MenuName = localHtml.Substring(0, localHtml.IndexOf("<span", StringComparison.Ordinal)).Trim()
                    };

                    if (localHtml.Contains("CHF "))
                    {
                        localHtml = localHtml.Substring(localHtml.IndexOf("CHF ", StringComparison.Ordinal) + 4);
                        menu.Prices = localHtml.Substring(0, localHtml.IndexOf("<", StringComparison.Ordinal)).Trim('\n', ' ');
                    }
                    localHtml = localHtml.Substring(localHtml.IndexOf("<p>", StringComparison.Ordinal) + 3);

                    if (localHtml.Contains("<br />"))
                        FillMenuInfo(localHtml, "<br />", menu);
                    else if (localHtml.Contains("<br>"))
                        FillMenuInfo(localHtml, "<br>", menu);
                    else
                        menu.Title = localHtml.Substring(localHtml.IndexOf("<", StringComparison.Ordinal)).Trim('\n', ' ');

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

        private void FillMenuInfo(string html, string divider, MenuModel menu)
        {
            menu.Title = html.Substring(0, html.IndexOf(divider, StringComparison.Ordinal)).Trim('\n', ' ');
            html = html.Substring(html.IndexOf(divider, StringComparison.Ordinal) + divider.Length);
            if (html.Contains("</p>"))
            {
                menu.Description = html.Substring(0, html.IndexOf("</p>", StringComparison.Ordinal));
            }
            menu.Description = menu.Description.Replace(divider, "\n").Replace("\n ", "\n").Trim();
        }
    }
}
