using Localization.Resources;
using System.ComponentModel.DataAnnotations;

namespace Core.Users.DAL.Constants
{
    public enum UserVerificationStatus
    {
        [Display(Name = nameof(SharedResource.UserVeificationStatus_EmailNotVerified), ResourceType = typeof(SharedResource))]
        EmailNotVerified = 1,
        [Display(Name = nameof(SharedResource.UserVeificationStatus_PasswordResetRequested), ResourceType = typeof(SharedResource))]
        PasswordResetRequested = 2,
        // add more statuses here
        [Display(Name = nameof(SharedResource.UserVeificationStatus_Verified), ResourceType = typeof(SharedResource))]
        Verified = 10
    }
}
