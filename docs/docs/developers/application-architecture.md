
# Application Architecture
The SiteWatchman Api follows the practices, patterns and principals prescribed by [Clean Architecture](https://app.pluralsight.com/library/courses/clean-architecture-patterns-practices-principles/table-of-contents) as presented by Pluralsight, which as heavily inspired by [Uncle Bob Martin](https://twitter.com/unclebobmartin) who is writing a [new book](https://www.amazon.com/Clean-Architecture-Robert-C-Martin/dp/0134494164) on the topic. In addition the naming conventions and code structure is also influenced by [Domain Driven Design](https://www.amazon.com/Patterns-Principles-Practices-Domain-Driven-Design/dp/1118714709/ref=pd_sbs_14_img_1?_encoding=UTF8&psc=1&refRID=GFJH8N8JD8NX1N7AYXT8) practices as well.

The Clean Architecture is used to structure the project and Domain Driven Design practices are used to implement the actual code.

## Diagram
![](..\imgs\application-architecture.png)

## Layers, Components & Responsibilities

### Api (Service) Layer
The api layer is what exposes our application to the world. It is the entry point into the system from the api client perspective. We have based our api on [ServiceStack](http://docs.servicestack.net/).


#### Components and their responsibilities
The responsibilities of the api are scoped to just those activities that relate to exposing the functionality of the platform to the outside world. This includes defining requests & responses along with enforcing security (authentication, authorization) and capturing broad performance metrics that would be representative of the api client's perspective and enforcing SSL only communication. The following components are a part of the Api Layer:  


* **Requests** - blah  
* **Responses** - blah  
* **Performance Metrics Attributes** - blah  
* **Request Filters** - blah
    * Performance Metrics
        * Count of api calls made
        * Duration of api calls
        * Amount of data returned by api calls
    * Identifying the calling application
    * Identifying the user
    * Identifying the environment
    * Identifying the tenant
    * Enforcing SSL

### Application Layer
#### Components and their responsibilities

### Domain Layer
#### Components and their responsibilities

### Persistance Layer
#### Components and their responsibilities

### Infrastructure Layer
#### Components and their responsibilities

### Common Layer
#### Components and their responsibilities