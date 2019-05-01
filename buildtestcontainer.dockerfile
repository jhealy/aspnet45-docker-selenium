# adding in tests
FROM aspnet48

RUN mkdir seleniumtests 
RUN mkdir seleniumtests/assets

# The final instruction copies the site you published earlier into the container.
# COPY ./bin/Publishoutput/ /inetpub/wwwroot
COPY ./MvcHelloWorld45/MvcHelloWorld45/bin/Publish/ /inetpub/wwwroot/

# copy in the tests
COPY ./SeleniumDockerTest/bin/Release/ /seleniumtests/

# chromedriver is bundled into the official nuget
# make sure geckodriver is there with the selenium tests
COPY ./assets/geckodriver-v0.24.0-win64/ /seleniumtests/

# temp just to shove in ps installer
COPY ./installers/ /installers/

COPY ./assets/Pester-4.7.3/ C:/Program Files/WindowsPowerShell/Modules/Pester/