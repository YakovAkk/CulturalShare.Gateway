namespace CulturalShare.Gateway.Models.Model.Request;

public class UpdatePostRequestModel
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
}
