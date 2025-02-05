# Use the official .NET 8.0 SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the csproj files and restore dependencies
COPY ./RecipeMeal.API/RecipeMeal.API.csproj ./RecipeMeal.API/
COPY ./RecipeMeal.Core/RecipeMeal.Core.csproj ./RecipeMeal.Core/
COPY ./RecipeMeal.Infrastructure/RecipeMeal.Infrastructure.csproj ./RecipeMeal.Infrastructure/
RUN dotnet restore ./RecipeMeal.API/RecipeMeal.API.csproj

# Copy the entire source and build the application
COPY . .
RUN dotnet publish ./RecipeMeal.API/RecipeMeal.API.csproj -c Release -o /publish

# Use the .NET 8.0 runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /publish .

# Ensure the /app/RecipeMeal.API directory is created as a directory
RUN rm -rf /app/RecipeMeal.API && mkdir -p /app/RecipeMeal.API

# Now copy appsettings.json into the correct directory
COPY ./RecipeMeal.API/appsettings.json /app/RecipeMeal.API/appsettings.json

# Expose the ports for both HTTP (80) and HTTPS (443)
EXPOSE 80
EXPOSE 443

# Start the application with HTTPS enabled
ENTRYPOINT ["dotnet", "RecipeMeal.API.dll"]
