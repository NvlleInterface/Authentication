﻿namespace Authentication.Application.Common.Services.PasswordHashers;

public interface IPasswordHasher
{
    string HashPassword(string password);

    bool VerifyPassword(string password, string passwordHash);
}
