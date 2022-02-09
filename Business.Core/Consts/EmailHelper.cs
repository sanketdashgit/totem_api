
using System;
using System.Net;

namespace Totem.Business.Core.Consts
{
    public static class EmailHelper
    {
        public static string AccountActivation(string FirstName, string LastName, string Link)
        {

            string HTMLString = "Dear " + FirstName + " " + LastName + ",<br/><br/> Please " +
                                "<a href='" + Link + "'>Click Here</a> to activate your account." +
                                "<br/><br/>Thank You, <br/><br/>Totem.";

            return HTMLString;
        }

        public static string ReceiveEmailUpdatesSave(string message)
        {

            string HTMLString = "Welcome to Totem,<br/><br/>Thanks for subscribing.<br/><br/>" + "Here at Totem, we believe in building a strong community of like - minded people to discuss the latest trends.<br/><br/>" +
"You’ve come to the right place to find the most recent news and events in your area, previews of the latest trends, as well as special offers and promotions.<br/>"
                + "<br/><br/>Warm Regards,<br/>Totem.";

            return HTMLString;
        }

        public static string EarlyToParty(string message)
        {

            string HTMLString = "Welcome to Totem,<br/><br/>Thanks for Your Interest In Totem.<br/><br/>Coming Soon<br/><br/>" + "Here at Totem, we believe in building a strong community of like - minded people to discuss the latest trends.<br/><br/>" +
"You’ve come to the right place to find the most recent news and events in your area, previews of the latest trends, as well as special offers and promotions.<br/>"
                + "<br/><br/>Warm Regards,<br/>Totem.";

            return HTMLString;
        }


     
        public static string ForgotPassword(string FirstName, string LastName, string Link)
        {

            string HTMLString = "Dear " + FirstName + " " + LastName + ",<br/><br/> Please " +
                                "<a href='" + Link + "'>Click Here</a> for reset password." +
                                "<br/><br/>Thank You,<br/><br/>Totem.";

            return HTMLString;
        }
        public static string ResetPassword(string FirstName, string LastName)
        {
            string HTMLString = "Dear " + FirstName + " " + LastName + ",<br/><br/>" +
                                "Your Password has been set successfully." +
                                "<br/><br/>Thank You, <br/><br/>Totem.";

            return HTMLString;
        }
        public static string AccountActivated(string FirstName, string LastName)
        {
            string HTMLString = "Dear " + FirstName + " " + LastName + ",<br/><br/>" +
                                "Your account is activated successfully." +
                                "<br/><br/>Thank You, <br/><br/>Totem";

            return HTMLString;
        }
    }
}
