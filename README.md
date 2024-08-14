## Smart BI Analytics

.NET Core + React fullstack project

**Frontend**: https://github.com/Koksheng/kokshengbi-frontend

**Backend**: https://github.com/Koksheng/kokshengbi-backend

## Project Description

An intelligent data analysis platform based on .NET Core + MQ + AIGC + React. 

Unlike traditional BI, users only need to upload raw data and input their analysis goal to automatically generate analysis conclusions and visual charts, thereby reducing costs and increasing efficiency in data analysis.

## Tech Stack

### Frontend
- **Framework**: React, Umi
- **Scaffolding**: Ant Design Pro
- **Component library**: Ant Design, Ant Design Components
- **Plugin**: OpenAPI frontend code generation

### Backend
- **Framework**: .NET Core 
- **Database**: Microsoft SQL Server
- **OpenAI API**: Model gpt-3.5-turbo (AIGC)
- **Rate Limiting**: Redis (limits the same user to 2 requests per second for the same function)
- **Message Queue**: RabbitMQ 

## Login
![image](https://github.com/user-attachments/assets/dcafe35b-f0ea-4df0-a1b4-728d49ccf1e2)

## Register
![image](https://github.com/user-attachments/assets/589c90d9-56ff-438b-b9eb-7ebc55fa6ce4)

## Intelligent Analysis 
![image](https://github.com/user-attachments/assets/4bc151ed-a3fb-4a67-91c9-be1bfa9049de)

## Intelligent Analysis (Async): (Pending)

## My Chart
![image](https://github.com/user-attachments/assets/944e9950-7b63-4fef-bbd0-13ae0de64d7f)


## Project Structure: (Pending)
Infrastructure: The client analyzes input requests and raw data, sending requests to the business backend. The business backend uses AI services to process client data, stores it in the database, and generates charts. The processed data is sent from the business backend to the AI service, which generates results and returns them to the backend, ultimately sending the results back to the client for display.




### kokshengbi-backend.sln
**Solution File**: This is the main solution file that includes all the backend projects.

### kokshengbi.Api (Start)
**Function**: This project likely serves as the main API application.

**Features**:
Exposes different endpoints for clients.
Handles incoming HTTP requests and processes them.
Includes controllers, such as the UserController, ChartController.

### kokshengbi.Application
**Function**: This project contains the application logic and services.

**Features**:
Implements business rules.
Provides application services that interact with the domain and infrastructure layers.
Contains interfaces and implementations for various operations needed by the **kokshengbi.Api**.

### kokshengbi.Contracts
**Function**: This project defines the data transfer objects (DTOs) and contracts used for communication.

**Features**:
Defines request and response models for API endpoints.
Ensures consistency in the data exchanged between the client and server.

### kokshengbi.Domain
**Function**: This project contains the domain models and business logic.

**Features**:
Defines the core entities and value objects.
Implements domain services and business rules.
Contains domain events and other domain-related logic.

### kokshengbi.Infrastructure
**Function**: This project handles the infrastructure concerns like data access and external service integrations.

**Features**:
Implements repositories and data context for database interactions.
Contains configurations and setups for external services.
Handles data persistence and retrieval operations.

**Services**:
- **OpenAiService**: Manages interactions with the OpenAI API.
- **RedisRateLimiterService**: Implements rate limiting for API requests.
- **BiMessageProducer**: Sends messages to RabbitMQ.
- **BiMessageConsumer**: Consumes messages from RabbitMQ.


