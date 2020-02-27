# Build stage
FROM microsoft/dotnet:2.1-sdk AS build

# set working directory
WORKDIR /app

# Restore
COPY src/Interface/Interface.csproj ./src/Interface/
RUN dotnet restore src/Interface/Interface.csproj
COPY src/Service/Service.csproj ./src/Service/
RUN dotnet restore src/Service/Service.csproj
COPY src/Client/Client.csproj ./src/Client/
RUN dotnet restore src/Client/Client.csproj
COPY src/Process/Process.csproj ./src/Process/
RUN dotnet restore src/Process/Process.csproj
COPY test/Service.Test/Service.Test.csproj ./test/Service.Test/
RUN dotnet restore test/Service.Test/Service.Test.csproj
COPY test/Client.Test/Client.Test.csproj ./test/Client.Test/
RUN dotnet restore test/Client.Test/Client.Test.csproj

# Copy src
COPY . .

# Test
#RUN dotnet test tests/Service.Test/Service.Test.csproj

# Publish
RUN dotnet publish src/Process/Process.csproj -o /obj

# Runtime stage
FROM microsoft/dotnet:2.1-runtime

# set working directory
WORKDIR /app

# Copy compiled binaries
COPY --from=build /obj ./bin

# Copy configuration
COPY config/*.* ./config/

ENV MONGO_SERVICE_URI ""
ENV MONGO_SERVICE_HOST mongo
ENV MONGO_SERVICE_PORT 27017
ENV MONGO_DB app

EXPOSE 8080

CMD ["dotnet", "./bin/run.dll", "-c", "./config/config.yml"]
