FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
WORKDIR /app
ENV ASPNETCORE_URLS="http://*:5556"
EXPOSE 5555

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
RUN find -type d -name bin -prune -exec rm -rf {} \; && find -type d -name obj -prune -exec rm -rf {} \;

COPY . .
RUN ls
WORKDIR "/src/Find&Register/"
RUN dotnet build "Find&Register.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "Find&Register.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Creating and using a non-root user
RUN useradd -m -s /bin/bash appuser
USER appuser

ENTRYPOINT ["dotnet", "Find&Register.dll"]
