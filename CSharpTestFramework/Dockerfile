FROM mcr.microsoft.com/playwright/dotnet:v1.55.0-noble

WORKDIR /app

COPY . .

# Install Grafana K6
RUN apt-get update && apt-get install -y gnupg ca-certificates curl
RUN gpg -k
RUN gpg --no-default-keyring --keyring /usr/share/keyrings/k6-archive-keyring.gpg --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys C5AD17C747E3415A3642D57D77C6C491D6AC1D69
RUN echo "deb [signed-by=/usr/share/keyrings/k6-archive-keyring.gpg] https://dl.k6.io/deb stable main" | tee /etc/apt/sources.list.d/k6.list
RUN apt-get update
RUN apt-get install k6

# Install Allure Reports
RUN apt-get update && apt-get install -y openjdk-17-jre-headless wget unzip && \
    wget https://github.com/allure-framework/allure2/releases/download/2.29.0/allure-2.29.0.zip && \
    unzip allure-2.29.0.zip -d /opt/ && \
    ln -s /opt/allure-2.29.0/bin/allure /usr/local/bin/allure

RUN dotnet restore

RUN dotnet clean

RUN dotnet build --no-restore

# Install Playwright browsers
RUN pwsh UI.Tests/bin/Debug/net8.0/playwright.ps1 install --with-deps

CMD sh -c "dotnet test --no-build && allure generate --clean allure-results -o allure-report"