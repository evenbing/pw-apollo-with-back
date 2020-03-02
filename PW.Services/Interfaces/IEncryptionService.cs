using System;
using System.Collections.Generic;
using System.Text;

namespace PW.Services.Interfaces
{
    public interface IEncryptionService
    {
        string CreateSalt();
        string EncryptPassword(string password, string salt);
    }
}
