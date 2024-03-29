﻿using JetBrains.Annotations;

namespace OpenApi.Options;

[PublicAPI]
public class SecurityOptions
{
    public ClientCredentialOptions? ClientCredentials { get; init; }

    public ImplicitOptions? Implicit { get; init; }

    public AuthorizationCodeOptions? AuthorizationCode { get; init; }

    public ApiKeyOptions? ApiKey { get; init; }
}