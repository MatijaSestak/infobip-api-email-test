using Infobip.Api.Client;
using Infobip.Api.Client.Api;
using Newtonsoft.Json;
using RestSharp;
using System.Text;

var emailMessage = "TEST MESSAGE";
var senderEmailAddress = "msestak@appcro.com";
var emailReceiversTo = new List<string>() { "msestak@appcro.com" };
var emailReceiversCc = new List<string>();
var emailReceiversBcc = new List<string>();
var inReplyTo = string.Empty;
var emailSubject = "TEST";

var username = "*** YOUR USERNAME ***";
var password = "*** YOUR PASSWORD ***";

var configuration = new Configuration()
{
    BasePath = "https://1981gx.api.infobip.com",
    ApiKeyPrefix = "App",
    ApiKey = "**** YOUR API KEY ****"
};

var sendEmailApi = new SendEmailApi(configuration);

var toList = new List<string>();


var client = new RestClient($"{configuration.BasePath}");

string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
               .GetBytes(username + ":" + password));

if (emailMessage == null)
    throw new Exception("EMAIL_MESSAGE_NOT_DEFINED");

var restRequest = new RestRequest("email/3/send", Method.Post);
restRequest.AddHeader("Authorization", $"Basic {encoded}");
restRequest.AlwaysMultipartFormData = true;
restRequest.AddParameter("from", senderEmailAddress);

foreach (var receiver in emailReceiversTo)
{
    restRequest.AddParameter("to", receiver);
}

foreach (var ccReceiver in emailReceiversCc)
{
    restRequest.AddParameter("cc", ccReceiver);
}

foreach (var bccReceiver in emailReceiversBcc)
{
    restRequest.AddParameter("cc", bccReceiver);
}


if (!string.IsNullOrWhiteSpace(inReplyTo))
    restRequest.AddParameter("replyTo", inReplyTo);

restRequest.AddParameter("subject", emailSubject);
restRequest.AddParameter("html", emailMessage);


var files = new List<string>() { "file.txt" };

//here is problem: how to add files to attachment property?
foreach (var file in files)
{
    byte[] testFile = File.ReadAllBytes(file);
    var form = new MultipartFormDataContent
                    {
                        { new ByteArrayContent(testFile, 0, testFile.Length), "file.txt", "file.txt" }
                    };

    restRequest.AddParameter("attachment", JsonConvert.SerializeObject(form));
}


var emailResponse = client.Execute(restRequest);

Console.WriteLine("*** END ***");
Console.ReadLine();