namespace Langate.FacialRecognition.Model
{
    public class ApiCall : Entity
    {
        public int ApiCallId { get; set; }
        public string DeviceId { get; set; }
        public string IpAddress { get; set; }
        public string ApiMethod { get; set; }
        public string ParamsJson { get; set; }

        public int? TokenId { get; set; }
        public Token Token { get; set; }

        protected ApiCall() { }

        public ApiCall(string deviceId,
            string ipAddress,
            string apiMethod,
            string paramsJson,
            Token token)
        {
            DeviceId = deviceId;
            IpAddress = ipAddress;
            ApiMethod = apiMethod;
            ParamsJson = paramsJson;
            Token = token;
        }
    }
}
