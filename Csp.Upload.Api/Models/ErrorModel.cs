namespace Csp.Upload.Api.ViewModel
{
    public class ErrorModel
    {
        public int Error { get; } = 1;

        public string Message { get; }

        private ErrorModel(string msg)
        {
            Message = msg;
        }

        public static ErrorModel ShowErr(string msg)
        {
            return new ErrorModel(msg);
        }

    }

    public class SuccModel
    {
        public int Error { get; } = 0;

        public string Url { get; }

        private SuccModel(string url)
        {
            Url = url;
        }

        public static SuccModel ShowSucc(string url)
        {
            return new SuccModel(url);
        }
    }
}
