namespace UserMangement.Utils
{
    public class Response<T>
    {
            public string Message { set; get; }
            public string Code { set; get; }
            public T Data { set; get; }
        }

        public class AuthToken
        {
            public string Token { set; get; }
            public string ExpireAt { set; get; }
            public string RefreshToken { set; get; }
        }

        public class StatusCode
        {
            public string SUCCESS = "200";
            public string FAILED = "01";
            public string UNAUTHOURIZED = "401";
        }

        public class ClaimType
        {
            public static string OTP { get; set; }
            public static string UserName { get; set; }
        }
    }



    

