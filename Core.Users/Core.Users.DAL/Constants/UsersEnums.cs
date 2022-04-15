using Localization.Resources;
using System.ComponentModel.DataAnnotations;

namespace Core.Users.DAL.Constants
{
    public enum UserVeificationStatus
    {
        [Display(Name = nameof(SharedResource.UserVeificationStatus_NotValid), ResourceType = typeof(SharedResource))]
        NotVerified = 0,
        // EmailAndPhoneNotVerified = 1,
        // EmailNotVerified = 2,
        // PhoneNotVerified = 3,
        [Display(Name = nameof(SharedResource.UserVeificationStatus_Verified), ResourceType = typeof(SharedResource))]
        Verified = 1
    }
}
