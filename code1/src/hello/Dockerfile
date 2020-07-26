FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as runtime
EXPOSE 80
ENV ASPNETCORE_URLS http://+:80

WORKDIR /app
COPY . /app
ENTRYPOINT ["dotnet", "hello.dll"]