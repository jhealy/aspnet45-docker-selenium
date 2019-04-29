# FROM microsoft/aspnet
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8

LABEL maintainer='jhealy at microsoft.com'

# choco download failing behind msft proxy.  going the assets download route
# fail refs out to https://onegetcdn.azureedge.net/providers/providers.masterList.feed.swidtag - need to resolve
# choco chrome takes a bit
# RUN echo 'Downloading chocolatey...'
# RUN powershell -Command Get-PackageProvider -name chocolately
# RUN powershell -Command Install-PackageProvider -name chocolatey -Force
# RUN powershell -Command Set-PackageSource -Name chocolatey -Trusted
# RUN powershell -Command Get-PackageSource
# RUN echo 'Installing Chrome via chocolatey...'
# RUN powershell -Command Install-Package GoogleChrome -MinimumVersion 74
# RUN echo 'Installing Firefox via chocolately'
# RUN powershell -command Install-Package firefox

# Copy over firefox and chrome installers
# You need an 'installers' directory in at the same level of the docker file
# To use below chrome installer should be renamed chrome-installer.exe, firefox installer should be firefox-installer.exe
# OR just modify below to point to your specific exe
# chrome installer download - extract out - https://www.google.com/chrome/?standalone=1
# firefox installer download - https://www.mozilla.org/en-US/firefox/all/


RUN mkdir installers 

COPY ./installers/ /installers

RUN ["c:/installers/ChromeStandaloneSetup64.exe", "/silent", "/install"]

RUN ["c:/installers/Firefox Setup 66.0.3.exe", "-ms"]


# BAKE THE IMAGE HERE

