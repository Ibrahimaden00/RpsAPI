namespace RpsAPI.ApiModels
{
    public class CreateNewMsg
    {
       
        public long FromUserID { get; set; }
        public long ToUserID { get; set; }
        public string? MessageValue { get; set; }
    }
}
