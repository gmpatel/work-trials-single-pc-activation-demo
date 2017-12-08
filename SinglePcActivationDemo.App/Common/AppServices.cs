using RestSharp;
using SinglePcActivationDemo.Common.OperationMessages;

namespace SinglePcActivationDemo.App.Common
{
    public class AppServices
    {
        // RestSharp call to Login api // Calling Login api from AppController of MVC project
        public LoginResponse Login(LoginRequest request)
        {
            var client = new RestClient("http://localhost:50720");
            var webRequest = new RestRequest("api/app/login", Method.POST);

            webRequest.AddJsonBody(request);
            var response = client.Execute<LoginResponse>(webRequest);

            return response.Data;
        }

        // RestSharp call to Activate api // Calling Activate api from AppController of MVC project
        public ActivateResponse Activate(ActivateRequest request)
        {
            var client = new RestClient("http://localhost:50720");
            var webRequest = new RestRequest("api/app/activate", Method.POST);

            webRequest.AddJsonBody(request);
            var response = client.Execute<ActivateResponse>(webRequest);

            return response.Data;
        }
    }
}