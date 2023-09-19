using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MipLivrelStore.Controllers;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string MyEmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(MyAccountController.ConfirmEmail),
                controller: "MyAccount",
                values: new { userId, code },
                protocol: scheme);
        }
         
        public static string MyResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string email, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(MyAccountController.ResetPassword),
                controller: "MyAccount",
                values: new { userId, email, code },
                protocol: scheme);
        }

        public static string CompleteUserInformationLink(this IUrlHelper urlHelper, string userEmail, string scheme)
        {
            return urlHelper.Action(
                action: nameof(MyAccountController.CompleteAccountDetails),
                controller: "MyAccount",
                values: new { userEmail },
                protocol: scheme);
        }
    }
}
