#FROM node:10.15-alpine AS client
#EXPOSE 4200 49153
#USER node
#RUN mkdir /home/node/.npm-global
#ENV PATH=/home/node/.npm-global/bin:$PATH
#ENV NPM_CONFIG_PREFIX=/home/node/.npm-global
#RUN npm install -g @angular/cli@8.1.0
#WORKDIR /app
#CMD ["ng", "serve", "--port", "4200", "--host", "0.0.0.0", "--disable-host-check", "--poll", "2000"]




#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
# Setup NodeJs
RUN apt-get update && \
    apt-get install -y wget && \
    apt-get install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_8.x | bash - && \
    apt-get install -y build-essential nodejs
# End setup

WORKDIR /app
EXPOSE 80

FROM node:10.15-alpine AS client 
ARG skip_client_build=false 
WORKDIR /app 
COPY Sidekick.Examination.App/ClientApp . 
RUN [[ ${skip_client_build} = true ]] && echo "Skipping npm install" || npm install 
RUN [[ ${skip_client_build} = true ]] && mkdir dist || npm run-script build


FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Sidekick.Examination.App/Sidekick.Examination.App.csproj", "Sidekick.Examination.App/"]
RUN dotnet restore "Sidekick.Examination.App/Sidekick.Examination.App.csproj"
COPY . .
WORKDIR "/src/Sidekick.Examination.App"
RUN dotnet build "Sidekick.Examination.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sidekick.Examination.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=client /app/dist /app/dist
ENTRYPOINT ["dotnet", "Sidekick.Examination.App.dll"]