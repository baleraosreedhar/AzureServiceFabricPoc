# AzureServiceFabricPoc
A POC for azure service fabric with .net core

A axure service fabric .NET core example with 2 statefull services and 1 stateless service in the cluster

Install Visual Studio 2017
Install dot net SDK 2.02

UI : Stateless Service
AgeService: Statefull service with reliable collections .NET core
Charcount: Statefull service

UI Invokes Age service and char count service

Testing: Open Postman app on chrome and invoke the age service as 
http://Domain.eastus.cloudapp.azure.com:19081/MyCalculatorApplication/TechnicBirthdayAgeService/api/BirthdayCalculator/Calculate?PartitionKind=Int64Range&PartitionKey=1
and in the Body of the request pass in Key:birthdayvalye and Value: Valid Birthdate



