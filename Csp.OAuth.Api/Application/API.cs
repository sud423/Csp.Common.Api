namespace Csp.OAuth.Api.Application
{
    public static partial class API
    {
        public static string Remote_Service_Base_Url = null;

        public static class WeiXin
        {
            public static string GetWxUserByCode(string baseUrl,string code) => $"{baseUrl}/getsnsuser/{code}";
        }
    }
}
