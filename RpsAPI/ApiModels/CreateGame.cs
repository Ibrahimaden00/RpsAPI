namespace RpsAPI.ApiModels
{
    public class CreateGame
    {
        public long GameID { get; set; }

      
        public long InviteFromID { get; set; }

    
        public long InviteToID { get; set; }
        public int? InviteFromIDMove { get; set; }
        public int? InviteToIDMove { get; set; }
        public string? Gamestatus { get; set; }
    }
}
