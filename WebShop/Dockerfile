
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /WebShop

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /WebShop

ENV AppSettings__Token=MySuperSecureAndRandomKeyThatLooksJustAwesomeAndNeedsToBeVeryVeryLong!!!111oneeleven
ENV AppSettings__Issuer=MyAwesomeApp
ENV AppSettings__Audience=MyAwesomeAudience
ENV AppSettings__locallink=http://localhost:5173
ENV AppSettings__productionlink=https://webshopabysalto.onrender.com

COPY --from=build /WebShop/out .

EXPOSE 5001
ENTRYPOINT ["dotnet", "WebShop.dll"]
