namespace Famoser.ETHZMensa.Business.Models.Eth
{
    public class RootObject
    {
        public int id { get; set; }
        public string mensa { get; set; }
        public string daytime { get; set; }
        public Hours hours { get; set; }
        public Menu menu { get; set; }
    }
}