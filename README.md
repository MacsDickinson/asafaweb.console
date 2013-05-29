asafaweb.console
================

A simple console app to run the asafaweb tests. The purpose of this is to enable an automated post publish security step through CI platforms such as NANT ans MSBuild.

Usage
-----

```asafaweb.console [OPTIONS]+```

Example:

```asafaweb.console -url=www.google.com -fw -ignore=CustomErrors```

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