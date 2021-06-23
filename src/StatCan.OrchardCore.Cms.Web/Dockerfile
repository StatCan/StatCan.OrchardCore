FROM mcr.microsoft.com/dotnet/aspnet:5.0
EXPOSE 80
ENV ASPNETCORE_URLS http://+:80
WORKDIR /app
COPY ./.build/release .

ENTRYPOINT ["dotnet", "StatCan.OrchardCore.Cms.Web.dll"]
