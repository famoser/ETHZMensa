using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.ETHZMensa.Business.Enums;
using Famoser.ETHZMensa.Business.Models;

namespace Famoser.ETHZMensa.Business.Helpers
{
    public class UriHelper
    {
        private static string EthApiUrl = "https://www.webservices.ethz.ch/gastro/v1/RVRI/Q1E1/mensas/[ID]/de/menus/daily/[DAY_DATE]/[TIME_SLUG]?language=de";

        private static string EthMenuUrl = "https://www.ethz.ch/de/campus/gastronomie/menueplaene/offerDay.html?language=de&id=[ID]&date=[DAY_DATE]";

        private static string EthInfoUrl = "https://www.ethz.ch/de/campus/gastronomie/restaurants-und-cafeterias/[INFO_URL_SLUG]";

        private static string UzhApiUrl = "http://www.mensa.uzh.ch/menueplaene/[API_URL_SLUG]_[DAY_SHORT].html";

        private static string UzhMenuUrl = "http://www.mensa.uzh.ch/menueplaene/[API_URL_SLUG]_[DAY_SHORT].html";

        private static string UzhInfoUrl = "http://www.mensa.uzh.ch/standorte/[INFO_URL_SLUG].html";

        private static string UzhInfoDayDependentUrl = "http://www.mensa.uzh.ch/standorte/[INFO_URL_SLUG]_[DAY_SHORT].html";

        public static Uri GetTodayApiUrl(MensaModel mensa)
        {
            if (mensa.Type == LocationType.Eth)
                return GetLink(EthApiUrl, mensa);
            return GetLink(UzhApiUrl, mensa);
        }

        public static Uri GetTodayMenuUrl(MensaModel mensa)
        {
            if (mensa.Type == LocationType.Eth)
                return GetLink(EthMenuUrl, mensa);
            return GetLink(UzhMenuUrl, mensa);
        }

        public static Uri GetInfoUrl(MensaModel mensa)
        {
            if (mensa.Type == LocationType.Eth)
                return GetLink(EthInfoUrl, mensa);
            if (mensa.InfoDayDependent) 
                return GetLink(UzhInfoDayDependentUrl, mensa);
            return GetLink(UzhInfoUrl, mensa);
        }

        private static Uri GetLink(string template, MensaModel mensa)
        {
            return new Uri(template.Replace("[ID]", mensa.IdSlug).Replace("[DAY_DATE]", GetDayDate()).Replace("[DAY_SHORT]", GetDayShort()).Replace("[INFO_URL_SLUG]", mensa.InfoUrlSlug).Replace("[API_URL_SLUG]", mensa.ApiUrlSlug).Replace("[TIME_SLUG]", mensa.TimeSlug));
        }

        private static string GetDayShort()
        {
            if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
                return "mo";
            if (DateTime.Today.DayOfWeek == DayOfWeek.Tuesday)
                return "di";
            if (DateTime.Today.DayOfWeek == DayOfWeek.Wednesday)
                return "mi";
            if (DateTime.Today.DayOfWeek == DayOfWeek.Thursday)
                return "do";
            if (DateTime.Today.DayOfWeek == DayOfWeek.Friday)
                return "fre";
            if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday)
                return "sa";
            return "so";
        }

        private static string GetDayDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
}
