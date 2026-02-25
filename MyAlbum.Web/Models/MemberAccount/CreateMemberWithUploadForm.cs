using MyAlbum.Models.MemberAccount;

namespace MyAlbum.Web.Models.MemberAccount
{
    public class CreateMemberWithUploadForm: CreateMemberReq
    {
        public List<IFormFile> Files { get; set; } = new();
    }
}
