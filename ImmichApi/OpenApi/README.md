# Regenerating the ImmichApi C# Client

This guide explains how to use `nswag` to regenerate the `ImmichApi.cs` client from the OpenAPI specification. This is necessary if the API specification changes and the client needs to be updated.

## Prerequisites

1. Ensure you have `nswag` installed. You can install it via npm:
   ```bash
   npm install -g nswag
   ```

## Execution
1. Run the following nswag command from powershell to generate the C# client from the latest OpenAPI specification:

```bash
nswag openapi2csclient /input:https://raw.githubusercontent.com/immich-app/immich/main/open-api/immich-openapi-specs.json /classname:ImmichApi /namespace:SimpleImmichFrame.ImmichApi /output:ImmichApi.cs /generateOptionalPropertiesAsNullable:true
```