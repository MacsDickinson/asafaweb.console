asafaweb.console
================

A simple console app that runs the asafaweb tests and scrapes the results. The purpose of this is to enable an automated post publish security check through CI platforms such as NANT and MSBuild.

Download only the required files here: http://bit.ly/10H9PRr (v0.0.1)

Usage
-----

```asafaweb.console [OPTIONS]+```

Example:

```asafaweb.console -url=www.google.com -fw -ignore=CustomErrors```

Implementation using NAnt

```xml
<property name="asafaweb.console.exe" value="C:\YourLocalPathTo\asafaweb.console.exe" />
<property name="deployment.url" value="http://www.your-shiny-new-website.com/" />
  
<target name="asafaweb">
    <exec failonerror="true" program="${asafaweb.console.exe}" verbose="true">
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
| u|url=            | string **required** | The url to run the tests against    | -url=www.google.com |
| f|failonfailures  | standalone          | Fail the test if any failures occur | -f                  |
| w|failonwarning   | standalone          | Fail the test if any warnings occur | -w                  |
| n|failonnottested | standalone          | Fail if any tests arn't completed   | -n                  |
| i|ignore=         | Comma seperated list| Ignore any of the tests specified   | -i=Tracing          |

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