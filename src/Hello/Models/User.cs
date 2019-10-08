using System;

namespace Hello.Models
{
    public class User
    {
        public string Id { get; set; }
        /// <summary>
        /// 登录账号
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }
    }
}
