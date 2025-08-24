// Copyright (c) dominuxLABS. All rights reserved.

using TheSkibiditeca.Web.Models.Entities;

namespace TheSkibiditeca.Web.Models.Auth
{
    /// <summary>
    /// Represents the model for user registration data.
    /// </summary>
    public class RegisterModel
    {
        public User user { get; set; }
        public string passwordStr { get; set; }
        public string passwordConf {  get; set; }
    }
}
