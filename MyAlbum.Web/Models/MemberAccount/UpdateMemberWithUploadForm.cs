using MyAlbum.Models.MemberAccount;

namespace MyAlbum.Web.Models.MemberAccount
{
    public class UpdateMemberWithUploadForm: UpdateMemberAccountReq
    {
        public List<IFormFile> Files { get; set; } = new();
    }
}
