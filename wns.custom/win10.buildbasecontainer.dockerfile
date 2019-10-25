FROM dotnet/framework/aspnet-windows:4.8

RUN dism.exe /online /enable-feature /featurename:IIS-WebSockets

RUN powershell -Command Set-ExecutionPolicy Bypass -Force; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))

RUN powershell -Command choco install googlechrome -y
RUN powershell -Command choco install firefox -y

# Brute force installers
# RUN mkdir installers
# COPY ./installers/ /installers
# RUN ["c:/installers/ChromeStandaloneSetup64.exe", "/silent", "/install"]
# RUN ["c:/installers/Firefox Setup 66.0.3.exe", "-ms"]

# BAKE THE IMAGE HERE

