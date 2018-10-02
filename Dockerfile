FROM microsoft/dotnet:latest as build
WORKDIR /app

RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs

# copy csproj and restore as distinct layers
COPY *.sln .
COPY ChatAppReact/*.csproj ./ChatAppReact/
RUN dotnet restore

# WORKDIR /app/ChatAppReact/ClientApp
# RUN npm install
# RUN npm rebuild node-sass --force
# WORKDIR /app

# copy everything else and build app
COPY ChatAppReact/. ./ChatAppReact/
WORKDIR /app/ChatAppReact
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/ChatAppReact/out ./
EXPOSE 80
ENTRYPOINT ["dotnet", "ChatAppReact.dll"]