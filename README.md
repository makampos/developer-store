# Developer Store Project

## Use Case
**You are a developer on the DeveloperStore team. Now we need to implement the API prototypes.**

As we work with `DDD`, to reference entities from other domains, we use the `External Identities` pattern with denormalization of entity descriptions.

Therefore, you will write an API (complete CRUD) that handles sales records. The API needs to be able to inform:

* Sale number
* Date when the sale was made
* Customer
* Total sale amount
* Branch where the sale was made
* Products
* Quantities
* Unit prices
* Discounts
* Total amount for each item
* Cancelled/Not Cancelled

It's not mandatory, but it would be a differential to build code for publishing events of:
* SaleCreated
* SaleModified
* SaleCancelled
* ItemCancelled

If you write the code, **it's not required** to actually publish to any Message Broker. You can log a message in the application log or however you find most convenient.

### Business Rules

* Purchases above 4 identical items have a 10% discount
* Purchases between 10 and 20 identical items have a 20% discount
* It's not possible to sell above 20 identical items
* Purchases below 4 items cannot have a discount

These business rules define quantity-based discounting tiers and limitations:

1. Discount Tiers:
   - 4+ items: 10% discount
   - 10-20 items: 20% discount

2. Restrictions:
   - Maximum limit: 20 items per product
   - No discounts allowed for quantities below 4 items

---

## Instructions to Prepare and Run the Project
To set up and run the project, follow these steps:

#### Prerequisites
Ensure you have Docker installed on your machine.


#### Step 1: Raise Containers
You need to start the containers for the database and RabbitMQ. Execute the following commands in the root folder of your project:

```
docker-compose --project-name ambev-db up -d ambev.developerevaluation.database
docker-compose --project-name ambev-bus up -d ambev.developerevaluation.rabbitmq
```

#### Step 2: Update the Database
After raising the containers, update the database using Entity Framework Core. You can do this in one of two ways:
Using Rider IDE:
Navigate to Tools > Entity Framework Core > Update Database.
Using Command Line:
Run the following command in the root folder:

```
/usr/local/share/dotnet/dotnet ef database update --project src/Ambev.DeveloperEvaluation.ORM/Ambev.DeveloperEvaluation.ORM.csproj --startup-project src/Ambev.DeveloperEvaluation.WebApi/Ambev.DeveloperEvaluation.WebApi.csproj --context Ambev.DeveloperEvaluation.ORM.DefaultContext
```

#### Step 3: Run the Web API Project 
Select the project `Ambev.DeveloperEvaluation.WebApi` in your IDE and run it. This will start the Web API service.

#### Step 4: Access Swagger UI
Once the Web API is running, navigate to `http://localhost:5119/swagger/index.html` in your web browser. This page will display all available routes through Swagger, allowing you to test and interact with your API endpoints.

#### Step 5: Ready to Go
Your environment is now set up, and you can start working with the project. You can use Swagger to explore different APIs and test their functionality.
