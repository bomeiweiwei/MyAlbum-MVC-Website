using MyAlbum.Models.MemberAccount;

namespace MyAlbum.Web.Models.MemberAccount
{
    public class CreateMemberWithUploadForm: CreateMemberReq
    {
        public IFormFile? File { get; set; }
    }
}
