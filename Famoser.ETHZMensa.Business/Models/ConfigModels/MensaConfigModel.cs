using Famoser.ETHZMensa.Business.Enums;

namespace Famoser.ETHZMensa.Business.Models.ConfigModels
{
    public class MensaConfigModel
    {
        public string Name { get; set; }
        public string MealTime { get; set; }
        public string IdSlug { get; set; }
        public string TimeSlug { get; set; }
        public string ApiUrlSlug { get; set; }
        public string InfoUrlSlug { get; set; }
        public bool InfoDayDependent { get; set; }
        public LocationType Type { get; set; }
    }
}
