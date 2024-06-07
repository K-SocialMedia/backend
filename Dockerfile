FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 80
#BUILD STAGE
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src  
COPY ["./ChatChit.csproj","."]
RUN dotnet restore "./ChatChit.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./ChatChit.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChatChit.csproj" -c Release -o /app/publish


# FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
FROM base AS final
WORKDIR /app
COPY --from=publish  /app/publish .
ENTRYPOINT [ "dotnet", "ChatChit.dll" ]
#docker build --rm -t chatchit-dotnet-backend .
#docker run -d --name chatchit-dotnet-backend -p 5000:80 chatchit-dotnet-backend