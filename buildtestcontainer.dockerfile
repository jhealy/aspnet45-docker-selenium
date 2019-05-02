# adding in tests
FROM aspnet48

# Install chocolately, pretty much game on after that

RUN mkdir seleniumtests 
# RUN mkdir seleniumtests/assets

# copy in our website
COPY ./MvcHelloWorld45/publish/ /inetpub/wwwroot/

# copy in the tests
COPY ./SeleniumDockerTest/bin/Release/ /seleniumtests/

# chromedriver is bundled into the official nuget
# make sure geckodriver is there with the selenium tests
COPY ./assets/geckodriver-v0.24.0-win64/ /seleniumtests/