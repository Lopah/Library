# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [vNext]

### Changed

- Target .NET 11 across all libraries and tests, using the pinned 11.0.100 SDK.
- Replace GitHub Actions and Nuke with Forgejo Actions and plain .NET CLI checks
  on the shared `ubuntu-22.04` runners.
- Migrate API versioning to `Asp.Versioning` and Swagger to Swashbuckle 10 with
  OpenAPI 3.1. See the README migration notes for consumer changes.
- Use the ASP.NET Core shared framework and System.Text.Json instead of legacy
  ASP.NET Core packages and the Newtonsoft-based MVC formatter.

### Fixed

- Generate versioned Swagger documents and UI links that respect `UsePathBase`
  and nested or empty UI route prefixes.
- Describe date converters, binary downloads, response headers, and authorization
  metadata in the generated OpenAPI contracts.

## [0.0.1] / 2022-07-22
- Added this project
