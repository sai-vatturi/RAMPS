# Use the official .NET 8.0 SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy and restore dependencies
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
COPY --from=build /publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "RecipeMeal.API.dll"]
