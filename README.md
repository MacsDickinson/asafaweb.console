asafaweb.console
================

A simple console app that runs the asafaweb tests and scrapes the results. The purpose of this is to enable an automated post publish security check through CI platforms such as NANT and MSBuild.

Updates
-------

| Version | Details | Download |
| -------:| -------:| --------:|
| 0.0.2   | Updated to use the asafaweb api. Api username and key must be specified | http://bit.ly/129p319 |
| 0.0.1   | Initial application that uses screen scraping | http://bit.ly/11a2NdH |

Usage
-----

```asafaweb.console [OPTIONS]+```

Example:

```asafaweb.console -n=macs -k=abc123 -url=www.google.com -fw -ignore=CustomErrors```

Implementation using NAnt

```xml
<property name="asafaweb.console.exe" value="C:\YourLocalPathTo\asafaweb.console.exe" />
<property name="asafaweb.api.name" value="username" />
<property name="asafaweb.api.key" value="apikey" />
<property name="deployment.url" value="http://www.your-shiny-new-website.com/" />
  
<target name="asafaweb">
    <exec failonerror="true" program="${asafaweb.console.exe}" verbose="true">
      <arg value="-n:${asafaweb.api.name}" />
      <arg value="-k:${asafaweb.api.key}" />
      <arg value="-u:${deployment.url}" />
      <arg value="-f" />
      <arg value="-i:CustomErrors,StackTrace" />
    </exec>
</target>
```

Options
-------

| Option            | Type                | Details                             | Usage               |
| -----------------:|:-------------------:| -----------------------------------:| -------------------:|
| n&#124;name=           | string **required** | Your asafaweb username              | -n=macs             |
| k&#124;key=            | string **required** | your asafaweb api key               | -k=abc123           |
| u&#124;url=            | string **required** | The url to run the tests against    | -url=www.google.com |
| f&#124;failonfailures  | standalone          | Fail the test if any failures occur | -f                  |
| w&#124;failonwarning   | standalone          | Fail the test if any warnings occur | -w                  |
| n&#124;failonnottested | standalone          | Fail if any tests arn't completed   | -n                  |
| i&#124;ignore=         | Comma seperated list| Ignore any of the tests specified   | -i=Tracing          |

Tests
-----

This app simply runs the tests on www.asafaweb.com and scrapes the results so the tests that are run are likely to change. At the time of writing the tests include:

* Tracing  
* CustomErrors  
* StackTrace  
* RequestValidation  
* HttpToHttps  
* HashDosPatch  
* ElmahLog  
* ExcessiveHeaders  
* HttpOnlyCookies  
* SecureCookies  
* Clickjacking 