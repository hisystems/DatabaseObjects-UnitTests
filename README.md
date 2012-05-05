DatabaseObjects-UnitTests
=========================

Overview
--------
The DatabaseObjects-UnitTests repository contains all of the unit tests and examples for the [DatabaseObjects library](https://github.com/hisystems/DatabaseObjects).

To run the unit tests project in conjunction with the library it must be located in the same directory as the library.

For example:

/DatabaseObjects
/DatabaseObjects.UnitTests

License
-------
The library can be used for commercial and non-commercial purposes.

Documentation
-------------
All documentation and other information for the DatabaseObjects library is available on the website [www.hisystems.com.au/databaseobjects](http://www.hisystems.com.au/databaseobjects)

Setup
-----

### Setup DatabaseObjects.UnitTestExtensions

1. Copy the DatabaseObjects.UnitExtensions to C:\Program Files\Microsoft Visual Studio 10.0\Common7\IDE\PublicAssemblies
2. Run the following with 'regedit.exe %1' to register the DatabaseObjects.UnitExtensions in Visual Studio 2010: 

Windows Registry Editor Version 5.00

[HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\VisualStudio\10.0\EnterpriseTools\QualityTools\TestTypes\{13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b}\TestTypeExtensions\DatabaseTestClassAttribute]
"AttributeProvider"="DatabaseObjects.UnitTestExtensions.DatabaseTestClassAttribute, DatabaseObjects.UnitTestExtensions"

[HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\VisualStudio\10.0\EnterpriseTools\QualityTools\TestTypes\{13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b}\TestTypeExtensions\DatabaseTestClassAttribute]
"AttributeProvider"="DatabaseObjects.UnitTestExtensions.DatabaseTestClassAttribute, DatabaseObjects.UnitTestExtensions"

[HKEY_CURRENT_USER\SOFTWARE\Microsoft\VisualStudio\10.0_Config\EnterpriseTools\QualityTools\TestTypes\{13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b}\TestTypeExtensions\DatabaseTestClassAttribute]
"AttributeProvider"="DatabaseObjects.UnitTestExtensions.DatabaseTestClassAttribute, DatabaseObjects.UnitTestExtensions"

[HKEY_CURRENT_USER\SOFTWARE\Wow6432Node\Microsoft\VisualStudio\10.0_Config\EnterpriseTools\QualityTools\TestTypes\{13cdc9d9-ddb5-4fa4-a97d-d965ccfc6d4b}\TestTypeExtensions\DatabaseTestClassAttribute]
"AttributeProvider"="DatabaseObjects.UnitTestExtensions.DatabaseTestClassAttribute, DatabaseObjects.UnitTestExtensions"

3. Restart Visual Studio