//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Blog.Models
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    
    public partial class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SelfSlogo { get; set; }
        public string SelfPhoto { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        [AllowHtml]
        public string AboutMe { get; set; }
    }
}