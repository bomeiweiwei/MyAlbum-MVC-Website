using MyAlbum.Models.MemberAccount;

namespace MyAlbum.Web.Models.MemberAccount
{
    public class UpdateMemberWithUploadForm: UpdateMemberAccountReq
    {
        public IFormFile? File { get; set; }
    }
}
