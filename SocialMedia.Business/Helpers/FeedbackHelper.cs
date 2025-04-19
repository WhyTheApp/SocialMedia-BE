namespace SocialMedia.Business.Helpers;

public class FeedbackHelper
{
    public static bool isValidFeedback(string feedback)
    {
        const int maxLength = 550;
        
        if(feedback.Length > maxLength)
            return false;
        
        return true;
    }
}