using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;

namespace SocialMedia.Business.Services.Authentication;

public class EmailerService: IEmailerService
{
    private readonly string _apiKey, _from, _domain, _serverUrl;

    public EmailerService(IConfiguration configuration)
    {
        _apiKey = configuration["Mailgun:ApiKey"];
        _from = configuration["Mailgun:From"];
        _domain = configuration["Mailgun:Domain"];
        _serverUrl = configuration["Mailgun:ServerURL"];
    }

    public async Task SendVerificationEmailAsync(string toName, string toEmail, string code)
    {
        var options = new RestClientOptions(_serverUrl)
        {
            Authenticator = new HttpBasicAuthenticator("api", _apiKey)
        };

        var client = new RestClient(options);
        var request = new RestRequest("/v3/"+_domain+"/messages", Method.Post);
        request.AlwaysMultipartFormData = true;
        request.AddParameter("from", _from);
        request.AddParameter("to", toName+ " <"+toEmail+">");
        request.AddParameter("subject", GetSubject());
        request.AddParameter("text", GetText(code));
        request.AddParameter("html", GetHtml(code));
        var response = await client.ExecuteAsync(request);
        if (!response.IsSuccessful)
        {
            throw new Exception("Failed to send email: " + response.Content);
        }
    }

    private string GetSubject()
    {
      return "Verify your email address";
    }
    
    private string GetText(string code)
    {
      return $"Your verification code is: {code}";
    }
    
    private string GetHtml(string code)
    {
        return $@"
          <!DOCTYPE html>
          <html lang='en'>
          <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Email Verification</title>
            <!--[if mso]>
            <noscript>
              <xml>
                <o:OfficeDocumentSettings>
                  <o:PixelsPerInch>96</o:PixelsPerInch>
                </o:OfficeDocumentSettings>
              </xml>
            </noscript>
            <![endif]-->
          </head>
          <body style='margin: 0; padding: 0; background-color: #0a0c16; font-family: Arial, sans-serif; color: #ffffff; -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%;'>
            <table role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%' style='background-color: #0a0c16; min-height: 100vh;'>
              <tr>
                <td align='center' valign='top' style='padding: 40px 20px;'>
                  <table role='presentation' cellspacing='0' cellpadding='0' border='0' width='100%' style='max-width: 600px; background-color: #0a0c16;'>
                    
                    <!-- Header -->
                    <tr>
                      <td align='center' style='padding: 0 0 24px 0;'>
                        <h1 style='margin: 0; color: #ffffff; font-size: 24px; font-weight: 600; text-align: center; font-family: Arial, sans-serif;'>
                          Verify your email
                        </h1>
                      </td>
                    </tr>
                    
                    <!-- Info Text -->
                    <tr>
                      <td align='center' style='padding: 0 0 32px 0;'>
                        <p style='margin: 0; color: #b0b0b0; font-size: 16px; text-align: center; line-height: 1.4; font-family: Arial, sans-serif;'>
                          Please use the code below to verify your email address.
                        </p>
                      </td>
                    </tr>
                    
                    <!-- Verification Code Button -->
                    <tr>
                      <td align='center' style='padding: 0 0 32px 0;'>
                        <table role='presentation' cellspacing='0' cellpadding='0' border='0'>
                          <tr>
                            <td align='center' style='border-radius: 10px; background-color: transparent; border: 2px solid #660033;'>
                              <div style='display: inline-block; padding: 16px 40px; color: #ffffff; font-size: 18px; font-weight: 600; text-align: center; letter-spacing: 2px; font-family: Arial, sans-serif; text-decoration: none; min-width: 200px; box-sizing: border-box;'>
                                {code}
                              </div>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                    
                    <!-- Additional Info -->
                    <tr>
                      <td align='center' style='padding: 0 0 40px 0;'>
                        <p style='margin: 0; color: #b0b0b0; font-size: 16px; text-align: center; line-height: 1.4; font-family: Arial, sans-serif;'>
                          If you did not request this, you can safely ignore this email.
                        </p>
                      </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                      <td align='center' style='padding: 20px 0 0 0; border-top: 1px solid #333;'>
                        <p style='margin: 0; color: #888888; font-size: 12px; text-align: center; font-family: Arial, sans-serif;'>
                          &copy; {DateTime.UtcNow.Year} WHY, the app
                        </p>
                      </td>
                    </tr>
                    
                  </table>
                </td>
              </tr>
            </table>
          </body>
          </html>";
    }
}