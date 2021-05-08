#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# Explicitly ask for path to be added
ENV PATH="${PATH}:/root/.dotnet/tools"

#RUN dotnet tool install --global dotnet-ef

COPY ["Asakabank.UserApi/Asakabank.UserApi.csproj", "Asakabank.UserApi/"]
RUN dotnet restore "Asakabank.UserApi/Asakabank.UserApi.csproj"
COPY . .
WORKDIR "/src/Asakabank.UserApi"
RUN dotnet build "Asakabank.UserApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Asakabank.UserApi.csproj" -c Release -o /app/publish

# Run updates
#RUN dotnet ef migrations add InitialCreate -c DataContext
#RUN dotnet ef database update -c DataContext

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Asakabank.UserApi.dll"]