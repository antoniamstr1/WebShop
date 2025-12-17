
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /WebShop

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /WebShop

ENV TOKEN="MySuperSecureAndRandomKeyThatLooksJustAwesomeAndNeedsToBeVeryVeryLong!!!111oneeleven"
ENV ISSUER="MyAwesomeApp"
ENV AUDIENCE="MyAwesomeAudience"

COPY --from=build /WebShop/out .

EXPOSE 5001
ENTRYPOINT ["dotnet", "WebShop.dll"]
