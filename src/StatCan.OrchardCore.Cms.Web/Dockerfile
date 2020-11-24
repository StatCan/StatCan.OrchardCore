FROM mcr.microsoft.com/dotnet/aspnet:5.0
EXPOSE 80

WORKDIR /app
COPY ./docker_run.sh ./docker_run.sh
RUN chmod +x docker_run.sh
COPY ./.build/release .

ENTRYPOINT ["./docker_run.sh"]
