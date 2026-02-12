FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["LanguageCards.Api.csproj", "."]
RUN dotnet restore "LanguageCards.Api.csproj"
COPY . .
RUN dotnet publish "LanguageCards.Api.csproj" -c Release -o /app


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "LanguageCards.Api.dll"]﻿