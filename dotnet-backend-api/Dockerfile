
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
RUN pwd
WORKDIR /app
RUN pwd
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
RUN pwd
RUN ls

FROM build AS final
ENTRYPOINT ["dotnet", "dotnet-backend-api.dll"]
