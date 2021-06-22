
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
RUN pwd
WORKDIR /app
RUN pwd
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["dotnet-backend-api.dll", "./"]
RUN pwd
RUN ls

FROM build AS final
RUN pwd
RUN ls
WORKDIR /app
RUN pwd
RUN ls
COPY --from=build /src .
RUN pwd
RUN ls
ENTRYPOINT ["dotnet", "dotnet-backend-api.dll"]
