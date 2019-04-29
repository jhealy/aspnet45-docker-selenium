# The `FROM` instruction specifies the base image. We are extending the `microsoft/aspnet` image.
# FROM microsoft/aspnet
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8

# EXPOSE 49247
# this will keep ff browser from starting in container - no bueno

# EASY ACCESS TO TEST CHANGES
# Need to mount it in the post run actions
# https://blog.sixeyed.com/docker-volumes-on-windows-the-case-of-the-g-drive/

#escape='
# work on this later....
# VOLUME C:/dev/devfish-shares.git/docker-selenium-aspnet45 -d "hostdrive"

RUN mkdir seleniumtests 
RUN mkdir seleniumtests/assets

# The final instruction copies the site you published earlier into the container.
# COPY ./bin/Publishoutput/ /inetpub/wwwroot
COPY ./MvcHelloWorld45/MvcHelloWorld45/bin/Publish/ /inetpub/wwwroot

# copy in the tests
COPY ./SeleniumDockerTest/bin/Release/ /seleniumtests 

# chromedriver is bundled into the official nuget
# make sure geckodriver is there with the selenium tests
COPY ./assets/geckodriver-v0.24.0-win64/ /seleniumtests

# feeble attempt, faster than choco
# COPY ./assets/GoogleChromeEnterpriseBundle64/Installers /seleniumtests/assets
COPY ./assets/firefox /seleniumtests/assets

RUN [ "c:/seleniumtests/assets/firefox_setup_66.0.3.exe", "-ms"]

# choco chrome takes a bit
# RUN echo 'Downloading chocolatey...'
# RUN powershell -Command Install-PackageProvider -name chocolatey -Force
# RUN powershell -Command Set-PackageSource -Name chocolatey -Trusted
# RUN powershell -Command Get-PackageSource

# RUN echo 'Install Chrome via chocolatey...'
# RUN powershell -Command Install-Package GoogleChrome -MinimumVersion 74

# at this point lets verify all our files got copied in

# run the chrome installer manually via ps
# verify chrome installer is there
# verify selenium tests from above

# run the chrome installer [cmd or run]
# bake the image

# copy in selenium tests to new image
# run the selenium tests [CMD]